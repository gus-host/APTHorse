using System.Collections;
using Aptos.Accounts;
using Aptos.BCS;
using Aptos.HdWallet.Utils;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using Aptos.Unity.Sample.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nftNameText;
    [SerializeField] private TextMeshProUGUI nftPriceText;
    [SerializeField] private TextMeshProUGUI nftDescText;
    [SerializeField] private Image nftImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private float nftPrice;
    private bool owns;
    private bool equipped;
    private ulong id;
    private SpinnerManager spinnerManager;

    private void Start()
    {
        buyButton.onClick.AddListener(() => StartCoroutine(BuyNFT()));
        equipButton.onClick.AddListener(() => StartCoroutine(EquipNFT()));
        unequipButton.onClick.AddListener(() => StartCoroutine(EquipNFT(1000)));
    }

    public void SetupNFT(string nftName, string nftDesc, int nftPrice, Sprite nftSprite, ulong id, bool owns, bool equipped)
    {
        spinnerManager = FindObjectOfType<SpinnerManager>();
        this.nftPrice = nftPrice;
        this.id = id;
        this.owns = owns;
        this.equipped = equipped;

        nftNameText.text = nftName;
        nftDescText.text = nftDesc;
        nftPriceText.text = $"{AptosUILink.Instance.AptosTokenToFloat(nftPrice)} APT";
        nftImage.sprite = nftSprite;

        CheckOwnsAndSetButtons();
    }

    private void CheckOwnsAndSetButtons()
    {
        equipButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        if (owns)
        { 
            if(equipped) unequipButton.gameObject.SetActive(true);
            else equipButton.gameObject.SetActive(true);
        }
        else buyButton.gameObject.SetActive(true);

        spinnerManager.HideMessage();
    }

    private IEnumerator BuyNFT()
    {
        if (WalletManager.Instance.APTBalance < nftPrice)
        {
            Debug.LogError("APT Balance too low!");
            yield break;
        }

        spinnerManager.ShowMessage($"Buying {nftNameText.text}...");

        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new U64(id) });

        var payload = new EntryFunction
        (
            new(new AccountAddress(bytes), "aptos_horses"),
            "mint_horse",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction buyTx = new();
        Coroutine buy = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            buyTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return buy;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot Buy. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + buyTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, buyTx.Hash)
        );
        yield return waitForTransactionCor;

        Debug.Log(responseInfo.status);

        if (responseInfo.status == ResponseInfo.Status.Success) StartCoroutine(FindObjectOfType<MarketplaceManager>().GetMarketplaceDataAsync());
        else
        {
            Debug.Log(responseInfo.message);
            spinnerManager.HideMessage();
        } 
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }

    private IEnumerator EquipNFT(ulong equip_id = 10000000)
    {
        spinnerManager.ShowMessage($"{(equip_id == 1000 ? "Unequipping" : "Equipping")} {nftNameText.text}...");
        ResponseInfo responseInfo = new();

        byte[] bytes = "dafe19420f798da33a13a5928202ee55f812b1d4666aad6e0f66dedd6daefead".ByteArrayFromHexString();
        Sequence sequence = new(new ISerializable[] { new U64(equip_id == 10000000 ? id : equip_id) });

        var payload = new EntryFunction
        (
            new(new AccountAddress(bytes), "aptos_horses_user"),
            "equip_horse",
            new(new ISerializableTag[] { }),
            sequence
        );

        Transaction equipTx = new();
        Coroutine equip = StartCoroutine(RestClient.Instance.SubmitTransaction((_transaction, _responseInfo) =>
        {
            equipTx = _transaction;
            responseInfo = _responseInfo;
        },
        WalletManager.Instance.Wallet.Account,
        payload));
        yield return equip;

        if (responseInfo.status != ResponseInfo.Status.Success)
        {
            Debug.LogError("Cannot Equip. " + responseInfo.message);
            yield break;
        }

        Debug.Log("Transaction: " + equipTx.Hash);
        Coroutine waitForTransactionCor = StartCoroutine(
            RestClient.Instance.WaitForTransaction((_pending, _responseInfo) =>
            {
                responseInfo = _responseInfo;
            }, equipTx.Hash)
        );
        yield return waitForTransactionCor;

        Debug.Log(responseInfo.status);

        if (responseInfo.status == ResponseInfo.Status.Success) StartCoroutine(FindObjectOfType<MarketplaceManager>().GetMarketplaceDataAsync());
        else
        {
            spinnerManager.HideMessage();
            Debug.Log(responseInfo.message);
        }
        yield return new WaitForSeconds(1);
        AptosUILink.Instance.LoadCurrentWalletBalance();
    }
}
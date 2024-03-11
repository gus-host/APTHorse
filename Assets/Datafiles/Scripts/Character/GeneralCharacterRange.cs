using UnityEngine;

public class GeneralCharacterRange : MonoBehaviour
{
    public enum CharacterType
    {
        Celorian,
        CelorianTypeTwo,
        CelorianTypeThree,
        TypeTwo
    }
    public CharacterType _characterType;
    public GeneralCharacterManager CharacterManager;

    public GameObject _smoke;
    public GameObject _goldSmoke;
    private void Start()
    {
        CharacterManager = GetComponentInParent<GeneralCharacterManager>();
        if(CharacterManager._characterType == GeneralCharacterManager.CharacterType.Celorian)
        {
            _characterType = CharacterType.Celorian;
        }else if (CharacterManager._characterType == GeneralCharacterManager.CharacterType.CelorianTypeTwo)
        {
            _characterType = CharacterType.CelorianTypeTwo;
        }
        else if (CharacterManager._characterType == GeneralCharacterManager.CharacterType.CelorianTypeThree)
        {
            _characterType = CharacterType.CelorianTypeThree;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
        {
            if (player._llumanaandMalumanaMagic.activeSelf)
            {
                if (_characterType == CharacterType.Celorian)
                {
                    _smoke.SetActive(true);
                }
                else if(_characterType == CharacterType.CelorianTypeTwo){
                    _goldSmoke.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            if (_characterType == CharacterType.Celorian)
            {
                _smoke.SetActive(false);
            }
            else if (_characterType == CharacterType.CelorianTypeTwo)
            {
                _goldSmoke.SetActive(false);
            }
        }
    }
}

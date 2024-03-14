using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;

public class FetchValuesResponse
{
    public int value1 { get; set; }
    public int value2 { get; set; }
}
public class CoinManager : MonoBehaviour
{
/*    public static CoinManager instance;

    [SerializeField]
    private TextMeshProUGUI _inGameCoin;

    public int _inGameCoinVar = 0;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        await UpdateValues();

        
    }

    async Task UpdateValues(int val1 = 1, int val2 = 2500)
    {
        HttpClient client = new HttpClient();

        string apiUrl = "http://in-game-coin.onrender.com/update";
        var values = new
        {
            value1 = val1,
            value2 = val2
        };

        string json = JsonConvert.SerializeObject(values);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(apiUrl, content);
        response.EnsureSuccessStatusCode();

        Debug.Log("Values updated successfully.");
        await FetchValues();
    }

    async Task FetchValues()
    {
        HttpClient client = new HttpClient();

        string apiUrl = "http://in-game-coin.onrender.com/fetch";
        HttpResponseMessage response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        FetchValuesResponse values = JsonConvert.DeserializeObject<FetchValuesResponse>(json);

        int value1 = values.value1;
        int value2 = values.value2;
        _inGameCoinVar = value2;

        Debug.Log("Value 1: " + value1);
        Debug.Log("Value 2: " + value2);

        //Display coins
        _inGameCoin.text = _inGameCoinVar.ToString();
    }

    private void Update()
    {
        //Display coins
        _inGameCoin.text = _inGameCoinVar.ToString();
    }

    public async void InitiateDeductAndUpdate(int val1, int val2)
    {
        await UpdateValues(val1,_inGameCoinVar - val2);
    }
*/
}

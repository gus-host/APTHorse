using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MyApiRequest : MonoBehaviour
{
    private string apiUrl = "http://45.79.126.10:3011/api/get-all_items"; // Replace with your API URL
    private string registerWallerAPI = "https://charged-gravity-390312.an.r.appspot.com/api/register";
    public GameObject parent;
    public GameObject contentOne;
    public GameObject contentTwo;
    
    private void Start()
    {
        //Get item
        StartCoroutine(GetDataFromApi());
        
        //Register wallet
        StartCoroutine(PostRequest(registerWallerAPI));
    }


    #region GET ITEMS

        private IEnumerator GetDataFromApi()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Request successful, parse the JSON response
                string json = webRequest.downloadHandler.text;
                Debug.Log("Response: " + json);

                // Deserialize the JSON into a custom data structure
                ProductsResponse productsResponse = JsonUtility.FromJson<ProductsResponse>(json);

                // Now you can access the parsed data
                GameObject _contentOne = Instantiate(contentOne, parent.transform);
                foreach (Product product in productsResponse.product)
                {
                    //Debug.Log("Product ID: " + product._id);
                    //Debug.Log("Product Title: " + product.title);
                    if (_contentOne.GetComponent<ContentOne>().childCount < 5)
                    {
                        GameObject _contentTwo = Instantiate(contentTwo, _contentOne.transform);
                        _contentOne.GetComponent<ContentOne>().childCount++;
                    }else if (_contentOne.GetComponent<ContentOne>().childCount >= 5)
                    {
                        _contentOne = Instantiate(contentOne, parent.transform);
                    }
                    // Add more fields as needed
                }
            }
        }
    }

    // Define a custom data structure to match the JSON structure
    [System.Serializable]
    public class Product
    {
        public string _id;
        public string title;
        public string description;
        public string itemId;
        public int prize;
        public string image;
        public int __v;
    }

    [System.Serializable]
    public class ProductsResponse
    {
        public Product[] product;
    }

    #endregion


    #region Register Wallet
    [System.Serializable]
    public class RegisterData
    {
        public string wallet_address;
    }

    [System.Serializable]
    public class WalletRes
    {
        public bool success;
        public string msg;
        public string token;
    }
    IEnumerator PostRequest(string uri)
    {
        // Create a RegisterData object with the desired JSON data
        RegisterData data = new RegisterData
        {
            wallet_address = "BAiAg6prUmaQ4P39wBdNyB6iRASm4yym39iJT93BjQtK"
        };

        // Serialize the object to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Create a UnityWebRequest object for the POST request
        UnityWebRequest uwr = new UnityWebRequest(uri, "POST");

        // Set the request headers (content type and authorization if needed)
        uwr.SetRequestHeader("Content-Type", "application/json");

        // Attach the JSON data to the request body
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        // Send the request
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            string res = uwr.downloadHandler.text;
            WalletRes walletRes = JsonUtility.FromJson<WalletRes>(res);
            Debug.LogError($"{walletRes.success}");
            Debug.LogError($"{walletRes.msg}");
            Debug.LogError($"{walletRes.token}");
            PlayerPrefs.SetString("Token",walletRes.token);
        }
    }

    #endregion

}
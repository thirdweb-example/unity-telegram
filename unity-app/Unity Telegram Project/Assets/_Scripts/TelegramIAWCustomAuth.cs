using Newtonsoft.Json;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TelegramIAWCustomAuth : MonoBehaviour
{
    [field: SerializeField]
    private Button WalletButton;

    [field: SerializeField]
    private TMP_Text LogText;

    private void Start()
    {
        Log("Waiting for payload...");
        var url = Application.absoluteURL;
        var uri = new System.Uri(url);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var payload = query.Get("payload");
        var payloadDecoded = System.Web.HttpUtility.UrlDecode(payload);
        var payloadObject = JsonConvert.DeserializeObject<Payload>(payloadDecoded);
        Log($"Payload: {JsonConvert.SerializeObject(payloadObject)}");
        ProcessPayload(payloadObject);
    }

    private async void ProcessPayload(Payload payload)
    {
        var connection = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 421614,
            inAppWalletOptions: new InAppWalletOptions(authprovider: AuthProvider.AuthEndpoint, jwtOrPayload: JsonConvert.SerializeObject(payload)),
            smartWalletOptions: new SmartWalletOptions(sponsorGas: true)
        );
        Log("Connecting wallet...");
        try
        {
            var smartWallet = await ThirdwebManager.Instance.ConnectWallet(connection);
            var address = await smartWallet.GetAddress();
            WalletButton.GetComponentInChildren<TMP_Text>().text = address[..4] + "..." + address[^4..];
            WalletButton.onClick.AddListener(() =>
            {
                Application.OpenURL("https://sepolia.arbiscan.io/address/" + address);
            });
            Log("Connected!");
        }
        catch (System.Exception e)
        {
            Log(e.ToString());
        }
    }

    private void Log(string message)
    {
        LogText.text = message;
        Debug.Log(message);
    }

    [System.Serializable]
    public class Payload
    {
        public string signature;
        public string message;
    }
}

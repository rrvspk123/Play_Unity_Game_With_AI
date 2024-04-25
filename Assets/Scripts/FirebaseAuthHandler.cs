using Proyecto26;
using UnityEngine;
using System.Collections;

/// <summary>
/// Handles authentication calls to Firebase
/// </summary>

[System.Serializable]
class SignInResponseData
{
    public string federatedId;
    public string providerId;
    public string email;
    public bool emailVerified;
}

public static class FirebaseAuthHandler
{
    private const string ApiKey = "?"; //TODO: Change [API_KEY] to your API_KEY
    public static string GoogleEmail;

    /// <summary>
    /// Signs in a user with their Id Token
    /// </summary>
    /// <param name="token"> Id Token </param>
    /// <param name="providerId"> Provider Id </param>
    public static void SingInWithToken(string token, string providerId)
    {
        var payLoad =
            $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"http://localhost\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";
        RestClient.Post($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}", payLoad).Then(
            async response =>
            {
                // 解析响应数据
                var responseData = JsonUtility.FromJson<SignInResponseData>(response.Text);

                GameObject playerFileObject = GameObject.Find("PlayerFile");
                PlayerFile playerFile = playerFileObject.GetComponent<PlayerFile>();

                // 检查emailVerified字段
                if (responseData.emailVerified)
                {
                    GoogleEmail = responseData.email;
                    PlayerPrefs.SetString("Email", GoogleEmail);
                    // 执行下一步动作
                    Debug.Log("Email is verified. Proceed with the next step.");

                    playerFile.OnSubmit();
                    
                }
                else
                {
                    Debug.Log("Email is not verified.");
                }
            }).Catch(Debug.Log);    
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles test UIs present in the SignIn Scene
/// </summary>

public class UIHandler : MonoBehaviour
{
    public TMP_InputField code;
    public void OnClickGetGoogleCode()
    {
        GoogleAuthenticator.GetAuthCode();
    }

    public void OnClickGoogleSignIn()
    {
        GoogleAuthenticator.ExchangeAuthCodeWithIdToken(code.text, idToken =>
            {
                FirebaseAuthHandler.SingInWithToken(idToken, "google.com");
            });
    }
}

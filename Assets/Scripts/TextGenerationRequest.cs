using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using Newtonsoft.Json; // For JsonConvert
using System.Text; // For Encoding
using System.Text.Json;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class ResponseData
{
    public string response_text;
}

public class TextGenerationRequest : MonoBehaviour
{
    public TextMesh BackgroundMesh;
    public TextMesh Q2Mesh;
    public TextMesh Q3Mesh;
    public TextMesh Q4Mesh;
    public TextMesh Q5Mesh;
    public TextMesh Q6Mesh;
    public TextMeshPro SpeechToText;
    
    public TextMesh TalkMesh;
    public GameObject NPC_Background;
    public TMP_InputField promptInputField;

    public GameObject G_Record;
    public GameObject G_StopRecord;
    public GameObject G_Submit;

    public static string voiceText;

    private const string functionUrl = "?";
    private const string functionUrl_chat = "?";
    private const string functionUrl_TextToSpeech = "?";
    private const string functionUrl_Firestore_Question = "?";

    private string idToken; // Question Spawn
    private string idToken_2; // ChatAI
    private string idToken_3; // TextToSpeech
    private string idToken_4; // Firestore Question
    public string currentSceneName { get; private set; }

    private void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(RetrieveIdTokens());
        
        // 将StopRecord设置为不可见
        if (G_StopRecord != null)
        {
            G_StopRecord.SetActive(false);
        }

        if (G_Submit != null)
        {
            G_Submit.SetActive(false);
        }
    }

    private IEnumerator RetrieveIdTokens()
    {   
        string targetUrl = "?";
        string target_audience = "?";
        string target_audience_2 = "?";
        string target_audience_3 = "?";
        string target_audience_4 = "?";
        
        if (currentSceneName == "1-1")
        {
            SpeechToText.text = "普通话测试 - 请读出 : \n绿水青山就是金山银山";
            Debug.Log("1-1");
            yield return StartCoroutine(GetIdToken(targetUrl, target_audience, token => idToken = token)); // Background or Q1
            yield return StartCoroutine(GetIdToken2(targetUrl, target_audience_2, token => idToken_2 = token)); // ChatAI
            yield return StartCoroutine(GetIdToken3(targetUrl, target_audience_3, token => idToken_3 = token)); // TextToSpeech
            yield return StartCoroutine(GetIdToken4(targetUrl, target_audience_4, token => idToken_4 = token)); // Firestore Question


            if (idToken != null)
            {
                yield return StartCoroutine(MakeRequest(idToken));
            }

            if (idToken_4 != null)
            {
                yield return StartCoroutine(FirestoreRequest_Q2(idToken_4));
                yield return StartCoroutine(FirestoreRequest_Q3(idToken_4));
            }

        } else if (currentSceneName == "1-2") {
            Debug.Log("1-2");
            yield return StartCoroutine(GetIdToken3(targetUrl, target_audience_3, token => idToken_3 = token)); // TextToSpeech
            yield return StartCoroutine(GetIdToken4(targetUrl, target_audience_4, token => idToken_4 = token)); // Firestore Question
        
            if (idToken_4 != null)
            {
                yield return StartCoroutine(FirestoreRequest_Q2(idToken_4));
                yield return new WaitForSeconds(5f); // Pause for 5 seconds

                yield return StartCoroutine(FirestoreRequest_Q3(idToken_4));
                yield return new WaitForSeconds(5f); // Pause for 5 seconds
                
                yield return StartCoroutine(FirestoreRequest_Q4(idToken_4));
                yield return new WaitForSeconds(5f); // Pause for 5 seconds
            }
        }
        
    }

    private IEnumerator GetIdToken(string targetUrl, string target_audience, Action<string> callback)
    {
        var payload = new
        {
            target_audience = target_audience
        };

        string jsonPayload = JsonConvert.SerializeObject(payload);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);

        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(targetUrl, "");
        webRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
        webRequest.SetRequestHeader("Content-Type", "application/json");
    
        yield return webRequest.SendWebRequest();
    
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            callback?.Invoke(webRequest.downloadHandler.text);
            Debug.Log("Id_Token is successfully retrieved");
        }
    }
    
    private IEnumerator GetIdToken2(string targetUrl, string target_audience2, Action<string> callback)
    {
        var payload = new
        {
            target_audience = target_audience2
        };
    
        string jsonPayload = JsonConvert.SerializeObject(payload);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
    
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(targetUrl, "");
        webRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
        webRequest.SetRequestHeader("Content-Type", "application/json");
    
        yield return webRequest.SendWebRequest();
    
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            callback?.Invoke(webRequest.downloadHandler.text);
            Debug.Log("Id_Token 2 is successfully retrieved");
        }
    }
    
    private IEnumerator GetIdToken3(string targetUrl, string target_audience3, Action<string> callback)
    {
        var payload = new
        {
            target_audience = target_audience3
        };
    
        string jsonPayload = JsonConvert.SerializeObject(payload);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
    
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(targetUrl, "");
        webRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            callback?.Invoke(webRequest.downloadHandler.text);
            Debug.Log("Id_Token 3 is successfully retrieved");
        }
    }

    private IEnumerator GetIdToken4(string targetUrl, string target_audience4, Action<string> callback)
    {
        var payload = new
        {
            target_audience = target_audience4
        };
    
        string jsonPayload = JsonConvert.SerializeObject(payload);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
    
        UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(targetUrl, "");
        webRequest.uploadHandler = new UploadHandlerRaw(payloadBytes);
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            callback?.Invoke(webRequest.downloadHandler.text);
            Debug.Log("Id_Token 4 is successfully retrieved");
        }
    }

    private IEnumerator MakeRequest(string idToken) // Background or Q1
    {
        // Create request data
        var requestData = new
        {
            temperature = 0.2,
            prompt = "Pretending you're a village chief. One day, a dragon came to your village and caught some villagers to his cave. You need to find a hero to save those villagers. To find a suitable hero, you decide to ask the hero a math question to test their reliability in defeating the dragon. Provide math question with 3 answer to select with the correct answer.Make sure the second answer always be right!"
        };
    
        string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        ///Debug.Log(requestBody);
    
        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl, requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken)); // Encode the idToken
        request.SetRequestHeader("Content-Type", "application/json");
    
        yield return request.SendWebRequest();
    
        // Handle response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            responseText = responseText.Replace("\\n\\n", Environment.NewLine); // Replace "\\n\\n" with actual line breaks
            BackgroundMesh.text = responseText;
            ///Debug.Log("Response: " + responseText);
        }
    }

    // For spawn question 2 & 4
    private IEnumerator FirestoreRequest_Q2(string idToken_4) 
    {
        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_Firestore_Question, "");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_4)); // Encode the idToken_4
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Handle response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            if (currentSceneName == "1-1") {
                string responseJson = request.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
                Q2Mesh.text = responseData.response_text;
                ///Debug.Log("Response: " + responseData.response_text);
            } else if (currentSceneName == "1-2") {
                string responseText = request.downloadHandler.text;
                Q4Mesh.text = responseText;
                ///Debug.Log("Response: " + responseText);
            }
        }
    }

    // For spawn question 3 & 5
    private IEnumerator FirestoreRequest_Q3(string idToken_4)
    {
        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_Firestore_Question, "");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_4)); // Encode the idToken_4
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Handle response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            if (currentSceneName == "1-1") {
                string responseJson = request.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
                Q3Mesh.text = responseData.response_text;
                ///Debug.Log("Response: " + responseData.response_text);
            } else if (currentSceneName == "1-2") {
                string responseText = request.downloadHandler.text;
                Q5Mesh.text = responseText;
                ///Debug.Log("Response: " + responseText);
            }
        }
    }

    // For spawn question 6
    private IEnumerator FirestoreRequest_Q4(string idToken_4)
    {
        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_Firestore_Question, "");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_4)); // Encode the idToken_4
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Handle response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Q6Mesh.text = responseText;
            ///Debug.Log("Response: " + responseText);
        }
    }

    public void OnSubmitButtonClicked()
    {
        StartCoroutine(MakeRequest3(idToken_2));
    }

    private IEnumerator MakeRequest3(string idToken_2) // ChatAI
    {
        // Create request data
        string prompt = promptInputField.text; // Get the user's input from the InputField

        // Create request data
        var requestData = new
        {
            prompt = prompt
        };

        string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        ///Debug.Log(requestBody);

        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_chat, requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_2)); // Encode the idToken_2
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // Handle response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string responseJson = request.downloadHandler.text;
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseJson);
            TalkMesh.text = responseData.response_text;
            NPC_Background.SetActive(true);
            ///Debug.Log("Response: " + responseData.response_text);
        }
    }


    public void TextToSpeechButton_Background()
    {
        string text = BackgroundMesh.text; // Get the desired text from BackgroundMesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public void TextToSpeechButton_Q2()
    {
        string text = Q2Mesh.text; // Get the desired text from Q2Mesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }
    
    public void TextToSpeechButton_Q3()
    {
        string text = Q3Mesh.text; // Get the desired text from Q2Mesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public void TextToSpeechButton_Q4()
    {
        string text = Q4Mesh.text; // Get the desired text from Q2Mesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public void TextToSpeechButton_Q5()
    {
        string text = Q5Mesh.text; // Get the desired text from Q2Mesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public void TextToSpeechButton_Q6()
    {
        string text = Q6Mesh.text; // Get the desired text from Q2Mesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public void TextToSpeechButton_Talk()
    {
        string text = TalkMesh.text; // Get the desired text from TalkMesh
        MakeTextToSpeechRequest(text); // Pass the text as a parameter to the MakeTextToSpeechRequest method
    }

    public DisplayMics displayMics; // Reference to the TextGenerationRequest script
    
    public void Record()
    {
        // 将Record设置为不可见
        if (G_Record != null)
        {
            G_Record.SetActive(false);
        }

        // 将StopRecord设置为可见
        if (G_StopRecord != null)
        {
            G_StopRecord.SetActive(true);
        }
    }

    public void PauseRecord()
    {
        // 将StopRecord设置为不可见
        if (G_StopRecord != null)
        {
            G_StopRecord.SetActive(false);
        }

        // 将Record设置为可见
        if (G_Record != null)
        {
            G_Record.SetActive(false);
        }
        
        // 将Submit设置为可见
        if (G_Submit != null)
        {
            G_Submit.SetActive(true);
        }
    }

    public void SubmitRecord()
    {
        // 将StopRecord设置为可见
        if (G_StopRecord != null)
        {
            G_StopRecord.SetActive(false);
        }

        // 将Record设置为不可见
        if (G_Record != null)
        {
            G_Record.SetActive(true);
        }
        
        // 将Submit设置为不可见
        if (G_Submit != null)
        {
            G_Submit.SetActive(false);
        }
    }

    public void MakeTextToSpeechRequest(string text)
    {
        // Create request data
        var requestData = new
        {
            text = text
        };

        string requestBody = System.Text.Json.JsonSerializer.Serialize(requestData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        ///UnityEngine.Debug.Log(requestBody);

        // Create the UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(functionUrl_TextToSpeech, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_3)); // Encode the idToken_3
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(requestBody));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        StartCoroutine(SendRequest(request));
    }

    IEnumerator SendRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Handle the response
            string responseText = request.downloadHandler.text;
            var responseJson = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseText);
            string audioUrl = responseJson["url"];

            // Play the MP3 file
            StartCoroutine(PlayAudio(audioUrl));
        }
        else
        {
            Debug.LogError("请求失败，错误信息：" + request.error);
        }
    }

    IEnumerator PlayAudio(string audioUrl)
    {
        using (WWW audioLoader = new WWW(audioUrl))
        {
            yield return audioLoader;

            if (string.IsNullOrEmpty(audioLoader.error))
            {
                AudioClip audioClip = audioLoader.GetAudioClip(false, false);

                // Play the audio clip
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("加载音频失败：" + audioLoader.error);
            }
        }
    }
}

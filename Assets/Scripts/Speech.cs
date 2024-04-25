using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using Newtonsoft.Json; // For JsonConvert

[System.Serializable]
public class CloudFunctionResponse
{
    public string[] text;
}
public class Speech : MonoBehaviour
{
    private string idToken_5; // SpeechToText 
    private const string functionUrl_SpeechToText = "?";
    public string blobUrl;
    public TextMeshPro SpeechToText;
    public GameManager gameManager;
    
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void ProcessBlobData()
    {
        StartCoroutine(ReadBlobData(blobUrl));
    }

    private IEnumerator ReadBlobData(string blobUrl)
    {
        // 创建一个WWW对象，将Blob数据加载到内存中
        WWW www = new WWW(blobUrl);

        // 等待WWW对象加载完成
        yield return www;

        // 检查是否有错误发生
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Error loading Blob data: " + www.error);
            yield break;
        }

        // 获取加载的音频数据
        byte[] audioData = www.bytes;

        Debug.Log(audioData);

        // 将音频数据转换为Base64编码的字符串
        string base64Audio = Convert.ToBase64String(audioData);

        StartCoroutine(GetIdToken5(base64Audio));
    }

    private IEnumerator GetIdToken5(string base64Audio)
    {
        string targetUrl = "?";
        string target_audience5 = "?";

        var payload = new
        {
            target_audience = target_audience5
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
            string idToken_5 = webRequest.downloadHandler.text;
            StartCoroutine(SpeechToText_F(idToken_5, base64Audio));
        }
    }

    private IEnumerator SpeechToText_F(string idToken_5, string base64Audio)
    {
        // Create request data
        var requestData = new
        {
            audio = base64Audio
        };

        string requestBody = JsonConvert.SerializeObject(requestData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);

        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_SpeechToText, requestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_5));
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
            // 解析响应文本为CloudFunctionResponse对象
            CloudFunctionResponse response = JsonConvert.DeserializeObject<CloudFunctionResponse>(responseText);

            // 访问解析后的文本结果
            string[] textResults = response.text;

            // 使用StringBuilder构建整合后的文字
            StringBuilder combinedText = new StringBuilder();

            // 处理文本结果
            foreach (string result in textResults)
            {
                combinedText.Append(result).Append(" "); // 添加解析后的文本结果到StringBuilder中，以空格分隔
            }

            string finalText = combinedText.ToString().Trim(); // 获取最终整合后的文字，并去除首尾空格

            Debug.Log("Combined Text: " + finalText);
            if(finalText == "绿水青山就是金山银山")
            {
                
                SpeechToText.text = "你所录制的文字是：" + finalText + ", 与题目一样, Score + 50";
                gameManager.AddScore();
            }
            else
            {
                SpeechToText.text = "你所录制的文字是：" + finalText + ", 与题目不一, 分数保持不变";
            }
        }
    }

}

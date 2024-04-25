using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json; // For JsonConvert

public class ImageRandomizer : MonoBehaviour
{
    public GameObject[] images;
    public Vector2[] positions;
    public GameObject[] gameObjects;
    public Sprite[] sprites;
    public TextMeshPro Picture;
    private string idToken_6; // Picture_VisionCaption
    private const string functionUrl_Picture_VisionCaption = "?";
    public string VisionCpation_Ans;
    public string picture1;
    public string picture2;
    public string picture3;
    public string Ans_VisionCaption;
    public bool isPicture1Dog;
    public bool isPicture2Dog;
    public bool isPicture3Dog;
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
        
        ShufflePositions();
        //ShuffleSprites();
        PlaceImages();
        PictureToBytes();
    }

    public void ActivePicture()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }

    void ShufflePositions()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, positions.Length);
            Vector2 tempPosition = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = tempPosition;
        }
    }

    void ShuffleSprites()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, sprites.Length);
            Sprite tempSprite = sprites[i];
            sprites[i] = sprites[randomIndex];
            sprites[randomIndex] = tempSprite;
        }
    }

    void PlaceImages()
    {
        if (images.Length != positions.Length || images.Length != sprites.Length)
        {
            Debug.LogError("Number of images, positions, and sprites do not match!");
            return;
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].transform.position = new Vector3(positions[i].x, positions[i].y, 0f);
            SpriteRenderer spriteRenderer = images[i].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[i];
        }
    }


    public void PictureToBytes()
    {
        List<byte[]> pictureBytesList = new List<byte[]>();

        foreach (GameObject obj in images)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                Sprite sprite = spriteRenderer.sprite;

                // Convert Sprite to Texture2D
                Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
                texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height));
                texture.Apply();

                // Convert Texture2D to byte array
                byte[] bytes = texture.EncodeToPNG();
                
                // Add the byte array to the list
                pictureBytesList.Add(bytes);
            }
        }

        for (int i = 0; i < pictureBytesList.Count; i++) {
            byte[] pictureBytes = pictureBytesList[i];
            if (i == 0) {
                picture1 = Convert.ToBase64String(pictureBytes);
                Debug.Log(picture1);
            } else if (i == 1){
                picture2 = Convert.ToBase64String(pictureBytes);
                Debug.Log(picture2);
            } else {
                picture3 = Convert.ToBase64String(pictureBytes);
                Debug.Log(picture3);
            };
        }

        StartCoroutine(GetIdToken6()); // 启动 GetIdToken6 协程
    }

    private IEnumerator GetIdToken6()
    {
        string targetUrl = "?";
        string target_audience6 = "?";

        var payload = new
        {
            target_audience = target_audience6
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
            string idToken_6 = webRequest.downloadHandler.text;
            Debug.Log("Id_Token 6 is successfully retrieved");
            StartCoroutine(VisionCpation(idToken_6));
        }
    }

    private IEnumerator VisionCpation(string idToken_6)
    {
        // Create request data
        var requestData = new
        {
            picture1 = picture1,
            picture2 = picture2,
            picture3 = picture3
        };

        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestData));

        // Create request
        UnityWebRequest request = UnityWebRequest.PostWwwForm(functionUrl_Picture_VisionCaption, "");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + Uri.EscapeDataString(idToken_6));
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
            VisionCpation_Ans = responseJson;
            Debug.Log(VisionCpation_Ans);

            // Deserialize the response into a dictionary
            var responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJson);

            // Call the new method to check pictures for dogs
            CheckPicturesForDogs(responseDict);
        }
    }

    private void CheckPicturesForDogs(Dictionary<string, string> responseDict)
    {
        // Check if Picture1 contains "dog"
        bool picture1ContainsDog = responseDict["picture1"].Contains("dog");
        if (picture1ContainsDog)
        {
            Debug.Log("Picture 1 Yes");
        }
        else
        {
            Debug.Log("Picture 1 No");
        }

        // Set a boolean value based on Picture1 result
        isPicture1Dog = picture1ContainsDog;

        // Check if Picture2 contains "dog"
        bool picture2ContainsDog = responseDict["picture2"].Contains("dog");
        if (picture2ContainsDog)
        {
            Debug.Log("Picture 2 Yes");
        }
        else
        {
            Debug.Log("Picture 2 No");
        }

        // Set a boolean value based on Picture2 result
        isPicture2Dog = picture2ContainsDog;

        // Check if Picture3 contains "dog"
        bool picture3ContainsDog = responseDict["picture3"].Contains("dog");
        if (picture3ContainsDog)
        {
            Debug.Log("Picture 3 Yes");
        }
        else
        {
            Debug.Log("Picture 3 No");
        }

        // Set a boolean value based on Picture3 result
        isPicture3Dog = picture3ContainsDog;

        // Use the boolean values as needed
        if (isPicture1Dog)
        {
            // Do something when Picture1 contains "dog"
            Ans_VisionCaption = "A";            
        }
        else
        {
            // Do something when Picture1 does not contain "dog"
            Debug.Log("Wrong");
        }

        if (isPicture2Dog)
        {
            // Do something when Picture2 contains "dog"
            Ans_VisionCaption = "B";
        }
        else
        {
            // Do something when Picture2 does not contain "dog"
            Debug.Log("Wrong");
        }

        if (isPicture3Dog)
        {
            // Do something when Picture3 contains "dog"
            Ans_VisionCaption = "C";
        }
        else
        {
            // Do something when Picture3 does not contain "dog"
            Debug.Log("Wrong");
        }
    }

    public void Vision_A()
    {
        string Ans = "A";
        if (Ans_VisionCaption == Ans) {
            Picture.text = "Right Answer: Score +50";
            gameManager.AddScore();
        } else {
            Picture.text = "Wrong Answer";
        }
    }

    public void Vision_B()
    {
        string Ans = "B";
        if (Ans_VisionCaption == Ans) {
            Picture.text = "Right Answer: Score +50";
            gameManager.AddScore();
        } else {
            Picture.text = "Wrong Answer";
        }
    }

    public void Vision_C()
    {
        string Ans = "C";
        if (Ans_VisionCaption == Ans) {
            Picture.text = "Right Answer: Score +50";
            gameManager.AddScore();
        } else {
            Picture.text = "Wrong Answer";
        }
    } 
}
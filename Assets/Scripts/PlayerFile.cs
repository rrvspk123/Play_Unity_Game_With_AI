using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Proyecto26;
using SimpleJSON;

public class PlayerFile : MonoBehaviour
{
    public static string player_username;
    public static int player_score;

    public GameObject signInObject;
    public GameObject scoreBoardObject;

    public TextMeshProUGUI outputText;

    public static bool signIn;
    public GameObject[] Verified;

    //Below part is the Normal Login System

    // Start is called before the first frame update
    private static PlayerFile instance;
    void Start()
    {
        signInObject.SetActive(true);
        scoreBoardObject.SetActive(false);
        signIn = false;

        for (int i = 0; i < Verified.Length; i++)
        {
            Verified[i].SetActive(false);
        }

        GameObject outputText = GameObject.Find("Score");

        if (FindObjectsOfType<PlayerFile>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }


    }

    public void OnSubmit()
    {
        player_username = FirebaseAuthHandler.GoogleEmail;
        PlayerPrefs.SetString("Username", player_username);
        CheckUsername();
    }

    private void CheckUsername()
    {
        string url = "?";
        StartCoroutine(GetRequest(url));
    }

    private IEnumerator GetRequest(string url)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("CheckLoginCredentials request failed: " + request.error);
        }
        else
        {
            Debug.Log("CheckLoginCredentials request successful");

            // Retrieve the response JSON
            string responseJson = request.downloadHandler.text;
            ///Debug.Log("Response JSON: " + responseJson);

            // Parse the response JSON using SimpleJSON
            JSONNode jsonData = JSON.Parse(responseJson);

            // Check if the entered username and password match any user in the database
            bool loginSuccessful = false;
            
            string check_username = player_username.Replace(".com", "");

            foreach (KeyValuePair<string, JSONNode> entry in jsonData)
            {
                string usernameFromDatabase = entry.Value["username"];

                if (usernameFromDatabase == check_username)
                {
                    loginSuccessful = true;
                    break;
                }
            }

            if (loginSuccessful)
            {
                Debug.Log("Login successful!");
                PlayerPrefs.SetString("Username", player_username);
                signIn = true;
                GameManager.Instance.NewGame();
            }
            else
            {
                PostOnDatabase();
            }
        }
    }

    private void PostOnDatabase()
    {
        User user = new User();
        user.username = player_username.Replace(".com", "");
        user.score = player_score;

        string json = JsonUtility.ToJson(user);
        string url = "?" + user.username + ".json";
        ///Debug.Log(url);
        StartCoroutine(PostRequest(url, json));
    }

    public IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityEngine.Networking.UnityWebRequest(url, "PUT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Post request failed: " + request.error);
        }
        else
        {
            Debug.Log("Post request successful");
            signIn = true;
            GameManager.Instance.NewGame();
        }
    }
    

    public void GetTopScores()
    {
        string url = "?";
        StartCoroutine(GetTopScoresRequest(url));
    }

    private IEnumerator GetTopScoresRequest(string url)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("GetTopScores request failed: " + request.error);
        }
        else
        {
            Debug.Log("GetTopScores request successful");

            // Retrieve the response JSON
            string responseJson = request.downloadHandler.text;
            ///Debug.Log("Response JSON: " + responseJson);

            // Parse the response JSON using SimpleJSON
            JSONNode jsonData = JSON.Parse(responseJson);

            // Extract the username and score from each entry in the JSON data
            List<string> usernames = new List<string>();
            List<int> scores = new List<int>();

            foreach (KeyValuePair<string, JSONNode> entry in jsonData)
            {
                string username = entry.Value["username"];
                int score = entry.Value["score"];

                usernames.Add(username);
                scores.Add(score);
            }

            // Sort the scores in descending order
            List<KeyValuePair<string, int>> sortedScores = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < usernames.Count; i++)
            {
                sortedScores.Add(new KeyValuePair<string, int>(usernames[i], scores[i]));
            }
            sortedScores.Sort((a, b) => b.Value.CompareTo(a.Value));

            // Get the top 10 scores
            int numScoresToDisplay = Mathf.Min(sortedScores.Count, 10);
            string outputText = "";
            for (int i = 0; i < numScoresToDisplay; i++)
            {
                outputText += "Username: " + sortedScores[i].Key + ", Score: " + sortedScores[i].Value + "\n";
            }

            // Set the formatted string as the output text
            this.outputText.text = outputText;
        }
    }

    public static void UpdateScore()
    {
        string savedUsername = player_username.Replace(".com", "");
        string url = "?";
        string url2 = "?";

        PlayerFile playerFile = FindObjectOfType<PlayerFile>();
        playerFile.StartCoroutine(playerFile.GetUserAndUpdateScore(url, savedUsername, url2));
    }

    private IEnumerator GetUserAndUpdateScore(string url, string username, string url2)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("GetUserAndUpdateScore request failed: " + request.error);
        }
        else
        {
            Debug.Log("GetUserAndUpdateScore request successful");

            // Retrieve the response JSON
            string responseJson = request.downloadHandler.text;
            ///Debug.Log("Response JSON: " + responseJson);

            // Parse the response JSON using SimpleJSON
            JSONNode jsonData = JSON.Parse(responseJson);

            // Find the user with the matching username
            foreach (KeyValuePair<string, JSONNode> entry in jsonData)
            {
                string usernameFromDatabase = entry.Value["username"];

                if (usernameFromDatabase == username)
                {
                    // Update the score for the user
                    JSONNode userNode = entry.Value;
                    ///Debug.Log(entry.Key);
                    userNode["score"] = GameManager.Instance.GetScore(); // Assuming GameManager is accessible
                    ///Debug.Log("Updated Score: " + userNode["score"]);
                    string updatedJson = userNode.ToString();

                    // Send a PUT request to update the score in the database
                    yield return StartCoroutine(PutRequest(url2 + "/" + entry.Key + ".json", updatedJson, url2));

                    break;
                }
            }
        }
    }

    private IEnumerator PutRequest(string url, string json, string url2)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Put request failed: " + request.error);
        }
        else
        {
            Debug.Log("Put request successful");
        }
    }

    public void Login()
    {
        string url = "?";
        StartCoroutine(CheckLoginCredentials(url));
    }

    public IEnumerator CheckLoginCredentials(string url)
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("CheckLoginCredentials request failed: " + request.error);
        }
        else
        {
            Debug.Log("CheckLoginCredentials request successful");

            // Retrieve the response JSON
            string responseJson = request.downloadHandler.text;
            ///Debug.Log("Response JSON: " + responseJson);

            // Parse the response JSON using SimpleJSON
            JSONNode jsonData = JSON.Parse(responseJson);

            // Check if the entered username and password match any user in the database
            bool loginSuccessful = false;

            foreach (KeyValuePair<string, JSONNode> entry in jsonData)
            {
                string usernameFromDatabase = entry.Value["username"];

                if (usernameFromDatabase == player_username)
                {
                    loginSuccessful = true;
                    break;
                }
            }

            if (loginSuccessful)
            {
                Debug.Log("Login successful!");
                PlayerPrefs.SetString("Username", player_username);
                signIn = true;
                GameManager.Instance.NewGame();
            }
            else
            {
                Debug.Log("Invalid username or password");
                signIn = false;
            }
        }
    }
    public void SignInUsername()
    {
        signInObject.SetActive(false);
        for (int i = 0; i < Verified.Length; i++)
        {
            Verified[i].SetActive(true);
        }
    }

    public void ScoreBoard()
    {
        signInObject.SetActive(false);
        scoreBoardObject.SetActive(true);
    }

    public void BackToSignIn()
    {
        signInObject.SetActive(true);
        scoreBoardObject.SetActive(false);
    }

    public void VerifiedMenu()
    {
        signInObject.SetActive(false);
        scoreBoardObject.SetActive(false);
        for (int i = 0; i < Verified.Length; i++)
        {
            Verified[i].SetActive(true);
        }
    }
}

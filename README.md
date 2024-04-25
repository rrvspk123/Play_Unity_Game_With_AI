# Play_Unity_Game_With_AI

This is a game that create with Unity Engine and using the VertexAI from Google Cloud Platform(GCP) to completed. Also, we're using the firebase database to store some information and authentication.

Also, the game is required "Microphone" to pass some question such as SpeechToText. But it is not necessary at all since the question is only for adding marks.

> This project is working with my teammate [@EvansAu](https://github.com/EvansAu) [@LoHauYin](https://github.com/LoHauYin) [@wongshunki](https://github.com/wongshunki)

> [!WARNING]
> This is a project done within the IVE studying process, only targeting leanring cloud computing and other programming skills. Please notice this is a learning project, may contain different lack of skill problem.

# BookMarks

- [Play\_Unity\_Game\_With\_AI](#play_unity_game_with_ai)
- [BookMarks](#bookmarks)
- [Remark for places need to add/change](#remark-for-places-need-to-addchange)
  - [Basic Setup](#basic-setup)
    - [GoogleAuthenicator.cs](#googleauthenicatorcs)
    - [HttpCodeListener.cs](#httpcodelistenercs)
  - [AI function that required GCP function](#ai-function-that-required-gcp-function)
    - [ImageRandomizer.cs](#imagerandomizercs)
    - [FirebaseAuthHandler.cs](#firebaseauthhandlercs)
    - [PlayerFile.cs](#playerfilecs)
    - [Speech.cs](#speechcs)
    - [TextGenerationRequest.cs](#textgenerationrequestcs)
- [For WebGL Build in Unity (Use of Microphone)](#for-webgl-build-in-unity-use-of-microphone)
- [Reference](#reference)

# Remark for places need to add/change

> [!IMPORTANT]
> Due to our lack of programming/cloud computing skills, all the AI function will required the GCP function to do it. Please pay attention to this

## Basic Setup

### GoogleAuthenicator.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change the ClientID/ClientSecret/RedirectUri getting from the Google Cloud Platform's OAuth 2.0 Client ID. (Don't care about the non-UnityWebGL part unless you are local testing)
</details>

### HttpCodeListener.cs

<details>
<summary>Details</summary>
For this .cs file, you would only need to change the RedirectUri just like the last .cs file.
</details>

## AI function that required GCP function

> [!NOTE]
> Those google cloud function will show as sample in the [FunctionSamples](./FunctionSample.md) file

> [!IMPORTANT]
> Since those cloud function required service account's identity-token to process, so it required another cloud function to create the token for specific cloud function (every cloud function need one) / make all function public

> [!TIP]
> You're not required to do the above important note if you have the ability to get the identity-token from code. We doing this is because we lack of skills :D

### ImageRandomizer.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change the "functionUrl_Picture_VisionCaption"/"targetUrl"/"target_audience6".

> "functionUrl_Picture_VisionCaption" is function that do Vision Caption to some picture

> "targetUrl" is function that create identity-token for each cloud function

> "target_audience6" is function that need to has the identity-token to process, basically = "functionUrl_Picture_VisionCaption"

</details>

### FirebaseAuthHandler.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change the "ApiKey" only.

> "ApiKey" is key that created in the Firebase which can find in the project setting's "Web API Key" section

</details>

### PlayerFile.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change the "url" in CheckUsername() & GetTopScores() & UpdateScore() & Login() sections / "url" in PostOnDatabase() section / "url2" in UpdateScore() section.

> "url" in CheckUsername() & GetTopScores() & UpdateScore() & Login() sections will required you to create a realtime-database and get the link

> "url" in PostOnDatabase() will required you to change the link in specific format like "https://xxx/users" + user.username + ".json"

> "url2" in UpdateScore() will required you to change the link in specific format like "https://xxx/users"

</details>

### Speech.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change the "functionUrl_SpeechToText"/"targetUrl"/"target_audience5".

> "functionUrl_SpeechToText" is function that do SpeechToText to audio bytes

> "targetUrl" is function that create identity-token for each cloud function

> "target_audience5" is function that need to has the identity-token to process, basically = "functionUrl_SpeechToText"

</details>

### TextGenerationRequest.cs

<details>
<summary>Details</summary>
For this .cs file, you would need to change "targetUrl"/"target_audience"/"target_audience_2"/"target_audience_3"/"target_audience_4"/"functionUrl"/"functionUrl_chat"/"functionUrl_TextToSpeech"/"functionUrl_Firestore_Question".

> "functionUrl"/"functionUrl_chat"/"functionUrl_TextToSpeech"/"functionUrl_Firestore_Question" is functions that do different function base on their name to text generation or TextToSpeech

> "functionUrl_Firestore_Question" will required you to create a firestore database to store different question such as Chinese/English/Maths category

> "targetUrl" is function that create identity-token for each cloud function

> "target_audience"/"target_audience_2"/"target_audience_3"/"target_audience_4" is functions that need to has the identity-token to process, basically = "functionUrl_SpeechToText"

</details>

# For WebGL Build in Unity (Use of Microphone)

> [!TIP]
> The Unity WebGL is not supported the use of Microphone but the website does, so we use some JavaScript to do it. [JavaScript](./ForWebGL)

After you do the "WebGL Build", please put the "recorder.wav.min.js" file into the same level folder.

And you need to add the "AddToIndex.js" file's content into the index.html file you created from WebGL Build. You can take a look at the Sampleindex.html file to get some idea on how to do it.

# Reference

During the working process, we have refer to different project / video to find ways to complete our project.

> [GameDesign](https://github.com/zigurous/unity-super-mario-tutorial)

> [VertexAI](https://cloud.google.com/vertex-ai/docs/start/introduction-unified-platform?hl=en)

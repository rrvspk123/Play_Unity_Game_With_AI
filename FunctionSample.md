> [!IMPORTANT]
> These function is not 100% accurate to your application. Please don't copy it until you confirm.

# Functions

## Identity-token function

<details>
<summary>Scripts</summary>

```
from flask import Flask, request, jsonify
from flask_cors import CORS
from functions_framework import http
import os
import google.oauth2.id_token
import google.auth.transport.requests
from google.cloud import storage

def token(request):
    # Set CORS headers for the preflight request
    if request.method == "OPTIONS":
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "Content-Type",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {"Access-Control-Allow-Origin": "*"}

    # Retrieve the credentials file from the Cloud Storage bucket
    bucket_name = "?"
    credentials_file_path = "?"

    storage_client = storage.Client()
    bucket = storage_client.get_bucket(bucket_name)
    blob = bucket.blob(credentials_file_path)
    blob.download_to_filename("/tmp/credentials.json")

    # Set the environment variable for the retrieved credentials file
    os.environ["GOOGLE_APPLICATION_CREDENTIALS"] = "/tmp/credentials.json"

    # Fetch the ID token from the request data
    request_data = request.get_json()
    target_audience = request_data.get("target_audience")
    if target_audience:
        request_obj = google.auth.transport.requests.Request()
        id_token = google.oauth2.id_token.fetch_id_token(request_obj, target_audience)
        print(id_token)

        # Create a response object with the ID token
        response_data = {
            "id_token": id_token
        }
        response = jsonify(response_data)

        return (response, 200, headers)
    else:
        return (jsonify({"error": "Target audience not provided in the request."}), 400, headers)

if __name__ == '__main__':
    app.run()
```

</details>

## Background story based text generation

<details>
<summary>Scripts</summary>

```
from flask import Flask, request, jsonify
from flask_cors import CORS, cross_origin
from functions_framework import http
import os
import json
import vertexai
from vertexai.language_models import TextGenerationModel

# Initialize the Vertex AI client
project_id = "?"
location = "us-central1"
vertexai.init(project=project_id, location=location)

def generate_text(request):
    if request.method == "OPTIONS":
        # Allows GET requests from any origin with the Content-Type
        # header and caches preflight response for an 3600s
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {
        "Access-Control-Allow-Origin": "*",
    }

    request_json = request.json
    temperature = float(request_json["temperature"])
    prompt = request_json["prompt"]

    # Define the model parameters
    parameters = {
        "temperature": temperature,
        "top_p":0.8,
        "top_k":40
    }

    # Load the pre-trained TextGenerationModel
    model = TextGenerationModel.from_pretrained("text-bison@001")

    # Generate text using the model
    response = model.predict(prompt + " Remember to split the sentence for each line.", **parameters)
    response_text = jsonify(response.text)

    return (response_text, 200, headers)
```

</details>

## Firestore Question rewrite

<details>
<summary>Scripts</summary>

```
import random
import json
from google.cloud import firestore
import vertexai
from vertexai.language_models import TextGenerationModel

# Initialize the Vertex AI client
project_id = "?"
location = "us-central1"
vertexai.init(project=project_id, location=location)

def retrieve_random_question(request):
    # Set CORS headers for the preflight request
    if request.method == "OPTIONS":
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("",204, headers)

    # Create a Firestore client
    db = firestore.Client()

    # Reference the "Questions" collection
    collection_ref = db.collection('Questions')

    # Retrieve all documents from the collection
    all_documents = collection_ref.get()

    # Select a random document
    random_document = random.choice(all_documents)

    # Retrieve the data from the random document
    document_data = random_document.to_dict()

    # Get the question from the document
    question = document_data["All"]

    return generate_text(question)

def generate_text(prompt):
    # Set CORS headers for the main request
    headers = {"Access-Control-Allow-Origin": "*"}

    # Define the model parameters
    parameters = {
        "temperature": 0.2,
        "top_p": 0.8,
        "top_k": 40
    }

    # Load the pre-trained TextGenerationModel
    model = TextGenerationModel.from_pretrained("text-bison@001")

    # Generate text using the model
    response = model.predict(f"rewrite this sentence with Mario game play style: {prompt} Remember to split the sentence for each line.", **parameters)
    response_text = response.text

    return (json.dumps({"response_text": response_text}),200, headers)
```

</details>

## Chat AI

<details>
<summary>Scripts</summary>

```
import json
import vertexai
from vertexai.language_models import ChatModel, InputOutputTextPair

# Initialize Vertex AI
project_id = "?"
location = "us-central1"
vertexai.init(project=project_id, location=location)

# Define the chat handler function
def chat_handler(request):
    # Set CORS headers for the preflight request
    if request.method == "OPTIONS":
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {"Access-Control-Allow-Origin": "*"}

    # Create an instance of the ChatModel using the pre-trained "chat-bison" model
    chat_model = ChatModel.from_pretrained("chat-bison")

    # Set the parameters for the chat conversation
    parameters = {
        "candidate_count": 1,
        "max_output_tokens": 1024,
        "temperature":0.2,
        "top_p":0.8,
        "top_k":40
    }

    # Start the chat conversation with an initial context
    chat = chat_model.start_chat(
        context="""Pretending you're a village chief. One day, a dragon came to your village and caught some villagers to his cave. You need to find a hero to save those villagers. To find a suitable hero, you decide to ask the hero a math question to test their reliability in defeating the dragon. You will now talk to the hero who came to the village. The hero will need to fight across the level to reach the final room which is the dragon and the villagers are. (The user is hero)""",
    )

    # Check if the request method is POST
    if request.method == 'POST':
        # Get the prompt from the request JSON
        prompt = request.get_json()["prompt"]

        # Send the prompt to the chat model and get the response
        response = chat.send_message(f"{prompt} Remember to split the sentence for each line.", **parameters)

        response_text = response.text

        # Return the response as JSON
        return (json.dumps({"response_text": response_text}),200, headers)
    else:
        # Return an error message for invalid request method
        return 'Invalid request method. Only POST is allowed.'

# This is the entry point for the Cloud Function
def main(request):
    return chat_handler(request)
```

</details>

## SpeechToText

<details>
<summary>Scripts</summary>

```
import base64
import json
from google.cloud import speech, storage
from flask import request

def audio_to_text(request):
    if request.method == "OPTIONS":
        # Allows GET requests from any origin with the Content-Type
        # header and caches preflight response for an 3600s
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {
        "Access-Control-Allow-Origin": "*",
    }

    request_data = json.loads(request.data)
    # Get the base64-encoded audio data from the request
    audio_data = request_data['audio']

    # Decode the base64-encoded audio data
    audio_bytes = base64.b64decode(audio_data)

    # Initialize Cloud Speech-to-Text client
    client = speech.SpeechClient()

    # Configure audio input
    audio = speech.RecognitionAudio(content=audio_bytes)
    config = speech.RecognitionConfig(
        encoding=speech.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=16000,
        language_code='zh-Hans-CN'
    )

    # Perform speech recognition
    response = client.recognize(config=config, audio=audio)

    # Extract recognition results
    results = []
    for result in response.results:
        results.append(result.alternatives[0].transcript)

    # Return recognition results
    return ({"text": results}, 200, headers)
```

</details>

## TextToSpeech

<details>
<summary>Scripts</summary>

```
import os
import base64
import uuid
from google.cloud import texttospeech
from google.cloud import storage

def text_to_speech(request):
    if request.method == "OPTIONS":
        # Allows GET requests from any origin with the Content-Type
        # header and caches preflight response for an 3600s
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {
        "Access-Control-Allow-Origin": "*",
    }

    text = request.get_json().get('text')

    client = texttospeech.TextToSpeechClient()

    synthesis_input = texttospeech.SynthesisInput(text=text)

    voice = texttospeech.VoiceSelectionParams(
        language_code='en-US',
        name='en-US-Neural2-H',
        ssml_gender=texttospeech.SsmlVoiceGender.FEMALE
    )
    audio_config = texttospeech.AudioConfig(
        audio_encoding=texttospeech.AudioEncoding.MP3
    )

    response = client.synthesize_speech(
        input=synthesis_input,
        voice=voice,
        audio_config=audio_config
    )

    # Encode audio content as Base64
    audio_base64 = base64.b64encode(response.audio_content).decode('utf-8')

    # Decode Base64 audio data
    audio_bytes = base64.b64decode(audio_base64)

    # Upload audio data to Cloud Storage
    storage_client = storage.Client()
    # Get the BUCKET_NAME environment variable
    bucket_name = "?"
    bucket = storage_client.bucket(bucket_name)
    blob = bucket.blob(f"audio/{uuid.uuid4()}.mp3")
    blob.upload_from_string(audio_bytes)

    # Return the URL of the audio file
    return {"url": blob.public_url}, 200, headers
```

</details>

## Vision Caption

<details>
<summary>Scripts</summary>

```
import base64
import io
from io import BytesIO
from google.cloud import storage
import json
from flask import Flask, request, Request

import vertexai
from vertexai.vision_models import ImageTextModel, Image

def vision_caption(request):
    if request.method == "OPTIONS":
        # Allows GET requests from any origin with the Content-Type
        # header and caches preflight response for an 3600s
        headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "*",
            "Access-Control-Allow-Headers": "*",
            "Access-Control-Max-Age": "3600",
        }

        return ("", 204, headers)

    # Set CORS headers for the main request
    headers = {
        "Access-Control-Allow-Origin": "*",
    }

    # Extract the base64-encoded images from the request data.
    # Convert the request data to a JSON object first
    request_data = json.loads(request.data)
    picture1 = request_data['picture1']
    picture2 = request_data['picture2']
    picture3 = request_data['picture3']

    # Convert the strings to bytes before decoding them.
    picture1 = picture1.encode('utf-8')
    picture2 = picture2.encode('utf-8')
    picture3 = picture3.encode('utf-8')

    # Decode the base64-encoded images.
    image_bytes1 = base64.b64decode(picture1)
    image_bytes2 = base64.b64decode(picture2)
    image_bytes3 = base64.b64decode(picture3)

    # Create an in-memory file object for each image.
    image1_file = BytesIO(image_bytes1)
    image2_file = BytesIO(image_bytes2)
    image3_file = BytesIO(image_bytes3)

    # Create a Cloud Storage client.
    storage_client = storage.Client()

    # Get the Cloud Storage bucket to which the images will be uploaded.
    bucket = storage_client.bucket("?")

    # Upload the images to the bucket.
    blob1 = bucket.blob("image1.jpg")
    blob1.upload_from_file(image1_file)

    blob2 = bucket.blob("image2.jpg")
    blob2.upload_from_file(image2_file)

    blob3 = bucket.blob("image3.jpg")
    blob3.upload_from_file(image3_file)

    # Print a success message.
    print("Images saved successfully to Cloud Storage bucket.")

    # Download the blob files to the /tmp directory.
    blob1.download_to_filename("/tmp/image1.jpg")
    blob2.download_to_filename("/tmp/image2.jpg")
    blob3.download_to_filename("/tmp/image3.jpg")

    # Load the pre-trained Image Text model.
    image_text_model = ImageTextModel.from_pretrained("imagetext@001")

    # Get the URIs of the downloaded files.
    uri1 = f"/tmp/image1.jpg"
    uri2 = f"/tmp/image2.jpg"
    uri3 = f"/tmp/image3.jpg"

    # Load the images from the URIs.
    image1 = Image.load_from_file(uri1)
    image2 = Image.load_from_file(uri2)
    image3 = Image.load_from_file(uri3)

    # Perform text detection on each image.
    captions1 = image_text_model.get_captions(image=image1, number_of_results=2, language="en")
    captions2 = image_text_model.get_captions(image=image2, number_of_results=2, language="en")
    captions3 = image_text_model.get_captions(image=image3, number_of_results=2, language="en")

    print(captions1)
    print(captions2)
    print(captions3)

    # Extract the text from each image.
    text1 = captions1[0]
    text2 = captions2[0]
    text3 = captions3[0]

    # Return the text as a JSON response.
    return json.dumps({
        "picture1": text1,
        "picture2": text2,
        "picture3": text3
    }), 200, headers
```

</details>

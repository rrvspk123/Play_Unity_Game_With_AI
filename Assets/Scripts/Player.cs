using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovements normalRenderer;
    
    private DeathAnimation deathAnimation;

    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }
    public TextGenerationRequest textGenerationRequest; // Reference to the TextGenerationRequest script
    public SignalManager signalManager;
    public Speech speech;
    public ImageRandomizer imageRandomizer; // Reference to the ImageRandomizer script

    public void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        textGenerationRequest = FindObjectOfType<TextGenerationRequest>();
        signalManager = FindObjectOfType<SignalManager>();
        speech = FindObjectOfType<Speech>();
        imageRandomizer = FindObjectOfType<ImageRandomizer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ChatAI"))
        {
            textGenerationRequest.TextToSpeechButton_Talk(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q1")) {
            textGenerationRequest.TextToSpeechButton_Background(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q2")) {
            textGenerationRequest.TextToSpeechButton_Q2(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q3")) {
            textGenerationRequest.TextToSpeechButton_Q3(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q4")) {
            textGenerationRequest.TextToSpeechButton_Q4(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q5")) {
            textGenerationRequest.TextToSpeechButton_Q5(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Q6")) {
            textGenerationRequest.TextToSpeechButton_Q6(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Record")) {
            textGenerationRequest.Record();
            signalManager.StartRecorderFunc(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("StopRecord")) {
            textGenerationRequest.PauseRecord();
            signalManager.EndRecorderFunc(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Submit")) {
            textGenerationRequest.SubmitRecord(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Picture")) {
            imageRandomizer.ActivePicture(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Vision_A")) {
            imageRandomizer.Vision_A(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Vision_B")) {
            imageRandomizer.Vision_B(); // Call the method from TextGenerationRequest
        } else if(collision.gameObject.CompareTag("Vision_C")) {
            imageRandomizer.Vision_C(); // Call the method from TextGenerationRequest
        }
    }

    public void Hit()
    {
        if(!dead && !starpower)
        {
            Death();
        }
    }

    private void Death()
    {
        normalRenderer.enabled = false;
        deathAnimation.enabled = true;
        
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        GameManager.Instance.ResetLevel(3f);

    }

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                normalRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); 
            }

            yield return null;
        }

        normalRenderer.spriteRenderer.color = Color.white;

        starpower = false;

    }
}

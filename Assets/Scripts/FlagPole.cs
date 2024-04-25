using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform portal;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    public string End;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovements>().enabled = false;
 
        yield return MoveTo(player, portal.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        if (End == "No"){
            GameManager.Instance.LoadLevel(nextWorld, nextStage);
        } else {
            GameManager.Instance.GameOver();
        }
        

    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}

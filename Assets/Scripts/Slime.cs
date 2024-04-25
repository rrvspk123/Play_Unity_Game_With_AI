using UnityEngine;

public class Slime : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player.starpower)
            {
                Hit();
            } else {
                player.Hit();
            }
        }
    }

    private void Hit()
    {
        Destroy(gameObject, 0f);
    }
}

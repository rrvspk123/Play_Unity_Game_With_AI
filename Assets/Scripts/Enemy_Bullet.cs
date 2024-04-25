using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public float speed = 3f; // 子弹速度

    private GameObject target; // 目标对象

    private void Start()
    {
        FindTarget();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.fixedDeltaTime);
        }
    }

    private void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            target = players[0];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Hit();
            }
        }
        Destroy(gameObject);
    }
}
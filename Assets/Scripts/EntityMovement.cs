
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    public GameObject playerPrefab; // c1 prefab

    private new Rigidbody2D rigidbody;
    public Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        if (playerPrefab != null)
        {
            // 计算生物需要朝向的方向
            direction = (playerPrefab.transform.position - transform.position).normalized;
        }
        

        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction))
        {
            Vector2 reflection = Vector2.Reflect(direction, (Vector2)(rigidbody.position - (Vector2)transform.position));
            direction = reflection.normalized;
        }

        if (rigidbody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}

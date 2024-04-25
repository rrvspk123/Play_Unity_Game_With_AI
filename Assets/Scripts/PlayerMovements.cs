using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private new Camera camera;
    // character body's occupied size
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    
    private Vector2 velocity;

    private float inputAxis;

    // moveSpeed settings
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float downwardSpeed = 4f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;

    public SpriteRenderer spriteRenderer { get; private set; }
    
    public Sprite idle;
    public Sprite jump;
    public AnimatedSprite run;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
        spriteRenderer.enabled = true;
    }

    private void Update()
    {
        HorizontalMovement();
        
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) {
            GroundedMovement();
        }

        ApplyGravity();

        if (Input.GetKey(KeyCode.S))
        {
            velocity.y = -downwardSpeed;
        }

        if (Input.GetKeyDown(KeyCode.J)) {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = transform.right * 6; // Shoot the bullet in the direction the character is facing
        Destroy(bullet, 1f); // Destroy the bullet after the specified lifetime
    }
    }

    private void LateUpdate()
    {
        if (jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (running)
        {   
            run.enabled = running;
        }
        else {
            spriteRenderer.sprite = idle;
        }
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x,inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) 
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // perform jump
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
        
    }

    private void ApplyGravity()
    {
        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump") || Input.GetKey(KeyCode.S);
        float multiplier = falling ? 2f : 1f;

        // apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   

        if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // stop vertical movement if mario bonks his head
            if (transform.DotTest(collision.transform, Vector2.up)) {
                velocity.y = 0f;
            }
        }
    }
}

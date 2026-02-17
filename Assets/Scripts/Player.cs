using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]float moveSpeed = 1.0f;

    bool alive = true;
    Rigidbody2D body;

    public DodgerAttributes attributes;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        attributes = new(3);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.ReadValue() > 0.5 && !alive)
        {   //Resets scene if you died and press space
            SceneManager.LoadScene("SampleScene");
        }

        if (!alive) return;

        //Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
        //Physics2D.gravity = new Vector2(acceleration.x, acceleration.y);

        if (InputManager.IsPressing(out Vector2 screenPos))
        {
            Vector3 movePoint = Camera.main.ScreenToWorldPoint(screenPos);
            movePoint.z = 0;
            if (movePoint.x - transform.position.x > 0.8f) body.linearVelocityX = moveSpeed;
            if (movePoint.x - transform.position.x < -0.8f) body.linearVelocityX = -moveSpeed;
        }
        else
        {
            body.linearVelocityX = 0;
        }

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if(viewportPos.x < 0 || viewportPos.x > 1)
        {
            body.linearVelocityX = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && alive)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Random.Range(-5, 5), 5);
            Destroy(collision.gameObject.GetComponent<Collider2D>());

            if (attributes.TakeDamage(1))
            {
                transform.Rotate(new Vector3(0, 0, 90));
                alive = false;
                GameManager.EndGame();
            }
        }
    }
}

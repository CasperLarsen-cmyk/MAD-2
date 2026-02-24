using UnityEngine;
public abstract class BaseEnemy : MonoBehaviour
{
    protected float gravityScale = 1f;

    private void Start()
    {
        var body = GetComponent<Rigidbody2D>();
        body.gravityScale = gravityScale;
    }

    private void FixedUpdate()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.y < 0)
        {
            GameManager.AddScore(1);
            Destroy(gameObject);
        }
    }
}
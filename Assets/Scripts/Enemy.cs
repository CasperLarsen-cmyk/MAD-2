using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        
        if (viewPos.y < 0 )
        {
            GameManager.AddScore(1);
            Destroy(gameObject);
        }
    }
}

using System;
using UnityEngine;
public class BaseEnemy : MonoBehaviour
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

    public EnemyData GetSaveData()
    {
        var data = new EnemyData();
        data.gravityScale = gravityScale;
        data.position = transform.position;
        data.velocity = GetComponent<Rigidbody2D>().linearVelocity;
        data.color = GetComponent<SpriteRenderer>().color;
        return data;
    }

    public void LoadSaveData(EnemyData data)
    {
        gravityScale = data.gravityScale;
        transform.position = data.position;
        GetComponent<Rigidbody2D>().linearVelocity = data.velocity;
        GetComponent<SpriteRenderer>().color = data.color;
    }
}

[Serializable]
public struct EnemyData
{
    public float gravityScale;

    public Vector3 position;
    public Vector3 velocity;

    public Color color;
}
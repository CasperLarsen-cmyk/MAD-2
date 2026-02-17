using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float enemyDelay = 1;

    public TextMeshProUGUI scoreTracker;
    public static float score = 0;

    private static bool started;
    private static bool ended;
    private Vector3 spawnOrigin;
    private float spawnXAdd;
    private float spawnTimer = 0;

    void Start()
    {
        spawnOrigin = Camera.main.ViewportToWorldPoint(new Vector2(0, 1.2f));
        spawnXAdd = Camera.main.ViewportToWorldPoint(new Vector2(1, 1.2f)).x - spawnOrigin.x;
        spawnOrigin.z = 0;

        ResetStaticVariables();
    }

    void Update()
    {
        if (ended) return;
        if (!started && InputManager.IsPressing(out Vector2 position)) started = true;
        if (started) spawnTimer += Time.deltaTime;
        if (spawnTimer > enemyDelay)
        {
            spawnTimer -= enemyDelay;
            
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = spawnOrigin;
            enemy.transform.Translate(new Vector2(Random.Range(0, spawnXAdd), 0));
            enemy.GetComponent<Rigidbody2D>().AddTorque(60, ForceMode2D.Impulse);
        }

        scoreTracker.text = score.ToString();
    }

    private void ResetStaticVariables()
    {
        score = 0;
        started = false;
        ended = false;
    }

    public static void EndGame()
    {
        ended = true;
    }

    public static float AddScore(float points)
    {
        if (!ended) score += points;
        return score;
    }
}

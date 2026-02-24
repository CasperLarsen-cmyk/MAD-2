using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float enemyDelay = 1;

    public TextMeshProUGUI scoreTracker;

    private static Player player;

    private static bool started;
    private static bool ended;
    private Vector3 spawnOrigin;
    private float spawnXAdd;
    private float spawnTimer = 0;

    void Start()
    {
        spawnOrigin = Camera.main.ViewportToWorldPoint(new Vector2(0, 1.1f));
        spawnXAdd = Camera.main.ViewportToWorldPoint(new Vector2(1, 1.1f)).x - spawnOrigin.x;
        spawnOrigin.z = 0;

        player = FindFirstObjectByType<Player>();

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
            
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab);
            enemy.transform.position = spawnOrigin;
            enemy.transform.Translate(new Vector2(Random.Range(0, spawnXAdd), 0));
            enemy.GetComponent<Rigidbody2D>().AddTorque(60, ForceMode2D.Impulse);
        }

        scoreTracker.text = player.attributes.GetScore().ToString();
    }

    private void ResetStaticVariables()
    {
        started = false;
        ended = false;
    }

    public static void EndGame()
    {
        ended = true;
    }

    public static float AddScore(int points)
    {
        if (!ended) player.attributes.AddScore(points);
        return player.attributes.GetScore();
    }
}

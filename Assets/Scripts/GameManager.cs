using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int[] spawnWeights;
    public float enemyDelay = 1;

    public TextMeshProUGUI scoreTracker;

    private static Player player;

    public static bool started;
    public static bool ended;
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
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if (ended && (Keyboard.current.spaceKey.ReadValue() > 0.5 || InputManager.swiping))
        {   //Resets scene if you died and press space
            SceneManager.LoadScene("SampleScene");
        }

        if (ended) return;

        if (!started && InputManager.IsPressing(out Vector2 position))
        {
            started = true;
            Time.timeScale = 1.0f;
        }
        if (started) spawnTimer += Time.deltaTime;

        while (spawnTimer > enemyDelay)
        {
            spawnTimer -= enemyDelay;
            
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab);
            enemy.transform.position = spawnOrigin;
            enemy.transform.Translate(new Vector2(Random.Range(0, spawnXAdd), 0));
            enemy.GetComponent<Rigidbody2D>().AddTorque(60, ForceMode2D.Impulse);
        }

        var acc = Accelerometer.current;
        //if (acc != null) Physics2D.gravity = acc.acceleration.value;

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
    readonly FileInfo file = new(Application.dataPath + "/savedata.json");

    public void Save()
    {
        SaveBlob blob = SaveBlob.Create();
        string json = JsonUtility.ToJson(blob, true);
        file.Delete();

        using StreamWriter stream = new(file.FullName);
        stream.Write(json);
    }

    static SaveBlob blob;
    
    public void Load()
    {
        SceneManager.LoadScene("SampleScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        StreamReader stream = new(file.FullName);
        string json = stream.ReadToEnd();
        stream.Close();

        blob = JsonUtility.FromJson<SaveBlob>(json);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindFirstObjectByType<Player>().LoadSaveData(blob.dataPlayer);

        BaseEnemy[] enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);
        foreach (BaseEnemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        foreach (EnemyData data in blob.dataEnemies)
        {
            var enemy = Instantiate(enemyPrefabs[0], data.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().LoadSaveData(data);
        }

        //Cleanup
        SceneManager.sceneLoaded -= OnSceneLoaded;
        blob = null;
    }
}

using System.IO;
using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
#endif

public class BigBrother : MonoBehaviour
{
    readonly FileInfo file = new(Application.dataPath + "/Player data.csv");
    float timer;
    float delay = 0.25f;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // Request write permission if needed (older Android)
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif

        if (!file.Exists)
        {
            using StreamWriter stream = new(file.FullName);
            stream.WriteLine("Time,X,Score,Health");
        }
        else
        {
            using StreamWriter stream = new(file.FullName, true);
            stream.WriteLine("0,0,0,0");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (GameManager.started && !GameManager.ended && timer >= delay)
        {
            timer -= delay;
            Save();
        }
    }

    public void Save()
    {
        var player = FindFirstObjectByType<Player>();

        var x = player.transform.position.x;
        var time = Time.time;
        var score = player.attributes.GetScore();
        var hp = player.attributes.GetHP();

        using StreamWriter stream = new(file.FullName, true);
        stream.WriteLine(time + "," + x + "," + score + "," + hp);
    }

    /*public bool IsWatching()
    {
        return true;
    }*/

    /*[Serializable]
    struct Entry
    {   // I reconsidered doing this as i wanted to use CSV instead of JSON for the logging.
        public float x;
        public float time;
        public int score;
        public int hp;

        public Entry(Player player)
        {
            x = player.transform.position.x;
            time = Time.time;
            score = player.attributes.GetScore();
            hp = player.attributes.GetHP();
        }
    }*/
}

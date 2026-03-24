using System;
using UnityEngine;

[Serializable]
public class SaveBlob
{
    public PlayerData dataPlayer;
    public EnemyData[] dataEnemies;
    
    static public SaveBlob Create()
    {
        SaveBlob blob = new();
        blob.dataPlayer = MonoBehaviour.FindFirstObjectByType<Player>().GetSaveData();

        var enemies = MonoBehaviour.FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);
        blob.dataEnemies = new EnemyData[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            blob.dataEnemies[i] = enemies[i].GetSaveData();
        }
        return blob;
    }
}

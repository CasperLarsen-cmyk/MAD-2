using UnityEngine;

public class EnemySlow : BaseEnemy
{
    private void Awake()
    {
        gravityScale = 0.5f;
    }
}
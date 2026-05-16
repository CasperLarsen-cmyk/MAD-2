using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class Gameplay : MonoBehaviour
{
    // Instance of the Player class
    private PlayerClass playerOne;
    private EnemyClass enemyOne;

    public void Update()
    {
        enemyOne.Attack(playerOne);
    }

    // Initialize the Player in Start
    void Start()
    {
        // Initialize the player with some starting values
        playerOne = new("Bob", 100);
        Weapon weapon = new("Excalibur", 15);
        playerOne.weapon = weapon;

        enemyOne = new("Werewolf", 16);

        // Adds 10 points to the score
        playerOne.score += 10;


        // Hit player with 17 damage
        playerOne.Hurt(17);
    }
}

struct Weapon
{
    public string name;
    public int damage;
    
    public Weapon(string name, int damage)
    {
        this.name = name;
        this.damage = damage;
    }
}

class PlayerClass
{
    public string name;
    public int health;
    public int score;
    public Weapon weapon;

    public PlayerClass(string name, int health, int score = 0)
    {
        this.name = name;
        this.health = health;
        this.score = score;
    }

    public bool Hurt(int damage, string sourceName = "unknown source")
    {   //Return true if i am dead
        health -= damage;
        Debug.Log(name + " took " + damage.ToString() + " from " + sourceName + ".");
        if (health < 0)
        {
            Debug.Log(name + " died...");
        }
        return health < 0;
    }
}

class EnemyClass
{
    public string name;
    public int health;
    public Weapon weapon;

    public EnemyClass(string name, int health)
    {
        this.name = name;
        this.health = health;
        weapon = new("claws", 5);
    }

    public bool Attack(PlayerClass player)
    {   //Return true if target is dead
        return player.Hurt(weapon.damage, name + "'s " + weapon.name);
    }
}
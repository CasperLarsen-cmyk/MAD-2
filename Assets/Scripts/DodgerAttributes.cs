using System;
using UnityEngine;

[Serializable]
public class DodgerAttributes
{
    [SerializeField] int maxHP;
    [SerializeField] int HP;
    [SerializeField] int score;

    public DodgerAttributes(int maxHP)
    {
        this.maxHP = maxHP;
        this.HP = maxHP;
    }
    public DodgerAttributes(int maxHP, int score)
    {
        this.maxHP = maxHP;
        this.HP = maxHP;
        this.score = score;
    }

    public int GetMaxHP()
    {
        return this.maxHP;
    }

    public int GetHP()
    {
        return this.HP;
    }
    
    public int GetScore()
    {
        return this.score;
    }

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
    }

    public void SetHP(int HP)
    {
        this.HP = HP;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public int AddScore(int score)
    {
        this.score += score;
        return this.score;
    }

    public bool TakeDamage(int damage)
    {
        this.HP = System.Math.Clamp(this.HP - damage, 0, this.maxHP);
        return HP <= 0;
    }
}

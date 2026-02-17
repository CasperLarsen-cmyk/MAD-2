
public class DodgerAttributes
{
    int maxHP;
    int HP;
    int score;

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
        this.HP -= damage;
        return HP <= 0;
    }
}

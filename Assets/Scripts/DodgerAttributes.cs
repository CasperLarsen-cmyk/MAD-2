
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

    public int getMaxHP()
    {
        return this.maxHP;
    }

    public int getHP()
    {
        return this.HP;
    }
    
    public int getScore()
    {
        return this.score;
    }

    public int addScore(int value)
    {
        this.score += value;
        return this.score;
    }
}

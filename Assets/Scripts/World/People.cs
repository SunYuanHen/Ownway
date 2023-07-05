public class People
{
    public string Name;
    public int Hp;
    public int Atk;
    public int Def;
    public int Spd;

    public People(string Name, int Hp, int Atk, int Def, int Spd)
    {
        this.Name = Name;
        this.Hp = Hp;
        this.Atk = Atk;
        this.Def = Def;
        this.Spd = Spd;
    }
    public void SetStat(int h, int a, int d, int s)
    {
        Hp = h;
        Atk = a;
        Def = d;
        Spd = s;
    }
}

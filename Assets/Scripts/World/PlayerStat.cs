using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public Text hpText,atkText,defText,spdText;
    public People player;
    void Start()
    {
        player = new("player", 100, 10, 10, 10);
    }
    void Update()
    {
        hpText.text = player.Hp.ToString();
        atkText.text = player.Atk.ToString();
        defText.text = player.Def.ToString();
        spdText.text = player.Spd.ToString();
    }

    int RandStat(int a,int r)
    {
        if (a < 10)r = (Random.Range(0, r));
        else if (a < 100) r = (Random.Range(0, r) / 10);
        else r = (Random.Range(0, r) / 100);
        Debug.Log("r = " + r);
        int plus = Random.Range(0, 2);
        Debug.Log("plus = " + plus);
        if (plus == 1) return a + r;
        else return a - r;
    }
    public void BattleEvent()
    {
        Debug.Log("Battle!");
        string[] enemyName = { "A", "B", "C" };
        int rN = Random.Range(0, 3);
        int rH = player.Hp, rA = player.Atk, rD = player.Def, rS = player.Spd;
        rH += RandStat(rH, 20);
        rA += RandStat(rA, 5);
        rD += RandStat(rD, 5);
        rS += RandStat(rS, 5);
        People enemy = new(enemyName[rN],rH,rA,rD,rS);
        Debug.Log("Enemy:" +"|" + enemyName[rN] + "|" +  rH + "|" +  rA + "|" + rD + "|" + rS);
    }
}

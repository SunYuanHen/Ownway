using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public Text hpText,atkText,defText,spdText;
    public Text EnameText,EhpText, EatkText, EdefText, EspdText;
    int playerNowHp = 0,rN = 0,rH = 0,rA = 0,rD = 0,rS = 0;
    public bool spawn = false;
    string[] enemyName = { "野豬", "野雞", "盜賊","小偷","三角褲野蠻人","四角褲野蠻人","骷髏射手","骷髏劍士","流氓","沒穿內褲野蠻人"};
    public People player, enemy;

    void Start()
    {
        //之後會加入存檔判斷是否生成新檔案
        player = new("player", 100, 10, 10, 10);
    }
    void Update()
    {
        hpText.text = player.Hp.ToString();
        atkText.text = player.Atk.ToString();
        defText.text = player.Def.ToString();
        spdText.text = player.Spd.ToString();
        EnameText.text = enemy.Name;
        EhpText.text = enemy.Hp.ToString();
        EatkText.text = enemy.Atk.ToString();
        EdefText.text = enemy.Def.ToString();
        EspdText.text = enemy.Spd.ToString();
    }

    public void SpawnBattleEvent()
    {
        //Spawn Enemy
        rN = Random.Range(0,enemyName.Length);
        rH = player.Hp * (100 + Random.Range(0, 50) - 50) / 100;
        rA = player.Atk * (100 + Random.Range(0, 30) - 30) / 100;
        rD = player.Def * (100 + Random.Range(0, 30) - 30) / 100;
        rS = player.Spd * (100 + Random.Range(0, 30) - 30) / 100;
        enemy = new (enemyName[rN], rH, rA, rD, rS);
        Debug.Log("Enemy:" + enemyName[rN] + "|" +  rH + "|" +  rA + "|" + rD + "|" + rS);
        //Set player Hp
        playerNowHp = player.Hp;
        spawn = true;
    }

    public bool End()
    {
        if (playerNowHp == 0 || enemy.Hp == 0)
        {
            spawn = false;
            return true;
        }
        else return false;
    }

    public bool Spawned()
    {
        if (spawn) return true;
        else return false;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public Text nameText,hpText,atkText,defText,spdText;
    public Text hpText2, atkText2, defText2, spdText2;
    public Text EnameText,EhpText, EatkText, EdefText, EspdText;
    int playerNowHp = 0,rN = 0,rH = 0,rA = 0,rD = 0,rS = 0,gameMode = 3;
    public bool spawn = false,playerDown = false;
    public People player;
    public People enemy = new("", 0, 0, 0, 0);
    public GameObject Events;
    float Timer = 0,RoundTime;

    void Start()
    {
        //之後會加入存檔判斷是否生成新檔案
        player = new("玩家", 100, 20, 20, 20);
    }
    void Update()
    {
        //text
        hpText.text =  "HP\t " + player.Hp.ToString();
        atkText.text = "ATK\t " + player.Atk.ToString();
        defText.text = "DEF\t " + player.Def.ToString();
        spdText.text = "SPD\t " + player.Spd.ToString();
        nameText.text = player.Name.ToString();
        hpText2.text = "HP\t " + playerNowHp.ToString();
        atkText2.text = "ATK\t " + player.Atk.ToString();
        defText2.text = "DEF\t " + player.Def.ToString();
        spdText2.text = "SPD\t " + player.Spd.ToString();
        EnameText.text = enemy.Name;
        EhpText.text = "HP\t " + enemy.Hp.ToString();
        EatkText.text = "ATK\t " + enemy.Atk.ToString();
        EdefText.text = "DEF\t " + enemy.Def.ToString();
        EspdText.text = "SPD\t " + enemy.Spd.ToString();
        //mode
        gameMode = Events.GetComponent<EventsLoadonMap>().GetGameMode();
        if(gameMode == 0 || gameMode == 2)//遇敵
        {
            Timer += Time.deltaTime;
            if(Timer > RoundTime)
            {
                Timer = 0;
                Battle();
            }
        }
    }

    public void SpawnBattleEvent()
    {
        string[] enemyName = { "野豬", "野雞", "盜賊", "小偷", "野蠻人", "骷髏射手", "骷髏劍士", "流氓" };
        //Spawn Enemy
        rN = Random.Range(0,enemyName.Length);
        rH = Random.Range(100, player.Hp);
        rA = Random.Range(10, player.Atk);
        rD = Random.Range(10, player.Def);
        rS = Random.Range(10, player.Spd);
        enemy = new (enemyName[rN], rH, rA, rD, rS);
        //Set player Hp
        playerNowHp = player.Hp;
        spawn = true;
    }

    public void SpawnBossEvent()
    {
        string[] enemyName = { "野豬王","盜賊老大", "神偷", "蠻人領主", "飛龍","巨人"};
        rN = Random.Range(0, enemyName.Length);
        rH = Random.Range(player.Hp,5000);
        if (player.Atk > 100) rA = Random.Range(player.Atk, 1000);
        else rA = Random.Range(player.Atk,100);
        rD = Random.Range(10, player.Def);
        rS = player.Spd * (100 + Random.Range(1, 50) - 50) / 100;
        enemy = new(enemyName[rN], rH, rA, rD, rS);
        //Set player Hp
        playerNowHp = player.Hp;
        spawn = true;
    }

    void Battle()
    {
        int enemy_GotDamage = player.Atk - enemy.Def, player_GotDamage = enemy.Atk - player.Def;
        bool enemyDown = false;
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;//傷害不得低於0
        if (player_GotDamage <= 0) player_GotDamage = 1;//傷害不得低於0
        if (gameMode == 3) RoundTime = 0.5f;
        else if(gameMode == 0)
        {
            if (enemy_GotDamage < 10 && player_GotDamage < 10) RoundTime = 0.05f;
            else RoundTime = 0.3f;
        }
        if (player.Spd >= enemy.Spd)//玩家比敵人快
        {

            enemy.Hp -= enemy_GotDamage;//玩家攻擊
            if (enemy.Hp <= 0)//敵人死亡
            {
                enemy.Hp = 0;
                enemyDown = true;
                Upgrade();//角色升級
            }
            if (enemyDown) player_GotDamage = 0;
            playerNowHp -= player_GotDamage;//敵人攻擊
            if (playerNowHp <= 0)//玩家死亡
            {
                playerNowHp = 0;
                playerDown = true;
            }
      
        }
        else//敵人比玩家快
        {
            if (enemyDown) player_GotDamage = 0;
            playerNowHp -= player_GotDamage;//敵人攻擊
            if (playerNowHp <= 0)//玩家死亡
            {
                playerNowHp = 0;
                playerDown = true;
            }
            if (playerDown) enemy_GotDamage = 0;
            enemy.Hp -= enemy_GotDamage;//玩家攻擊
            if (enemy.Hp <= 0)//敵人死亡
            {
                enemy.Hp = 0;
                enemyDown = true;
                Upgrade();//角色升級
            }
        }
    }

    public bool End()//判定戰鬥是否結束
    {
        if (enemy.Hp == 0||player.Hp == 0)
        {
            spawn = false;
            return true;
        }
        else return false;
    }

    public bool Spawned()//敵人是否生成
    {
        if (spawn) return true;
        else return false;
    }

    public bool PlayerisDead()//判定玩家是否死亡
    {
        return playerDown;
    }
    public void Upgrade()//角色升級
    {
        int n = Random.Range(0, 5);
        int up = 0;
        if(n == 0)//ATK++
        {
            up = (player.Atk * (100 + Random.Range(1, 50)) / 100) - player.Atk;
            player.Atk += up;
            Debug.Log("玩家提升 " + up + " 攻擊!");
        }
        else if (n == 1)//DEF++
        {
            up = (player.Def * (100 + Random.Range(1, 50)) / 100) - player.Def;
            player.Def += up;
            Debug.Log("玩家提升 " + up + " 防禦!");
        }
        else if(n == 2)//SPD++
        {
            up = (player.Spd * (100 + Random.Range(1, 50)) / 100) - player.Spd;
            player.Spd += up;
            Debug.Log("玩家提升 " + up + "速度!");
        }
        else//HP++
        {
            up = (player.Hp * (100 + Random.Range(1, 10)) / 100) - player.Hp;
            player.Hp += up;
            Debug.Log("玩家提升 " + up +" HP!");
        }
    }
}

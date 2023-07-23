using UnityEngine;
using UnityEngine.UI;
//At World Scene
public class PlayerStat : MonoBehaviour
{
    public Text nameText,hpText,atkText,defText,spdText;
    public Text hpText2, atkText2, defText2, spdText2;
    public Text EnameText,EhpText, EatkText, EdefText, EspdText;
    public Text UpgradeText;
    int playerNowHp = 0, rN = 0, rH = 0, rA = 0,
        rD = 0, rS = 0, gameMode = 3, round = 0, stage;
    public bool spawn = false,playerDown = false;
    public People player = new("玩家", 0, 0, 0, 0, 1);
    public People enemy = new("", 0, 0, 0, 0, 1);
    public GameObject Events,upFrame;
    float Timer = 0,RoundTime;

    void Awake()
    {
        player = new("玩家", PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
            PlayerPrefs.GetInt("playerDef"), PlayerPrefs.GetInt("playerSpd"), PlayerPrefs.GetInt("stage"));
        stage = player.stage;
    }
    void Update()
    {
        //text
        hpText.text =  "HP\t " + player.Hp.ToString();
        atkText.text = "ATK\t " + player.Atk.ToString();
        defText.text = "DEF\t " + player.Def.ToString();
        spdText.text = "SPD\t " + player.Spd.ToString();
        nameText.text = player.Name;
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
                GetComponent<AudioSource>().Stop();
                Timer = 0;
                GetComponent<AudioSource>().Play();
                Battle();
                round++;
            }
        }
    }
    //敵人生成
    public void SpawnBattleEvent()
    {
        string[] enemyName = { "野豬", "菜雞", "盜賊", "斯斯蛇", "野蠻人", "骷髏射手", "骷髏劍士", "流氓" };
        //Spawn Enemy
        rN = Random.Range(0, enemyName.Length);
        rH = Random.Range(50, player.Hp * 3 / 2) * stage;
        if (rH > 1000 * stage) rH = Random.Range(50, 1000 * stage);
        rH = CantOver10000(rH);
        rA = Random.Range(10, player.Def * 3 / 2) * stage;
        if (rA > 150 * stage) rA = Random.Range(10, 150 * stage);
        rA = CantOver10000(rA);
        rD = Random.Range(10, player.Atk) * stage;
        if (rD > 100 * stage) rD = Random.Range(10, 100 * stage);
        rD = CantOver10000(rD);
        rS = Random.Range(10, player.Spd * 3 / 2) * stage;
        if (rS > 80 * stage) rS = Random.Range(10, 80 * stage);
        rS = CantOver10000(rS);
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }
    //Boss生成
    public void SpawnBossEvent()
    {
        string[] enemyName = { "野豬王","盜賊老大", "神偷", "蠻人領主", "飛龍","巨人"};
        rN = Random.Range(0, enemyName.Length);
        rH = stage == 1 ? 1000 : 5000;
        rA = stage == 1 ? 150 : 1500;
        rD = stage == 1 ? 100 : 1000;
        rS = stage == 1 ? 80 : 800;
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }

    int CantOver10000(int i)
    {
        return i >= 10000 ? 9999 : i;
    }
    public void StartBattle()
    {
        //Set player Hp
        playerNowHp = player.Hp;
        spawn = true;
        round = 0;
        //BOSS戰時速度固定，其餘部分依照回合數調整速度
        if (gameMode == 2) RoundTime = 1f;
        else RoundTime = 0.5f;
    }

    void Battle()
    {
        int enemy_GotDamage = player.Atk - enemy.Def,
            player_GotDamage = enemy.Atk - player.Def;
        //傷害不得低於0
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;
        if (player_GotDamage <= 0) player_GotDamage = 1;
        //BOSS戰時速度固定，其餘部分依照回合數調整速度
        if (round >= 10)
        {
            round = 0;
            RoundTime *= 0.5f;
        }
        //玩家比敵人快
        if (player.Spd >= enemy.Spd)
        {
            //玩家攻擊
            enemy.Hp -= enemy_GotDamage;
            //敵人死亡
            if (enemy.Hp <= 0)
            {
                enemy.Hp = 0;
                //角色升級
                Upgrade();
                player_GotDamage = 0;
            }
            //敵人攻擊
            playerNowHp -= player_GotDamage;
            //玩家死亡
            if (playerNowHp <= 0)
            {
                playerNowHp = 0;
                playerDown = true;
            }
        }
        //敵人比玩家快
        else
        {
            if (enemy.Hp <= 0) player_GotDamage = 0;
            //敵人攻擊
            playerNowHp -= player_GotDamage;
            //玩家死亡
            if (playerNowHp <= 0)
            {
                playerNowHp = 0;
                playerDown = true;
            }
            if (playerDown) enemy_GotDamage = 0;
            //玩家攻擊
            enemy.Hp -= enemy_GotDamage;
            //敵人死亡
            if (enemy.Hp <= 0)
            {
                enemy.Hp = 0;
                //角色升級
                Upgrade();
            }
        }
    }
    //判定戰鬥是否結束
    public bool End()
    {
        if (enemy.Hp == 0 || playerNowHp == 0)
        {
            spawn = false;
            return true;
        }
        else return false;
    }
    //敵人是否生成
    public bool Spawned()
    {
        if (spawn) return true;
        else return false;
    }
    //玩家是否死亡
    public bool PlayerisDead()
    {
        return playerDown;
    }
    //角色升級
    public void Upgrade()
    {
        int n = Random.Range(0, 5);
        int up = 0;
        //ATK++
        if (n == 0)
        {
            up = RandStat(player.Atk, 10);
            UpgradeText.text = "獲得了 " + up + "攻擊!";
            UpgradeText.color = atkText.color;
            player.Atk += up;
            player.Atk = CantOver10000(player.Atk);
        }
        //DEF++
        else if (n == 1)
        {
            up = RandStat(player.Def, 10);
            UpgradeText.text = "獲得了 " + up + "防禦!";
            UpgradeText.color = defText.color;
            player.Def += up;
            player.Def = CantOver10000(player.Def);
        }
        //SPD++
        else if (n == 2)
        {
            up = RandStat(player.Spd, 10);
            UpgradeText.text = "獲得了 " + up + "速度!";
            UpgradeText.color = spdText.color;
            player.Spd += up;
            player.Spd = CantOver10000(player.Spd);
        }
        //HP++
        else
        {
            up = RandStat(player.Hp, 50);
            UpgradeText.text = "獲得了 " + up + "生命!";
            UpgradeText.color = hpText.color;
            player.Hp += up;
            player.Hp = CantOver10000(player.Hp);
        }
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //沒有獎勵
    public void Nothing()
    {
        UpgradeText.text = "啥都沒有...";
        UpgradeText.color = Color.white;
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //陷阱
    public void Trap()
    {
        int n = Random.Range(0, 5);
        int up = 0;
        UpgradeText.color = Color.red;
        //ATK--
        if (n == 0)
        {
            up = RandStat(player.Atk, 5);
            UpgradeText.text = "陷阱!失去 " + up + "攻擊!";
            player.Atk -= up;
        }
        //DEF--
        else if (n == 1)
        {
            up = RandStat(player.Def, 5);
            UpgradeText.text = "陷阱!失去 " + up + "防禦!";
            player.Def -= up;
        }
        //SPD--
        else if (n == 2)
        {
            up = RandStat(player.Spd, 5);
            UpgradeText.text = "陷阱!失去 " + up + "速度!";
            player.Spd -= up;
        }
        //HP--
        else
        {
            up = RandStat(player.Hp, 20);
            UpgradeText.text = "陷阱!失去 " + up + "生命!";
            player.Hp -= up;
        }
        GetComponent<AudioSource>().Play();
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //取得(失去)隨機能力
    int RandStat(int stat,int range)
    {
        int total = range > 10 ? Random.Range(10, range) : Random.Range(1, range);
        return total;
    }
    public void CleanText()
    {
        UpgradeText.text = "";
        upFrame.transform.position = new Vector3(0,8f,0);
    }
    //得到數據
    public int GetStats(int i)
    {
        switch (i)
        {
            case 1:
                return player.Hp;
            case 2:
                return player.Atk;
            case 3:
                return player.Def;
            case 4:
                return player.Spd;
            case 5:
                return player.stage;
        }
        return 0;
    }
    //下一關
    public void NextStage()
    {
        player.stage++;
        stage = player.stage;
        PlayerPrefs.SetInt("stage",stage);
    }
    //金手指，沒事別用
    public void Godmode()
    {
        player.Hp = 9999;
        player.Atk = 9999;
        player.Def = 9999;
        player.Spd = 9999;
    }
}
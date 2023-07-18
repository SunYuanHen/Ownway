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
        rD = 0, rS = 0, gameMode = 3, round = 0, stage = 0;
    public bool spawn = false,playerDown = false;
    public People player = new("玩家", 0, 0, 0, 0, 1);
    public People enemy = new("", 0, 0, 0, 0, 1);
    public GameObject Events,upFrame;
    float Timer = 0,RoundTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("playerHp"))
            player = new("玩家", PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
            PlayerPrefs.GetInt("playerDef"), PlayerPrefs.GetInt("playerSpd"), PlayerPrefs.GetInt("stage"));      
    }
    void Awake()
    {
        stage = player.stage;
    }
    void Update()
    {
        if (player.Hp == 0) player = new("玩家", 125, 25, 25, 25, 0);
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
        string[] enemyName = { "野豬", "野雞", "盜賊", "斯斯蛇", "野蠻人", "骷髏射手", "骷髏劍士", "流氓" };
        //Spawn Enemy
        rN = Random.Range(0, enemyName.Length) * stage;
        rH = Random.Range(50, player.Hp * 3 / 2) * stage;
        rA = Random.Range(10, player.Atk * 4 / 3) * stage;
        rD = Random.Range(10, player.Atk + 5) * stage;
        rS = Random.Range(10, player.Spd * 3 / 2) * stage;
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }
    //Boss生成
    public void SpawnBossEvent()
    {
        string[] enemyName = { "野豬王","盜賊老大", "神偷", "蠻人領主", "飛龍","巨人"};
        rN = Random.Range(0, enemyName.Length);
        rH = 1000 * stage;
        rA = 150 * stage;
        rD = 100 * stage;
        rS = 80 * stage;
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }

    public void StartBattle()
    {
        //Set player Hp
        playerNowHp = player.Hp;
        spawn = true;
        round = 0;
    }

    void Battle()
    {
        int enemy_GotDamage = player.Atk - enemy.Def, 
            player_GotDamage = enemy.Atk - player.Def;
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;//傷害不得低於0
        if (player_GotDamage <= 0) player_GotDamage = 1;//傷害不得低於0
        //BOSS戰時速度固定，其餘部分依照回合數調整速度
        if (gameMode == 3) RoundTime = 1f;
        else if (round > 50) RoundTime = 0.02f;
        else if (round > 30) RoundTime = 0.05f;
        else if (round > 10) RoundTime = 0.1f;
        else RoundTime = 0.5f;
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
            up = RandStat(player.Atk, 50);
            UpgradeText.text = "獲得了 " + up + "攻擊!";
            UpgradeText.color = atkText.color;
            player.Atk += up;
        }
        //DEF++
        else if (n == 1)
        {
            up = RandStat(player.Def, 50);
            UpgradeText.text = "獲得了 " + up + "防禦!";
            UpgradeText.color = defText.color;
            player.Def += up;
        }
        //SPD++
        else if (n == 2)
        {
            up = RandStat(player.Spd, 50);
            UpgradeText.text = "獲得了 " + up + "速度!";
            UpgradeText.color = spdText.color;
            player.Spd += up;
        }
        //HP++
        else
        {
            up = RandStat(player.Hp, 10);
            UpgradeText.text = "獲得了 " + up + "生命!";
            UpgradeText.color = hpText.color;
            player.Hp += up;
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
            up = RandStat(player.Atk, 50);
            UpgradeText.text = "陷阱!失去 " + up + "攻擊!";
            player.Atk -= up;
        }
        //DEF--
        else if (n == 1)
        {
            up = RandStat(player.Def, 50);
            UpgradeText.text = "陷阱!失去 " + up + "防禦!";
            player.Def -= up;
        }
        //SPD--
        else if (n == 2)
        {
            up = RandStat(player.Spd, 50);
            UpgradeText.text = "陷阱!失去 " + up + "速度!";
            player.Spd -= up;
        }
        //HP--
        else
        {
            up = RandStat(player.Hp, 10);
            UpgradeText.text = "陷阱!失去 " + up + "生命!";
            player.Hp -= up;
        }
        GetComponent<AudioSource>().Play();
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //取得(失去)隨機能力
    int RandStat(int stat,int range)
    {
        int total = ((stat * (100 + Random.Range(1, range)) / 100) - stat) * stage;
        return total == 0 ? 1 : total;
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
            default:
                return 0;
        }
    }
    //金手指
    public void Godmode()
    {
        player.Hp += 10000 * stage;
        player.Atk += 5000 * stage;
        player.Def += 10000 * stage;
    }
}
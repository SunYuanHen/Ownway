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
    public People player = new("���a", 0, 0, 0, 0, 1);
    public People enemy = new("", 0, 0, 0, 0, 1);
    public GameObject Events,upFrame;
    float Timer = 0,RoundTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("playerHp"))
            player = new("���a", PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
            PlayerPrefs.GetInt("playerDef"), PlayerPrefs.GetInt("playerSpd"), PlayerPrefs.GetInt("stage"));      
    }
    void Awake()
    {
        stage = player.stage;
    }
    void Update()
    {
        if (player.Hp == 0) player = new("���a", 125, 25, 25, 25, 0);
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
        if(gameMode == 0 || gameMode == 2)//�J��
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
    //�ĤH�ͦ�
    public void SpawnBattleEvent()
    {
        string[] enemyName = { "����", "����", "�s��", "�����D", "���Z�H", "�u�\�g��", "�u�\�C�h", "�y�]" };
        //Spawn Enemy
        rN = Random.Range(0, enemyName.Length) * stage;
        rH = Random.Range(50, player.Hp * 3 / 2) * stage;
        rA = Random.Range(10, player.Atk * 4 / 3) * stage;
        rD = Random.Range(10, player.Atk + 5) * stage;
        rS = Random.Range(10, player.Spd * 3 / 2) * stage;
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }
    //Boss�ͦ�
    public void SpawnBossEvent()
    {
        string[] enemyName = { "���ޤ�","�s��Ѥj", "����", "�Z�H��D", "���s","���H"};
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
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;//�ˮ`���o�C��0
        if (player_GotDamage <= 0) player_GotDamage = 1;//�ˮ`���o�C��0
        //BOSS�Ԯɳt�שT�w�A��l�����̷Ӧ^�X�ƽվ�t��
        if (gameMode == 3) RoundTime = 1f;
        else if (round > 50) RoundTime = 0.02f;
        else if (round > 30) RoundTime = 0.05f;
        else if (round > 10) RoundTime = 0.1f;
        else RoundTime = 0.5f;
        //���a��ĤH��
        if (player.Spd >= enemy.Spd)
        {
            //���a����
            enemy.Hp -= enemy_GotDamage;
            //�ĤH���`
            if (enemy.Hp <= 0)
            {
                enemy.Hp = 0;
                //����ɯ�
                Upgrade();
                player_GotDamage = 0;
            }
            //�ĤH����
            playerNowHp -= player_GotDamage;
            //���a���`
            if (playerNowHp <= 0)
            {
                playerNowHp = 0;
                playerDown = true;
            }
        }
        //�ĤH�񪱮a��
        else
        {
            if (enemy.Hp <= 0) player_GotDamage = 0;
            //�ĤH����
            playerNowHp -= player_GotDamage;
            //���a���`
            if (playerNowHp <= 0)
            {
                playerNowHp = 0;
                playerDown = true;
            }
            if (playerDown) enemy_GotDamage = 0;
            //���a����
            enemy.Hp -= enemy_GotDamage;
            //�ĤH���`
            if (enemy.Hp <= 0)
            {
                enemy.Hp = 0;
                //����ɯ�
                Upgrade();
            }
        }
    }
    //�P�w�԰��O�_����
    public bool End()
    {
        if (enemy.Hp == 0 || playerNowHp == 0)
        {
            spawn = false;
            return true;
        }
        else return false;
    }
    //�ĤH�O�_�ͦ�
    public bool Spawned()
    {
        if (spawn) return true;
        else return false;
    }
    //���a�O�_���`
    public bool PlayerisDead()
    {
        return playerDown;
    }
    //����ɯ�
    public void Upgrade()
    {
        int n = Random.Range(0, 5);
        int up = 0;
        //ATK++
        if (n == 0)
        {
            up = RandStat(player.Atk, 50);
            UpgradeText.text = "��o�F " + up + "����!";
            UpgradeText.color = atkText.color;
            player.Atk += up;
        }
        //DEF++
        else if (n == 1)
        {
            up = RandStat(player.Def, 50);
            UpgradeText.text = "��o�F " + up + "���m!";
            UpgradeText.color = defText.color;
            player.Def += up;
        }
        //SPD++
        else if (n == 2)
        {
            up = RandStat(player.Spd, 50);
            UpgradeText.text = "��o�F " + up + "�t��!";
            UpgradeText.color = spdText.color;
            player.Spd += up;
        }
        //HP++
        else
        {
            up = RandStat(player.Hp, 10);
            UpgradeText.text = "��o�F " + up + "�ͩR!";
            UpgradeText.color = hpText.color;
            player.Hp += up;
        }
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //�S�����y
    public void Nothing()
    {
        UpgradeText.text = "ԣ���S��...";
        UpgradeText.color = Color.white;
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //����
    public void Trap()
    {
        int n = Random.Range(0, 5);
        int up = 0;
        UpgradeText.color = Color.red;
        //ATK--
        if (n == 0)
        {
            up = RandStat(player.Atk, 50);
            UpgradeText.text = "����!���h " + up + "����!";
            player.Atk -= up;
        }
        //DEF--
        else if (n == 1)
        {
            up = RandStat(player.Def, 50);
            UpgradeText.text = "����!���h " + up + "���m!";
            player.Def -= up;
        }
        //SPD--
        else if (n == 2)
        {
            up = RandStat(player.Spd, 50);
            UpgradeText.text = "����!���h " + up + "�t��!";
            player.Spd -= up;
        }
        //HP--
        else
        {
            up = RandStat(player.Hp, 10);
            UpgradeText.text = "����!���h " + up + "�ͩR!";
            player.Hp -= up;
        }
        GetComponent<AudioSource>().Play();
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //���o(���h)�H����O
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
    //�o��ƾ�
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
    //�����
    public void Godmode()
    {
        player.Hp += 10000 * stage;
        player.Atk += 5000 * stage;
        player.Def += 10000 * stage;
    }
}
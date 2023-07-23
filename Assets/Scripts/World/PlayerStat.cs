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
    public People player = new("���a", 0, 0, 0, 0, 1);
    public People enemy = new("", 0, 0, 0, 0, 1);
    public GameObject Events,upFrame;
    float Timer = 0,RoundTime;

    void Awake()
    {
        player = new("���a", PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
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
    //Boss�ͦ�
    public void SpawnBossEvent()
    {
        string[] enemyName = { "���ޤ�","�s��Ѥj", "����", "�Z�H��D", "���s","���H"};
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
        //BOSS�Ԯɳt�שT�w�A��l�����̷Ӧ^�X�ƽվ�t��
        if (gameMode == 2) RoundTime = 1f;
        else RoundTime = 0.5f;
    }

    void Battle()
    {
        int enemy_GotDamage = player.Atk - enemy.Def,
            player_GotDamage = enemy.Atk - player.Def;
        //�ˮ`���o�C��0
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;
        if (player_GotDamage <= 0) player_GotDamage = 1;
        //BOSS�Ԯɳt�שT�w�A��l�����̷Ӧ^�X�ƽվ�t��
        if (round >= 10)
        {
            round = 0;
            RoundTime *= 0.5f;
        }
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
            up = RandStat(player.Atk, 10);
            UpgradeText.text = "��o�F " + up + "����!";
            UpgradeText.color = atkText.color;
            player.Atk += up;
            player.Atk = CantOver10000(player.Atk);
        }
        //DEF++
        else if (n == 1)
        {
            up = RandStat(player.Def, 10);
            UpgradeText.text = "��o�F " + up + "���m!";
            UpgradeText.color = defText.color;
            player.Def += up;
            player.Def = CantOver10000(player.Def);
        }
        //SPD++
        else if (n == 2)
        {
            up = RandStat(player.Spd, 10);
            UpgradeText.text = "��o�F " + up + "�t��!";
            UpgradeText.color = spdText.color;
            player.Spd += up;
            player.Spd = CantOver10000(player.Spd);
        }
        //HP++
        else
        {
            up = RandStat(player.Hp, 50);
            UpgradeText.text = "��o�F " + up + "�ͩR!";
            UpgradeText.color = hpText.color;
            player.Hp += up;
            player.Hp = CantOver10000(player.Hp);
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
            up = RandStat(player.Atk, 5);
            UpgradeText.text = "����!���h " + up + "����!";
            player.Atk -= up;
        }
        //DEF--
        else if (n == 1)
        {
            up = RandStat(player.Def, 5);
            UpgradeText.text = "����!���h " + up + "���m!";
            player.Def -= up;
        }
        //SPD--
        else if (n == 2)
        {
            up = RandStat(player.Spd, 5);
            UpgradeText.text = "����!���h " + up + "�t��!";
            player.Spd -= up;
        }
        //HP--
        else
        {
            up = RandStat(player.Hp, 20);
            UpgradeText.text = "����!���h " + up + "�ͩR!";
            player.Hp -= up;
        }
        GetComponent<AudioSource>().Play();
        upFrame.transform.position = new Vector3(6.1f, -1.3f, 0);
    }
    //���o(���h)�H����O
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
        }
        return 0;
    }
    //�U�@��
    public void NextStage()
    {
        player.stage++;
        stage = player.stage;
        PlayerPrefs.SetInt("stage",stage);
    }
    //������A�S�ƧO��
    public void Godmode()
    {
        player.Hp = 9999;
        player.Atk = 9999;
        player.Def = 9999;
        player.Spd = 9999;
    }
}
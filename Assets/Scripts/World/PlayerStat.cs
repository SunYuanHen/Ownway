using UnityEngine;
using UnityEngine.UI;
//At World Scene
public class PlayerStat : MonoBehaviour
{
    public Text nameText,hpText,atkText,defText,spdText;
    public Text hpText2, atkText2, defText2, spdText2;
    public Text EnameText,EhpText, EatkText, EdefText, EspdText;
    public Text UpgradeText;
    int playerNowHp = 0,rN = 0,rH = 0,rA = 0,rD = 0,rS = 0,gameMode = 3,round = 0;
    public bool spawn = false,playerDown = false;
    public People player = new("���a", 0, 0, 0, 0);
    public People enemy = new("", 0, 0, 0, 0);
    public GameObject Events,upFrame;
    float Timer = 0,RoundTime;

    void Start()
    {
        if(PlayerPrefs.HasKey("playerHp"))
            player = new("���a",PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
            PlayerPrefs.GetInt("playerDef"), PlayerPrefs.GetInt("playerSpd"));

    }
    void Update()
    {
        if (player.Hp == 0) player = new("���a", 125, 25, 25, 25);
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

    public void SpawnBattleEvent()
    {
        string[] enemyName = { "����", "����", "�s��", "�����D", "���Z�H", "�u�\�g��", "�u�\�C�h", "�y�]" };
        //Spawn Enemy
        rN = Random.Range(0,enemyName.Length);
        rH = Random.Range(50, player.Hp * 6 / 5);
        rA = Random.Range(1, player.Atk * 11 / 10);
        rD = Random.Range(1, player.Atk - 10);
        rS = Random.Range(1, player.Spd * 3 / 2);
        enemy = new (enemyName[rN], rH, rA, rD, rS);
        StartBattle();
    }

    public void SpawnBossEvent()
    {
        string[] enemyName = { "���ޤ�","�s��Ѥj", "����", "�Z�H��D", "���s","���H"};
        rN = Random.Range(0, enemyName.Length);
        rH = 1000;
        rA = 150;
        rD = 100;
        rS = 80;
        enemy = new(enemyName[rN], rH, rA, rD, rS);
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
        int enemy_GotDamage = player.Atk - enemy.Def, player_GotDamage = enemy.Atk - player.Def;
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
        else//�ĤH�񪱮a��
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
    //�P�w���a�O�_���`
    public bool PlayerisDead()
    {
        return playerDown;
    }
    //����ɯ�
    public void Upgrade()
    {
        int n = Random.Range(0, 5);
        int up = 0;
        if(n == 0)//ATK++
        {
            up = (player.Atk * (100 + Random.Range(1, 50)) / 100) - player.Atk;
            UpgradeText.text = "��o�F " + up + "����!";
            UpgradeText.color = atkText.color;
            player.Atk += up;
        }
        else if (n == 1)//DEF++
        {
            up = (player.Def * (100 + Random.Range(1, 50)) / 100) - player.Def;
            UpgradeText.text = "��o�F " + up + "���m!";
            UpgradeText.color = defText.color;
            player.Def += up;
        }
        else if(n == 2)//SPD++
        {
            up = (player.Spd * (100 + Random.Range(1, 50)) / 100) - player.Spd;
            UpgradeText.text = "��o�F " + up + "�t��!";
            UpgradeText.color = spdText.color;
            player.Spd += up;
        }
        else//HP++
        {
            up = (player.Hp * (100 + Random.Range(1, 10)) / 100) - player.Hp;
            UpgradeText.text = "��o�F " + up + "�ͩR!";
            UpgradeText.color = hpText.color;
            player.Hp += up;
        }
        upFrame.transform.position = new Vector3(6f, -1.3f, 0);
    }
    public void CleanText()
    {
        UpgradeText.text = "";
        upFrame.transform.position = new Vector3(0,8f,0);
    }

    public void EndGame()
    {
        player.Hp = 0;
    }

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
            default:
                return 0;
        }
    }
    //�����
    public void Godmode()
    {
        player.Hp += 10000;
        player.Atk += 5000;
        player.Def += 10000;
    }
}
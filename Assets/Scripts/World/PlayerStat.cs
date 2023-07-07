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
        //����|�[�J�s�ɧP�_�O�_�ͦ��s�ɮ�
        player = new("���a", 100, 20, 20, 20);
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
        if(gameMode == 0 || gameMode == 2)//�J��
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
        string[] enemyName = { "����", "����", "�s��", "�p��", "���Z�H", "�u�\�g��", "�u�\�C�h", "�y�]" };
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
        string[] enemyName = { "���ޤ�","�s��Ѥj", "����", "�Z�H��D", "���s","���H"};
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
        if (enemy_GotDamage <= 0) enemy_GotDamage = 1;//�ˮ`���o�C��0
        if (player_GotDamage <= 0) player_GotDamage = 1;//�ˮ`���o�C��0
        if (gameMode == 3) RoundTime = 0.5f;
        else if(gameMode == 0)
        {
            if (enemy_GotDamage < 10 && player_GotDamage < 10) RoundTime = 0.05f;
            else RoundTime = 0.3f;
        }
        if (player.Spd >= enemy.Spd)//���a��ĤH��
        {

            enemy.Hp -= enemy_GotDamage;//���a����
            if (enemy.Hp <= 0)//�ĤH���`
            {
                enemy.Hp = 0;
                enemyDown = true;
                Upgrade();//����ɯ�
            }
            if (enemyDown) player_GotDamage = 0;
            playerNowHp -= player_GotDamage;//�ĤH����
            if (playerNowHp <= 0)//���a���`
            {
                playerNowHp = 0;
                playerDown = true;
            }
      
        }
        else//�ĤH�񪱮a��
        {
            if (enemyDown) player_GotDamage = 0;
            playerNowHp -= player_GotDamage;//�ĤH����
            if (playerNowHp <= 0)//���a���`
            {
                playerNowHp = 0;
                playerDown = true;
            }
            if (playerDown) enemy_GotDamage = 0;
            enemy.Hp -= enemy_GotDamage;//���a����
            if (enemy.Hp <= 0)//�ĤH���`
            {
                enemy.Hp = 0;
                enemyDown = true;
                Upgrade();//����ɯ�
            }
        }
    }

    public bool End()//�P�w�԰��O�_����
    {
        if (enemy.Hp == 0||player.Hp == 0)
        {
            spawn = false;
            return true;
        }
        else return false;
    }

    public bool Spawned()//�ĤH�O�_�ͦ�
    {
        if (spawn) return true;
        else return false;
    }

    public bool PlayerisDead()//�P�w���a�O�_���`
    {
        return playerDown;
    }
    public void Upgrade()//����ɯ�
    {
        int n = Random.Range(0, 5);
        int up = 0;
        if(n == 0)//ATK++
        {
            up = (player.Atk * (100 + Random.Range(1, 50)) / 100) - player.Atk;
            player.Atk += up;
            Debug.Log("���a���� " + up + " ����!");
        }
        else if (n == 1)//DEF++
        {
            up = (player.Def * (100 + Random.Range(1, 50)) / 100) - player.Def;
            player.Def += up;
            Debug.Log("���a���� " + up + " ���m!");
        }
        else if(n == 2)//SPD++
        {
            up = (player.Spd * (100 + Random.Range(1, 50)) / 100) - player.Spd;
            player.Spd += up;
            Debug.Log("���a���� " + up + "�t��!");
        }
        else//HP++
        {
            up = (player.Hp * (100 + Random.Range(1, 10)) / 100) - player.Hp;
            player.Hp += up;
            Debug.Log("���a���� " + up +" HP!");
        }
    }
}

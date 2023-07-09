using UnityEngine;

public class EventsLoadonMap : MonoBehaviour
{
    public GameObject horizontal,player,BattleCanvas,DeadCanvas,Scripts;
    int[,] eventSaver = new int[5, 5];
    int playerx, playery;
    int gamemode = 3;//0:普通戰鬥,1:寶物,2:BOSS戰.3:探索,4:死亡
    Transform vertical, frame, delEvent;
    void Start()
    {
        //Set eventSaver to all 0
        for (int i = 0; i < 5; i++) { for (int j = 0; j < 5; j++) eventSaver[i, j] = 3; }
        SpawnEvent();
        playerx = 2;
        playery = playerx;
        player.transform.position = new Vector3(0, 1.38f, 0);
    }

    void Update()
    {
        if (gamemode == 3)//探索模式下玩家可以透過WASD移動
        {
            if (Input.GetKeyDown(KeyCode.A) && player.transform.GetComponent<Transform>().position.x > -2.5f)
            {
                player.transform.Translate(-1.25f, 0, 0);
                playerx--;
            }
            else if (Input.GetKeyDown(KeyCode.D) && player.transform.GetComponent<Transform>().position.x < 2.5f)
            {
                player.transform.Translate(1.25f, 0, 0);
                playerx++;
            }
            else if (Input.GetKeyDown(KeyCode.W) && player.transform.GetComponent<Transform>().position.y < 3.9f)
            {
                player.transform.Translate(0, 1.3f, 0);
                playery--;
            }
            else if (Input.GetKeyDown(KeyCode.S) && player.transform.GetComponent<Transform>().position.y > -1.2f)
            {
                player.transform.Translate(0, -1.3f, 0);
                playery++;
            }
        }
        //非死亡模式下觸發事件
        if(gamemode != 4)ActiveEvent(playerx, playery);
        else DeadCanvas.transform.position = new Vector3(0, 0, 0);
    }

    public void SpawnEvent()
    {
        
        int r,randomNumber;
        bool BossSpawned = false;
        for (int i = 0;i < horizontal.transform.childCount; i++)
        {
            vertical = horizontal.transform.GetChild(i);
            for (int j = 0;j< vertical.childCount; j++)
            {
                frame = vertical.transform.GetChild(j);
                randomNumber = Random.Range(0, 100);
                //0~20 BOSS(2) 41~80 enemy(0) 81~99 chest(1)
                if (randomNumber < 21) r = 2;
                else if (randomNumber < 81) r = 0;
                else r = 1;
                if (i == 2 && j == 2) r = 3;
                if (r == 2 && BossSpawned) r = 1;
                if (r == 2 && !BossSpawned) BossSpawned = true;
                frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(r);
                eventSaver[i, j] = r;
            }
        }
    }

    public void ClearEvent(int x ,int y)
    {
        vertical = horizontal.transform.GetChild(x);
        frame = vertical.transform.GetChild(y);
        delEvent = frame.GetChild(0);
        delEvent.GetComponent<EventActive>().DestoryEvent();
        eventSaver[x, y] = 3;
    }

    void ActiveEvent(int x,int y)
    {
        if(eventSaver[x,y] == 0)
        {
            gamemode = 0;//battle
            BattleCanvas.transform.position = new Vector3(0, 0, 0);
            bool spawn = Scripts.GetComponent<PlayerStat>().Spawned();
            if(!spawn)Scripts.GetComponent<PlayerStat>().SpawnBattleEvent();
            bool end = Scripts.GetComponent<PlayerStat>().End();
            if (end)
            {
                bool lose = Scripts.GetComponent<PlayerStat>().PlayerisDead();
                if (lose)gamemode = 4;
                else
                {
                    ClearEvent(x, y);
                    BattleCanvas.transform.position = new Vector3(-20f, 0, 0);
                    gamemode = 3;
                }
            }
        }
        else if(eventSaver[x, y] == 1)
        {
            gamemode = 1;//chest
            //預定增加獲得能力字幕
            Scripts.GetComponent<PlayerStat>().Upgrade();
            ClearEvent(x, y);
            gamemode = 3;
        }
        else if (eventSaver[x, y] == 2)
        {
            gamemode = 2;//Boss
            BattleCanvas.transform.position = new Vector3(0, 0, 0);
            bool spawn = Scripts.GetComponent<PlayerStat>().Spawned();
            if (!spawn) Scripts.GetComponent<PlayerStat>().SpawnBossEvent();
            bool end = Scripts.GetComponent<PlayerStat>().End();
            if (end)
            {
                bool lose = Scripts.GetComponent<PlayerStat>().PlayerisDead();
                if (lose) gamemode = 4;
                else
                {
                    ClearEvent(x, y);
                    BattleCanvas.transform.position = new Vector3(-20f, 0, 0);
                    gamemode = 3;
                }
            }
        }
        else if (eventSaver[x, y] == 4)
        {
            //show deathcanva
        }
    }

    public int GetGameMode()
    {
        return gamemode;
    }
}

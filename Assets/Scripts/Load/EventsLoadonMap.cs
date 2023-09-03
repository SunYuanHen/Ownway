using UnityEngine;
//At World Scene
public class EventsLoadonMap : MonoBehaviour
{
    public GameObject horizontal, player, BattleCanvas,DeadCanvas
        ,VictoryCanvas,scoreCanvas,Scripts, sceneControl;
    int[,] eventSaver = new int[5, 5];
    int playerx, playery,godmode = 0;
    //0:普通戰鬥,1:寶物,2:BOSS戰.3:探索,4:死亡,5:勝利,6:破關
    [SerializeField] int gameMode = 3;
    Transform vertical, frame, delEvent;
    bool BossSpawned = false,clear = false;
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
        if(Input.GetKeyDown(KeyCode.Escape))
            sceneControl.GetComponent<SceneLoader>().LoadScene("Title");
        switch (gameMode)
        {
            //探索模式
            case 3:
                //玩家透過WASD移動
                if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    && player.transform.GetComponent<Transform>().position.x > -2.5f)
                {
                    player.transform.Translate(-1.25f, 0, 0);
                    playerx--;
                    Scripts.GetComponent<PlayerStat>().CleanText();
                }
                else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    && player.transform.GetComponent<Transform>().position.x < 2.5f)
                {
                    player.transform.Translate(1.25f, 0, 0);
                    playerx++;
                    Scripts.GetComponent<PlayerStat>().CleanText();
                }
                else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    && player.transform.GetComponent<Transform>().position.y < 3.9f)
                {
                    player.transform.Translate(0, 1.3f, 0);
                    playery--;
                    Scripts.GetComponent<PlayerStat>().CleanText();
                }
                else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    && player.transform.GetComponent<Transform>().position.y > -1.2f)
                {
                    player.transform.Translate(0, -1.3f, 0);
                    playery++;
                    Scripts.GetComponent<PlayerStat>().CleanText();
                }
                //按R進行存檔並重置
                if (Input.GetKeyDown(KeyCode.R)) 
                    toSave();
                //如果打敗了BOSS
                if (!BossSpawned)
                {
                    scoreCanvas.transform.position = new Vector3(0, 0, 0);
                    if (!clear)
                    {
                        scoreCanvas.GetComponent<GameClear>().ClearGame();
                        clear = true;
                    }
                    gameMode = 6;
                }
                //測試用
                if (godmode == 0 && Input.GetKeyDown(KeyCode.G)) godmode++;
                if (godmode == 1 && Input.GetKeyDown(KeyCode.O)) godmode++;
                if (godmode == 2 && Input.GetKeyDown(KeyCode.D)) godmode++;
                break;
            //死亡模式
            case 4:
                DeadCanvas.transform.position = new Vector3(0, 0, 0);
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                    sceneControl.GetComponent<SceneLoader>().LoadScene("Title");
                break;
            //勝利模式
            case 5:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    VictoryCanvas.GetComponent<Victory>().MoveAwayCanvas();
                    GetComponent<AudioSource>().Play();
                }
                break;
            //結算模式
            case 6:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    Scripts.GetComponent<PlayerStat>().NextStage();
                    toSave();
                }
                break;
            //其餘不動作
            default:
                break;
        }
        //非死亡狀態下觸發事件
        if (gameMode != 4 && gameMode != 6) ActiveEvent(playerx, playery);
        if (godmode == 3)
        {
           godmode = 0; 
           Scripts.GetComponent<PlayerStat>().Godmode();
        }
    }
    private void toSave()
    {
        PlayerPrefs.SetInt("playerHp", Scripts.GetComponent<PlayerStat>().GetStats(1));
        PlayerPrefs.SetInt("playerAtk", Scripts.GetComponent<PlayerStat>().GetStats(2));
        PlayerPrefs.SetInt("playerDef", Scripts.GetComponent<PlayerStat>().GetStats(3));
        PlayerPrefs.SetInt("playerSpd", Scripts.GetComponent<PlayerStat>().GetStats(4));
        PlayerPrefs.SetInt("stage", Scripts.GetComponent<PlayerStat>().GetStats(5));
        sceneControl.GetComponent<SceneLoader>().LoadScene("Save");
    }
    public void SpawnEvent()
    {
        
        int r,randomNumber;
        BossSpawned = false;
        //針對地圖每一格進行事件隨機產生
        for (int i = 0;i < horizontal.transform.childCount; i++)
        {
            vertical = horizontal.transform.GetChild(i);
            for (int j = 0;j< vertical.childCount; j++)
            {
                frame = vertical.transform.GetChild(j);
                randomNumber = Random.Range(0, 100);
                //機率範圍0~20 BOSS(2) 21~80 enemy(0) 81~99 chest(1)
                if (randomNumber < 21) r = 2;
                else if (randomNumber < 81) r = 0;
                else r = 1;
                if (i == 2 && j == 2) r = 3;
                if (r == 2) 
                {
                    if (BossSpawned) r = 1;
                    else BossSpawned = true;
                }
                frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(r);
                eventSaver[i, j] = r;
            }
        }
        if (!BossSpawned)
        {
            vertical = horizontal.transform.GetChild(0);
            frame = vertical.transform.GetChild(0);
            frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(2);
            eventSaver[0, 0] = 2;
            BossSpawned = true;
        }
    }
    
    //將已完成事件清除
    public void ClearEvent(int x ,int y)
    {
        vertical = horizontal.transform.GetChild(x);
        frame = vertical.transform.GetChild(y);
        delEvent = frame.GetChild(0);
        delEvent.GetComponent<EventActive>().DestoryEvent();
        eventSaver[x, y] = 3;
    }

    //事件發動
    void ActiveEvent(int x,int y)
    {
        //戰鬥事件
        if(eventSaver[x,y] == 0)
        {
            gameMode = 0;
            BattleCanvas.transform.position = new Vector3(0, 0, 0);
            if (!Scripts.GetComponent<PlayerStat>().Spawned())
                Scripts.GetComponent<PlayerStat>().SpawnBattleEvent();
            if (Scripts.GetComponent<PlayerStat>().End())
            {
                if (Scripts.GetComponent<PlayerStat>().PlayerisDead()) gameMode = 4;
                else
                {
                    ClearEvent(x, y);
                    VictoryCanvas.transform.position = new Vector3(0, 0, 0);
                    gameMode = 5;
                }
            }
        }
        //寶物事件
        else if (eventSaver[x, y] == 1)
        {
            
            gameMode = 1;
            //0~60: 能力提升, 61~80: 無, 81~100: 陷阱
            int randomNumber = Random.Range(0,100);
            int r;
            if (randomNumber <= 60)
                r = 0;
            else if (randomNumber <= 80)
                r = 1;
            else
                r = 2;
            switch (r)
            {
                case 0:
                    Scripts.GetComponent<PlayerStat>().Upgrade();
                    GetComponent<AudioSource>().Play();
                    break;
                case 1:
                    Scripts.GetComponent<PlayerStat>().Nothing();
                    break;
                case 2:
                    Scripts.GetComponent<PlayerStat>().Trap();
                    break;

            }
            ClearEvent(x, y);
            gameMode = 3;
        }
        //Boss事件
        else if (eventSaver[x, y] == 2)
        {
            gameMode = 2;
            BattleCanvas.transform.position = new Vector3(0, 0, 0);
            if (!Scripts.GetComponent<PlayerStat>().Spawned()) 
                Scripts.GetComponent<PlayerStat>().SpawnBossEvent();
            if (Scripts.GetComponent<PlayerStat>().End())
            {
                if (Scripts.GetComponent<PlayerStat>().PlayerisDead()) gameMode = 4;
                else
                {
                    ClearEvent(x, y);
                    VictoryCanvas.transform.position = new Vector3(0, 0, 0);
                    gameMode = 5;
                    BossSpawned = false;
                }
            }
        }
    }

    //取得並回傳遊戲模式
    public int GetGameMode()
    {
        return gameMode;
    }

    //改變遊戲模式
    public void SetGameMode(int mode)
    {
        gameMode = mode;
    }
}

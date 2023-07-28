# Ownway
(2023/7/28施工中...)
## 簡介
玩家透過在地圖中移動觸發事件，目標是打倒地圖中的BOSS<br>
打倒BOSS會根據玩家能力值評分，目前只有兩關。
## 主要介紹功能
* 隨機事件
* 自動戰鬥
* 存讀檔
## 隨機事件
透過機率去隨機分配事件，每次玩家進入世界時會隨機生成。<br>
地圖構成宣告變數如下:(**粗體**為名字，*斜體字*為類型)
>**horizontal**:*GameObject*,整個地圖
>>**vertical**:*Transform*,行
>>>**frame**:*Transform*,格<br>

*於EventsLoadonMap.cs中*
```C#
public void SpawnEvent()
    {
        //用於分配事件和儲存機率
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
                //機率範圍: 0~20為BOSS(2) 21~80為敵人(0) 81~99為寶箱(1)
                if (randomNumber < 21) r = 2;
                else if (randomNumber < 81) r = 0;
                else r = 1;
                //中間設為空事件
                if (i == 2 && j == 2) r = 3;
                //根據BOSS是否已生成決定為BOSS或寶箱
                if (r == 2) 
                {
                    if (BossSpawned) r = 1;
                    else BossSpawned = true;
                }
                //引用frame內建函式生成圖片
                frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(r);
                eventSaver[i, j] = r;
            }
        }
        //如果一個BOSS都沒有，使最左上方變更為BOSS
        if (!BossSpawned)
        {
            vertical = horizontal.transform.GetChild(0);
            frame = vertical.transform.GetChild(0);
            frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(2);
            eventSaver[0, 0] = 2;
            BossSpawned = true;
        }
    }
```
## 自動戰鬥
在接觸戰鬥(BOSS)事件後，會隨機產生敵人能力(BOSS為固定)。<br>
戰鬥會以速度較快者優先行動，先將對方生命歸0者獲勝。<br>
*於PlayerStat.cs中*
```C#
//敵人生成
    public void SpawnBattleEvent()
    {
        string[] enemyName = { "敵人1", "敵人2", "敵人3"};
        //能力隨機生成在不超過BOSS或玩家的條件下
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
        string[] enemyName = { "BOSS1","BOSS2", "BOSS3"};
        rN = Random.Range(0, enemyName.Length);
        rH = stage == 1 ? 1000 : 5000;
        rA = stage == 1 ? 150 : 1500;
        rD = stage == 1 ? 100 : 1000;
        rS = stage == 1 ? 80 : 800;
        enemy = new(enemyName[rN], rH, rA, rD, rS, stage);
        StartBattle();
    }
    //預設能力最大為9999
    int CantOver10000(int i)
    {
        return i >= 10000 ? 9999 : i;
    }
    public void StartBattle()
    {
        //用於戰鬥中顯示目前剩餘血量
        playerNowHp = player.Hp;
        spawn = true;
        round = 0;
        //BOSS戰時速度較慢，其餘較快
        if (gameMode == 2) RoundTime = 1f;
        else RoundTime = 0.5f;
    }
    //根據速度先決定誰先攻，造成傷害為Atk-Def。傷害最低為1
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
```

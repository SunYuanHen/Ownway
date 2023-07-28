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
地圖構成主要如下:(**粗體**為名字，*斜體字*為類型)
>**horizontal**:*GameObject*,整個地圖
>>**vertical**:*Transform*,行
>>>**frame**:*Transform*,格

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

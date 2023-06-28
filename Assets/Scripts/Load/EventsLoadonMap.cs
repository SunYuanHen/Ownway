using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsLoadonMap : MonoBehaviour
{
    public GameObject horizontal;
    void Start()
    {
        SpawnEvent();
    }

    public void SpawnEvent()
    {
        
        int r,randomNumber;
        bool BossSpawned = false;
        for (int i = 0;i < horizontal.transform.childCount; i++)
        {
            Transform vertical = horizontal.transform.GetChild(i);
            for (int j = 0;j< vertical.childCount; j++)
            {
                Transform frame = vertical.transform.GetChild(j);
                randomNumber = Random.Range(0, 100);
                //0~20 BOSS(2) 41~80 enemy(0) 81~99 chest(1)

                if (randomNumber < 21) r = 2;
                else if (randomNumber < 81) r = 0;
                else r = 1;
                if (r == 2 && BossSpawned)r = 1;
                if (r == 2 && !BossSpawned) BossSpawned = true;
                frame.GetComponent<Frame_SpawnEvent>().SpawnEvent(r);
                //Debug.Log("vertical:" + vertical.name + " frame:"+frame.name + " randomNumber: " + randomNumber + " r = " + r);
            }
        }
        Debug.Log("A NewWorld has begun~");
    }

}

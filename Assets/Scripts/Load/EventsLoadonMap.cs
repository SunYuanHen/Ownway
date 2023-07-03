using UnityEngine.SceneManagement;
using UnityEngine;

public class EventsLoadonMap : MonoBehaviour
{
    public GameObject horizontal;
    int[,] eventSaver = new int[5, 5];
    void Start()
    {
        //Set eventSaver to all 0
        for (int i = 0; i < 5; i++) { for (int j = 0; j < 5; j++) eventSaver[i, j] = 3; }
        SpawnEvent();
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("["+eventSaver[i,0]+"]" + "[" + eventSaver[i,1] + "]" +
                "[" + eventSaver[i,2] + "]" + "[" + eventSaver[i, 3] + "]" + "[" + eventSaver[i, 4] + "]");
        }
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
                eventSaver[i, j] = r;
            }
        }
    }

    public void ChangeEvent(int x ,int y)
    {
        eventSaver[x, y] = 3;
    }
}

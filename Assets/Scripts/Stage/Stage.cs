using System.IO;
using UnityEngine;
using UnityEngine.UI;
 
public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 0;
    string[] log ={
        "你沒有錢\n你唯一的財物是穿在身上的內褲\n" +
            "一到酒館老闆給了你一張地圖標著有賞金的怪物\n為了錢\n你只好踏上新的冒險...",
        "你有了點錢\n你身上的財物多了個上衣\n隔天到酒館老闆又給了你一張地圖標著有賞金的怪物" +
            "\n為了錢\n你又踏上新的冒險...",
        "你有錢了\n某天在逛街的路上\n被酒館老闆的跑車撞死了\n很可惜\n你的人生結束了..."
    };
    void Awake()
    {
        stage = PlayerPrefs.GetInt("stage");
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            NextScene();
        GetComponent<Text>().text = log[stage - 1];
    }
    
    public void NextScene()
    {
        if (stage == 3)
            sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
        else
            sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
    }
}

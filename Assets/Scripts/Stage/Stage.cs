using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 0;
    string[] log ={
        "第一關:\n一到酒館，老闆給了你一張地圖。\n標著有賞金的怪物\n你踏上新的冒險...",
        "第二關:\n一到酒館，老闆又給了你一張地圖。\n標著有賞金的新怪物\n你踏上新的冒險...",
        "恭喜破關!!"
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

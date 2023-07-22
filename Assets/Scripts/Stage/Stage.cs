using System.IO;
using UnityEngine;
using UnityEngine.UI;
 
public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 0;
    string json;
    Log log = new("");
    void Awake()
    {
        stage = PlayerPrefs.GetInt("stage");
        string filePath = $"/Scripts/Stage/log{stage}.json";
        string fullPath = (Application.dataPath + filePath);
        json = File.ReadAllText(fullPath);
        log = JsonUtility.FromJson<Log>(json);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            NextScene();
        GetComponent<Text>().text = log.log.Replace("\\n","\n");
    }
    
    public void NextScene()
    {
        if (stage == 3)
            sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
        else
            sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
    }
}

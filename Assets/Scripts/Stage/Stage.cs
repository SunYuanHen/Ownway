using System.IO;
using UnityEngine;
using UnityEngine.UI;
 
public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 1;
    string json;
    Log log = new("");
    void Awake()
    {
        stage = PlayerPrefs.GetInt("stage");
        string filePath = $"/Scripts/Stage/log{stage}.json";
        string fullPath = (Application.dataPath + filePath);
        //log = File.ReadAllText(fullPath);
        log.log.Replace("\\n", "\n");
        json = JsonUtility.ToJson(log);
        File.WriteAllText($"Assets/Scripts/Stage/log{stage}.json", json);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
        GetComponent<Text>().text = log.log;
    }
}

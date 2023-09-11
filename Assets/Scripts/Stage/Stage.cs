using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 0;
    string[] log ={
        "�Ĥ@��:\n�@��s�]�A���󵹤F�A�@�i�a�ϡC\n�еۦ�������Ǫ�\n�A��W�s���_�I...",
        "�ĤG��:\n�@��s�]�A����S���F�A�@�i�a�ϡC\n�еۦ�������s�Ǫ�\n�A��W�s���_�I...",
        "���߯}��!!"
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

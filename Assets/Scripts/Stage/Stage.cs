using System.IO;
using UnityEngine;
using UnityEngine.UI;
 
public class Stage : MonoBehaviour
{
    public GameObject sceneLoader;
    int stage = 0;
    string[] log ={
        "�A�S����\n�A�ߤ@���]���O��b���W������\n" +
            "�@��s�]���󵹤F�A�@�i�a�ϼеۦ�������Ǫ�\n���F��\n�A�u�n��W�s���_�I...",
        "�A���F�I��\n�A���W���]���h�F�ӤW��\n�j�Ѩ�s�]����S���F�A�@�i�a�ϼеۦ�������Ǫ�" +
            "\n���F��\n�A�S��W�s���_�I...",
        "�A�����F\n�Y�Ѧb�}�󪺸��W\n�Q�s�]���󪺶]�������F\n�ܥi��\n�A���H�͵����F..."
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

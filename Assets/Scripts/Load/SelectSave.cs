using UnityEngine;

public class SelectSave : MonoBehaviour
{
    public GameObject[] selection;
    public GameObject sceneLoader;
    public int selectOption = 0;//0~2 save1~3, 3 = exit

    void Update()
    {
        //上下選擇選單
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectOption == 0) selectOption = 3;
            else selectOption--;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectOption == 3) selectOption = 0;
            else selectOption++;
        }
        //改變選框位置以及大小
        gameObject.transform.position = selection[selectOption].transform.position;
        gameObject.transform.localScale = selection[selectOption].transform.localScale;
        
        //Enter或空白鍵選擇
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (selectOption == 3) sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
            //else selection[selectOption].GetComponent<>
        }
    }
}

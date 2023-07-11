using UnityEngine;

public class SelectSave : MonoBehaviour
{
    public GameObject[] selection;
    public GameObject sceneLoader;
    public int selectOption = 0;//0~2 save1~3, 3 = exit

    void Update()
    {
        //�W�U��ܿ��
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
        //���ܿ�ئ�m�H�Τj�p
        gameObject.transform.position = selection[selectOption].transform.position;
        gameObject.transform.localScale = selection[selectOption].transform.localScale;
        
        //Enter�Ϊť�����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (selectOption == 3) sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
            //else selection[selectOption].GetComponent<>
        }
    }
}

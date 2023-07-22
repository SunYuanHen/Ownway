using UnityEngine;
//At Title Scene
public class TitleSelect : MonoBehaviour
{
    public GameObject newGame, loadGame, exit,sceneLoader,quitControl;
    public int selectOption = 0;//0 = new, 1 = load, 2 = exit
    void Awake()
    {
        PlayerPrefs.SetInt("playerHp", 100);
        PlayerPrefs.SetInt("playerAtk", 25);
        PlayerPrefs.SetInt("playerDef", 25);
        PlayerPrefs.SetInt("playerSpd", 25);
        PlayerPrefs.SetInt("stage", 1);
    }
    void Update()
    {
        //上下選擇選單
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectOption == 0) selectOption = 2;
            else selectOption--;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectOption == 2) selectOption = 0;
            else selectOption++;
        }
        if (selectOption == 0)
        {
            gameObject.transform.position = newGame.transform.position;
            gameObject.transform.localScale = newGame.transform.localScale;
        }
        else if (selectOption == 1)
        {
            gameObject.transform.position = loadGame.transform.position;
            gameObject.transform.localScale = loadGame.transform.localScale;
        }
        else //selectOption == 2
        {
            gameObject.transform.position = exit.transform.position;
            gameObject.transform.localScale = exit.transform.localScale;
        }
        //Enter或空白鍵選擇
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return))
        {
            //新遊戲
            if (selectOption == 0)
                sceneLoader.GetComponent<SceneLoader>().LoadScene("Stage");
            //讀檔
            else if (selectOption == 1) 
                sceneLoader.GetComponent<SceneLoader>().LoadScene("Load");
            //離開遊戲
            else quitControl.GetComponent<Quit>().QuitGame();
        }
    }
}

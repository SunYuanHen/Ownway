using System.IO;
using UnityEngine;
using UnityEngine.UI;
//At Load Scene
public class SelectSave : MonoBehaviour
{
    public GameObject[] selection, playerLoads;
    public GameObject sceneLoader;
    Transform[] stats = new Transform[4];
    public int selectOption = 0;//0~2 save, 3 = exit
    People[] saves = new People[3];
    string[] loadData = new string[3];
    float[] loadsPosition = new float[3];

    void Start()
    {
        //載入所有存檔
        for (int i = 0; i < 3; i++)
        {
            string filePath = $"/Saves/Save{i + 1}.json";
            string fullPath = (Application.dataPath + filePath);
            loadData[i] = File.ReadAllText(fullPath);
            saves[i] = JsonUtility.FromJson<People>(loadData[i]);
            loadsPosition[i] = playerLoads[i].transform.position.y;
        }
    }
    void Update()
    {
        //設定存檔數據
        for (int i = 0; i < 3; i++)
            if (saves[i].Hp != 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    stats[j] = playerLoads[i].transform.GetChild(j);
                    switch (j)
                    {
                        case 0:
                            stats[j].GetComponent<Text>().text = "HP\t" + saves[i].Hp.ToString();
                            break;
                        case 1:
                            stats[j].GetComponent<Text>().text = "ATK\t" + saves[i].Atk.ToString();
                            break;
                        case 2:
                            stats[j].GetComponent<Text>().text = "DEF\t" + saves[i].Def.ToString();
                            break;
                        case 3:
                            stats[j].GetComponent<Text>().text = "SPD\t" + saves[i].Spd.ToString();
                            break;
                    }
                }
                playerLoads[i].transform.position = new Vector3(0, loadsPosition[i] - 10, 0);
            }
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
            switch (selectOption)
            {
                //根據位置進行存檔
                case 0:
                    PlayerPrefs.SetInt("playerHp",saves[0].Hp);
                    PlayerPrefs.SetInt("playerAtk", saves[0].Atk);
                    PlayerPrefs.SetInt("playerDef", saves[0].Def);
                    PlayerPrefs.SetInt("playerSpd", saves[0].Spd);
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
                    break;
                case 1:
                    PlayerPrefs.SetInt("playerHP", saves[1].Hp);
                    PlayerPrefs.SetInt("playerAtk", saves[1].Atk);
                    PlayerPrefs.SetInt("playerDef", saves[1].Def);
                    PlayerPrefs.SetInt("playerSpd", saves[1].Spd);
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
                    break;
                case 2:
                    PlayerPrefs.SetInt("playerHp", saves[2].Hp);
                    PlayerPrefs.SetInt("playerAtk", saves[2].Atk);
                    PlayerPrefs.SetInt("playerDef", saves[2].Def);
                    PlayerPrefs.SetInt("playerSpd", saves[2].Spd);
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
                    break;
                //返回首頁
                case 3:
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
                    break;
                default:
                    break;
            }
            
        }
    }
}

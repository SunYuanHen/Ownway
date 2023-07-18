using System.IO;
using UnityEngine;
using UnityEngine.UI;
//At Save Scene
public class SaveFile : MonoBehaviour
{
    public GameObject[] selection,playerLoads;
    public GameObject sceneLoader;
    Transform[] stats = new Transform[4];
    public int selectOption = 0;//0~2 save, 3 = exit
    People playerStat;
    People[] saves = new People[3];
    string[] loadData = new string[3];
    float[] loadsPosition = new float[3];
    void Start()
    {
        //Ū���ثe���a���
        playerStat = new("���a",
            PlayerPrefs.GetInt("playerHp"), PlayerPrefs.GetInt("playerAtk"),
            PlayerPrefs.GetInt("playerDef"), PlayerPrefs.GetInt("playerSpd"), PlayerPrefs.GetInt("stage"));
        //���J�Ҧ��s��
        for (int i = 0; i < 3; i++) loadsPosition[i] = playerLoads[i].transform.position.y;
        GetSave();

    }
    void Update()
    {
        
        //�]�w�s�ɼƾ�
        for(int i = 0; i < 3; i++)
            if(saves[i].Hp != 0) { 
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
            string json = JsonUtility.ToJson(playerStat);
            switch (selectOption)
            {
                //�ھڦ�m�i��s��
                case 0:
                    File.WriteAllText("Assets/Saves/Save1.json", json);
                    break;
                case 1:
                    File.WriteAllText("Assets/Saves/Save2.json", json);
                    break;
                case 2:
                    File.WriteAllText("Assets/Saves/Save3.json", json);
                    break;
                //��^�@��
                case 3:
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("World");
                    break;
                default:
                    break;
            }
            GetSave();
        }
    }

    void GetSave()
    {
        //���J�Ҧ��s��
        for (int i = 0; i < 3; i++)
        {
            string filePath = $"/Saves/Save{i + 1}.json";
            string fullPath = (Application.dataPath + filePath);
            loadData[i] = File.ReadAllText(fullPath);
            saves[i] = JsonUtility.FromJson<People>(loadData[i]);
        }
    }
}

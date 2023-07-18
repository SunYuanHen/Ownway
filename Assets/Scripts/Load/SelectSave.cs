using System.IO;
using UnityEngine;
using UnityEngine.UI;
//At Load Scene
public class SelectSave : MonoBehaviour
{
    public GameObject[] selection, playerLoads;
    public GameObject sceneLoader;
    public Text del;
    Transform[] stats = new Transform[4];
    public int selectOption = 0;//0~2 save, 3 = delete, 4 = exit
    People[] saves = new People[3];
    string[] loadData = new string[3];
    float[] loadsPosition = new float[3];
    public bool deleteMode = false;
    void Awake()
    {
        deleteMode = false;
    }
    void Start()
    {
        //���J�Ҧ��s��
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
        //�]�w�s�ɼƾ�
        for (int i = 0; i < 3; i++)
        {
            if (saves[i].Hp != 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    stats[j] = playerLoads[i].transform.GetChild(j);
                    SetText(i, j);
                }
                playerLoads[i].transform.position = new Vector3(0, loadsPosition[i] - 10, 0);
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    stats[j] = playerLoads[i].transform.GetChild(j);
                    SetText(i, j);
                }
                playerLoads[i].transform.position = new Vector3(0, loadsPosition[i], 0);
            }
        }
        //�W�U��ܿ��
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectOption == 0) selectOption = 3;
            else selectOption--;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectOption == 3 || selectOption == 4) selectOption = 0;
            else selectOption++;
        }
        //�bdelete�Mexit��
        if(selectOption > 2)
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                selectOption = selectOption == 3 ? 4 : 3;
        //���ܿ�ئ�m�H�Τj�p
        gameObject.transform.position = selection[selectOption].transform.position;
        gameObject.transform.localScale = selection[selectOption].transform.localScale;
        
        //Enter�Ϊť�����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectOption)
            {
                //�ھڦ�m�i��s��
                case 0:
                    if (!deleteMode)
                        SaveisReal(saves[0]);
                    else
                        DeleteSave(saves[0],1);
                    break;
                case 1:
                    if (!deleteMode)
                        SaveisReal(saves[1]);
                    else
                        DeleteSave(saves[1],2);
                    break;
                case 2:
                    if (!deleteMode)
                        SaveisReal(saves[2]);
                    else
                        DeleteSave(saves[2],3);
                    break;
                //��^����
                case 3:
                    if (!deleteMode)
                    {
                        deleteMode = true;
                        del.GetComponent<Text>().color = Color.red;
                    }
                    else
                    {
                        deleteMode = false;
                        del.GetComponent<Text>().color = Color.white;
                    }
                    break;
                case 4:
                    sceneLoader.GetComponent<SceneLoader>().LoadScene("Title");
                    break;
                default:
                    break;
            }
            
        }
    }
    //��J���e
    void SetText(int i,int j)
    {
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
    //�T�{�s��
    void SaveisReal(People save)
    {
        if (save.Hp > 0)
        {
            PlayerPrefs.SetInt("playerHp", save.Hp);
            PlayerPrefs.SetInt("playerAtk", save.Atk);
            PlayerPrefs.SetInt("playerDef", save.Def);
            PlayerPrefs.SetInt("playerSpd", save.Spd);
            PlayerPrefs.SetInt("stage", save.stage);
            sceneLoader.GetComponent<SceneLoader>().LoadScene("Stage");
        }
        else
            GetComponent<AudioSource>().Play();
    }
    //�R���s��
    void DeleteSave(People save,int i)
    {
        save.Hp = 0;
        save.Atk = 0;
        save.Def = 0;
        save.Spd = 0;
        save.stage = 0;
        string json = JsonUtility.ToJson(save);
        File.WriteAllText($"Assets/Saves/Save{i}.json", json);
    }
}

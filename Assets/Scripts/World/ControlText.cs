using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = 0;//用於判斷是否重複提示
    string[] text = {
        "W or ↑ :往上\nA  or ←:往左\nS  or ↓ :往下\nD  or →:往右",
        "Hp : 生命\nAtk : 攻擊\nDef : 防禦\nSpd : 速度",
        "戰鬥傷害:ATK-DEF\n再爛傷害都不會\n低於1",
        "速度(SPD)較快者先攻",
        "寶箱有時\n會得到能力\n有時啥都沒有\n有時...",
        "R鍵可以回城\n不過地圖也會重置",
        "對手名子爛\n不代表它爛",
        "你還期待有\n特別的提示?\n當然沒有",
        "王不是\n第一次就能贏\n請善用回城(R)",
        "如果覺得目前\n打不贏王\n請考慮回城(R)",
        "勝敗乃兵家常事\n但你不是兵",
        "是不是都在\n看提示?你真閒",
        "難度太高?\n可能是你\n運氣不好?",
        "沒有風險\n沒有收穫",
        "怕死?回城\n打不贏王?回城",
        "開場就掛?\n你也太衰...",
        "戰鬥是全自動\n只要等贏(死)\n就好",
        "有沒有覺得\n自動存檔是好功能?\n沒錯!這裡沒有"
    };

    void Update()
    {
        if (Input.anyKeyDown)
        {
            n = Random.Range(0,text.Length);
            while(n == num)
                n = Random.Range(0, text.Length);
            SetText();
        }
    }

    void SetText()
    {
        num = n;
        gameObject.GetComponent<Text>().text = text[n];
    }
}

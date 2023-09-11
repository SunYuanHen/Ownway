using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = 0;//用於判斷是否重複提示
    string[] text = {
        "W or ↑ :往上\nA  or ←:往左\nS  or ↓ :往下\nD  or →:往右",
        "Hp : 生命\nAtk : 攻擊\nDef : 防禦\nSpd : 速度",
        "戰鬥傷害:ATK-DEF\n傷害不會低於1",
        "速度(SPD)較快者先攻",
        "寶箱有時\n會得到能力\n有時啥都沒有\n有時...",
        "R鍵可以回城\n不過地圖也會重置",
        "如果覺得目前\n打不贏王\n試試回城(R)"
    };

    void Update()
    {
        if (Input.anyKeyDown)
        {
            n = Random.Range(0,text.Length);
            while(n == num) n = Random.Range(0, text.Length);
            SetText();
        }
    }

    void SetText()
    {
        num = n;
        gameObject.GetComponent<Text>().text = text[n];
    }
}

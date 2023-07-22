using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = -1;//用於判斷是否重複提示
    string[] text = {
        "W or ↑ :往上\nA  or ←:往左\nS  or ↓ :往下\nD  or →:往右",
        "Hp : 生命\nAtk : 攻擊\nDef : 防禦\nSpd : 速度",
        "戰鬥造成傷害都是\nAtk - Def\n放心你再爛傷害都\n不會低於1",
        "戰鬥中速度較快者會先打中對方",
        "寶箱有時\n會得到能力\n有時啥都沒有\n有時...",
        "R鍵可以存檔\n不過地圖也會重置",
        "別以為對方名子\n看起來很爛\n它就很爛\n你可能會死",
        "你還期待有甚麼\n特別的提示?",
        "別想一次存檔都\n沒有就直接打王\n你會死",
        "如果覺得目前\n你打不贏王\n那為何不回城(R)?",
        "勝敗乃兵家常事\n但你不是士兵\n死了就沒了",
        "你是不是在亂按\n看提示?是的話\n你真閒",
        "覺得難度太高嗎?\n有沒有想過\n是你運氣不好?",
        "沒有風險\n沒有收穫\n兄弟",
        "怕死?回城\n打不贏王?回城\n回城就是如此萬能",
        "開場葛屁?\n你運氣也太糟...",
        "戰鬥是全程自動\n只要等結果就好",
        "有沒有覺得\n自動存檔\n是神一般的功能\n沒錯!\n這裡沒有"
    };
    void Start()
    {
        n = Random.Range(0, text.Length);
        SetText();
    }
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

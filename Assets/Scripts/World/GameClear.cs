using UnityEngine;
using UnityEngine.UI;
//At World Scene
public class GameClear : MonoBehaviour
{
    public Text hpText, atkText, defText, spdText, evaluateText;
    public GameObject playerStat;
    public Text[] texts;
    public Text finalScore;
    int score = 0;
    private void Update()
    {
        hpText.text = "HP\t " + playerStat.GetComponent<PlayerStat>().GetStats(1).ToString();
        atkText.text = "ATK\t " + playerStat.GetComponent<PlayerStat>().GetStats(2).ToString();
        defText.text = "DEF\t " + playerStat.GetComponent<PlayerStat>().GetStats(3).ToString();
        spdText.text = "SPD\t " + playerStat.GetComponent<PlayerStat>().GetStats(4).ToString();
    }
    public void ClearGame()
    {
        for (int i = 1; i < 5; i++)
        {
            int num = playerStat.GetComponent<PlayerStat>().GetStats(i);
            if (i==1) score += num;
            else score += num*2;
            texts[i - 1].text = i - 1 == 0 ? num.ToString() : (num * 2).ToString();
        }
        finalScore.text = score.ToString();
        if (score > 10000) 
        { 
            finalScore.color = atkText.color;
            evaluateText.color = atkText.color;
            evaluateText.text = "GOD LIKE!!!";
        }
        else if (score > 5000)
        {
            finalScore.color = defText.color;
            evaluateText.color = defText.color;
            evaluateText.fontSize = 180;
            evaluateText.text = "You're Pretty Good.";
        }
        else if (score > 1000)
        {
            finalScore.color = spdText.color;
            evaluateText.color = spdText.color;
            evaluateText.text = "Great Job!";
        }
        else evaluateText.text = "Nice!";
    }
}

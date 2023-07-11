using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Text hpText, atkText, defText, spdText;
    public GameObject playerStat;
    public Text[] texts;
    public Text finalScore;
    int score = 0;
    private void Update()
    {
        hpText.text = "HP\t " + playerStat.GetComponent<PlayerStat>().GetStats(0).ToString();
        atkText.text = "ATK\t " + playerStat.GetComponent<PlayerStat>().GetStats(1).ToString();
        defText.text = "DEF\t " + playerStat.GetComponent<PlayerStat>().GetStats(2).ToString();
        spdText.text = "SPD\t " + playerStat.GetComponent<PlayerStat>().GetStats(3).ToString();
    }
    public void ClearGame()
    {
        for (int i = 1; i < 5; i++)
        {
            int num = playerStat.GetComponent<PlayerStat>().GetStats(i);
            if (i==1) score += num;
            else score += num*2;
            texts[i-1].text = num.ToString();
        }
        finalScore.text = score.ToString();
    }
}

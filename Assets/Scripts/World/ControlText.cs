using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = 0;//�Ω�P�_�O�_���ƴ���
    string[] text = {
        "W or �� :���W\nA  or ��:����\nS  or �� :���U\nD  or ��:���k",
        "Hp : �ͩR\nAtk : ����\nDef : ���m\nSpd : �t��",
        "�԰��ˮ`:ATK-DEF\n�ˮ`���|�C��1",
        "�t��(SPD)���֪̥���",
        "�_�c����\n�|�o���O\n����ԣ���S��\n����...",
        "R��i�H�^��\n���L�a�Ϥ]�|���m",
        "�p�Gı�o�ثe\n����Ĺ��\n�ոզ^��(R)"
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

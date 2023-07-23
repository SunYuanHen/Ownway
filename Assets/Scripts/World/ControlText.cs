using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = 0;//�Ω�P�_�O�_���ƴ���
    string[] text = {
        "W or �� :���W\nA  or ��:����\nS  or �� :���U\nD  or ��:���k",
        "Hp : �ͩR\nAtk : ����\nDef : ���m\nSpd : �t��",
        "�԰��ˮ`:ATK-DEF\n�A��ˮ`�����|\n�C��1",
        "�t��(SPD)���֪̥���",
        "�_�c����\n�|�o���O\n����ԣ���S��\n����...",
        "R��i�H�^��\n���L�a�Ϥ]�|���m",
        "���W�l��\n���N����",
        "�A�ٴ��ݦ�\n�S�O������?\n��M�S��",
        "�����O\n�Ĥ@���N��Ĺ\n�е��Φ^��(R)",
        "�p�Gı�o�ثe\n����Ĺ��\n�ЦҼ{�^��(R)",
        "�ӱѤD�L�a�`��\n���A���O�L",
        "�O���O���b\n�ݴ���?�A�u��",
        "���פӰ�?\n�i��O�A\n�B�𤣦n?",
        "�S�����I\n�S����ì",
        "�Ȧ�?�^��\n����Ĺ��?�^��",
        "�}���N��?\n�A�]�ӰI...",
        "�԰��O���۰�\n�u�n��Ĺ(��)\n�N�n",
        "���S��ı�o\n�۰ʦs�ɬO�n�\��?\n�S��!�o�̨S��"
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

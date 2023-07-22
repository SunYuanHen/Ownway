using UnityEngine;
using UnityEngine.UI;

public class ControlText : MonoBehaviour
{
    int n,num = -1;//�Ω�P�_�O�_���ƴ���
    string[] text = {
        "W or �� :���W\nA  or ��:����\nS  or �� :���U\nD  or ��:���k",
        "Hp : �ͩR\nAtk : ����\nDef : ���m\nSpd : �t��",
        "�԰��y���ˮ`���O\nAtk - Def\n��ߧA�A��ˮ`��\n���|�C��1",
        "�԰����t�׸��̷֪|���������",
        "�_�c����\n�|�o���O\n����ԣ���S��\n����...",
        "R��i�H�s��\n���L�a�Ϥ]�|���m",
        "�O�H�����W�l\n�ݰ_�ӫ���\n���N����\n�A�i��|��",
        "�A�ٴ��ݦ��ƻ�\n�S�O������?",
        "�O�Q�@���s�ɳ�\n�S���N��������\n�A�|��",
        "�p�Gı�o�ثe\n�A����Ĺ��\n�����󤣦^��(R)?",
        "�ӱѤD�L�a�`��\n���A���O�h�L\n���F�N�S�F",
        "�A�O���O�b�ë�\n�ݴ���?�O����\n�A�u��",
        "ı�o���פӰ���?\n���S���Q�L\n�O�A�B�𤣦n?",
        "�S�����I\n�S����ì\n�S��",
        "�Ȧ�?�^��\n����Ĺ��?�^��\n�^���N�O�p���U��",
        "�}������?\n�A�B��]���V...",
        "�԰��O���{�۰�\n�u�n�����G�N�n",
        "���S��ı�o\n�۰ʦs��\n�O���@�몺�\��\n�S��!\n�o�̨S��"
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        //�������O�o���ѱ�
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}

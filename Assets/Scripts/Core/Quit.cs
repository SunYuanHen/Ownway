using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        //正式版記得註解掉
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}

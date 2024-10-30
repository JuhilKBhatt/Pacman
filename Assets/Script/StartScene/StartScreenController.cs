using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level-1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level-2");
    }
}
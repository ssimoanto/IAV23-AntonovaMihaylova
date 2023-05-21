using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void SimonaAntonovaMihaylova()
    {
        SceneManager.LoadScene("SImonaAntonova");
    }
    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}

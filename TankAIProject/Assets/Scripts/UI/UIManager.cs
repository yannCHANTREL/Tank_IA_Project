using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public GameObject m_TitleScreen;
    public GameObject m_OptionScreen;
    [Space(10)] 
    public string m_GameSceneName; 

    private void Start()
    {
        GoToTitleScreen();
    }

    public void GoToTitleScreen()
    {
        m_TitleScreen.SetActive(true);
        m_OptionScreen.SetActive(false);
    }
    
    public void GoToOptionScreen()
    {
        m_TitleScreen.SetActive(false);
        m_OptionScreen.SetActive(true);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(m_GameSceneName);
    }
}

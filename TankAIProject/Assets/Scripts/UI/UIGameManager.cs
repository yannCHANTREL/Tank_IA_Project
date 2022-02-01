using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameManager : MonoBehaviour
{
    public string m_GameSceneName;
    public string m_UISceneName;

    public GameObject m_UIEndGameMenu;

    private void Start()
    {
        m_UIEndGameMenu.SetActive(false);
    }

    public void DisplayEndMenu()
    {
        m_UIEndGameMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(m_GameSceneName);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(m_UISceneName);
    }
}

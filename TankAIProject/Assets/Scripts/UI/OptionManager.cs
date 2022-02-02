using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameOptions m_GameOptions;
    [Space(10)] 
    public Dropdown m_DropdownAlgo;
    public Dropdown m_DropdownDifficulty;
    public Dropdown m_DropdownMode;
    [Space(10)] 
    public string m_GameSceneName;

    private void Start()
    {
        // Init dropdown options
        m_DropdownAlgo.ClearOptions();
        m_DropdownDifficulty.ClearOptions();
        m_DropdownMode.ClearOptions();
        
        m_DropdownAlgo.AddOptions(GetEnumString(typeof(GameOptions.AISearchAlgo)));
        m_DropdownDifficulty.AddOptions(GetEnumString(typeof(GameOptions.AIDifficulty)));
        m_DropdownMode.AddOptions(GetEnumString(typeof(GameOptions.Mode)));

        m_DropdownAlgo.value = (int) m_GameOptions.m_SearchAlgo;
        m_DropdownDifficulty.value = (int) m_GameOptions.m_AIDifficulty;
        m_DropdownMode.value = (int) m_GameOptions.m_Mode;
    }

    private List<string> GetEnumString(Type type)
    {
        return Enum.GetNames(type).ToList();
    }

    public void ChangeAlgoOption(int index)
    {
        m_GameOptions.m_SearchAlgo = (GameOptions.AISearchAlgo)index;
    }

    public void ChangeDifficultyOption(int index)
    {
        m_GameOptions.m_AIDifficulty = (GameOptions.AIDifficulty)index;
    }

    public void ChangeModeOption(int index)
    {
        m_GameOptions.m_Mode = (GameOptions.Mode)index;
    }
}

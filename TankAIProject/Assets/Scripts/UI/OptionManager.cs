using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public Dropdown m_DropdownBehaviorTree;
    public Dropdown m_DropdownMode;
    [Space(10)] 
    public string m_GameSceneName;
    [Space(10)] 
    public Text m_SliderText;
    public Slider m_Slider;

    private void Start()
    {
        // Init dropdown options
        m_DropdownAlgo.ClearOptions();
        m_DropdownDifficulty.ClearOptions();
        m_DropdownBehaviorTree.ClearOptions();
        m_DropdownMode.ClearOptions();

        m_DropdownAlgo.AddOptions(GetEnumString(typeof(GameOptions.AISearchAlgo)));
        m_DropdownDifficulty.AddOptions(GetEnumString(typeof(GameOptions.AIDifficulty)));
        m_DropdownBehaviorTree.AddOptions(GetEnumString(typeof(GameOptions.BehaviorTreeEnum)));
        List<string> modeString = GetEnumString(typeof(GameOptions.Mode));
        m_DropdownMode.AddOptions(ReformateModeText(modeString));
        
        m_DropdownAlgo.value = (int) m_GameOptions.m_SearchAlgo;
        m_DropdownDifficulty.value = (int) m_GameOptions.m_AIDifficulty;
        m_DropdownBehaviorTree.value = (int) m_GameOptions.m_BehaviorTree;
        m_DropdownMode.value = (int) m_GameOptions.m_Mode;
        
        // Update Slider Text
        ChangeSliderText(m_Slider.value);
    }

    private List<string> GetEnumString(Type type)
    {
        return Enum.GetNames(type).ToList();
    }

    private List<string> ReformateModeText(List<string> list)
    {
        List<string> returnList = new List<string>();
        foreach (string s in list)
        {
            int indexVS = s.IndexOf("VS", StringComparison.Ordinal);
            string firstText = s.Substring(0, indexVS);
            string lastText = s.Substring(indexVS + 2, s.Length - indexVS - 2);
            
            returnList.Add(firstText + " VS " + lastText);
        }

        return returnList;
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

    public void ChangeBehaviorTree(int index)
    {
        m_GameOptions.m_BehaviorTree = (GameOptions.BehaviorTreeEnum) index;
    }

    public void ChangeSliderText(float value)
    {
        m_SliderText.text = value.ToString();
        m_GameOptions.m_NbPlayer = (int)value;
    }
}

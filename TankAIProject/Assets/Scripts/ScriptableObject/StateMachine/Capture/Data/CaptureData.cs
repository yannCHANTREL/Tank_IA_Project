using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Complete;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "State Machine/Data/Capture")]
public class CaptureData : DataBase
{
    public Dictionary<int, int> m_PlayerNumbersPerTeam;
    public TeamList m_TeamList;
    
    [HideInInspector] public int m_OldTeamCapturing = 0;
    [HideInInspector] public int m_CurrentTeamCapturing = 0;
    public float m_FullCaptureDuration;
    public float m_AlphaColor;

    [HideInInspector] public bool m_TriggerEntered;
    [HideInInspector] public bool m_TriggerExit;
    [HideInInspector] public float m_Value = 0;

    [HideInInspector] public bool m_IsGameFinished;
    
    [HideInInspector] public Slider m_Slider;
    [HideInInspector] public Image m_FillImage;

    private System.Action m_UpdateScoreText;

    public void Init(Slider slider, Image image, System.Action updateScoreAction)
    {
        m_Slider = slider;
        m_FillImage = image;
        m_UpdateScoreText = updateScoreAction;
        
        m_PlayerNumbersPerTeam = new Dictionary<int, int>();
        
        SetCaptureDefaultValue();
    }
    
    public void ChangeFillColor()
    {
        m_FillImage.color = ChangeAlpha(m_TeamList.GetColorTeam(m_CurrentTeamCapturing - 1));
    }
    public void UpdateSlider()
    {
        m_Slider.value = m_Value;
    }

    public void UpdateScoreText()
    {
        m_UpdateScoreText.Invoke();
    }

    public void TriggerEnter(Collider other)
    {
        m_TriggerEntered = true; 
        
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank
        
        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] += 1;
        
        UpdateCurrentTeamCapturing();
    }

    public void TriggerExit(Collider other)
    {
        m_TriggerExit = true;
        
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank

        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] -= 1;

        UpdateCurrentTeamCapturing();
    }
    
    public int GetTeamCapturingNumber()
    {
        int nbTeamOnCapturePoint = 0;
        int teamNumberOnCapture = 0;

        foreach (KeyValuePair<int, int> keyValuePair in m_PlayerNumbersPerTeam)
        {
            if (keyValuePair.Value != 0)
            {
                nbTeamOnCapturePoint += 1;
                teamNumberOnCapture = keyValuePair.Key;
            }
        }

        return nbTeamOnCapturePoint == 1 ? teamNumberOnCapture : 0;
    }

    public int GetNumberTeamOnPoint()
    {        
        int nbTeamOnCapturePoint = 0;

        foreach (KeyValuePair<int, int> keyValuePair in m_PlayerNumbersPerTeam)
        {
            if (keyValuePair.Value != 0)
            {
                nbTeamOnCapturePoint += 1;
            }
        }

        return nbTeamOnCapturePoint;

    }
    
    private void InitDictionnary()
    {
        m_PlayerNumbersPerTeam.Clear();
        for (int i = 1; i <= m_TeamList.GetNumberTeam(); i++)
        {
            m_PlayerNumbersPerTeam.Add(i, 0);
        }
    }
    
    private Color ChangeAlpha(Color color)
    {
        color.a = m_AlphaColor;
        return color;
    }
    
    private void SetCaptureDefaultValue()
    {
        m_TriggerEntered = false;
        m_IsGameFinished = false;
        m_TriggerExit = false;
        
        m_Value = 0;
        
        m_OldTeamCapturing = 0;
        m_CurrentTeamCapturing = 0;
        
        InitDictionnary();
        UpdateScoreText();
        
        UpdateSlider();
    }

    public void UpdateDictionnary(Dictionary<int, int> dictionary)
    {
        m_PlayerNumbersPerTeam = dictionary;
        UpdateCurrentTeamCapturing();
    }
    
    private void UpdateCurrentTeamCapturing()
    {
        m_CurrentTeamCapturing = GetTeamCapturingNumber();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointManager : MonoBehaviour
{
    public Slider m_Slider;
    public Image m_FillImage;
    public float m_AlphaColor;
    [Space(10)]
    public float m_FullCaptureDuration;
    public int m_ScoreIncrement;
    [Space(10)] 
    public TeamList m_TeamList;
    public float m_IntervalScoreSeconds;
    
    private float m_Value = 0;
    private int m_OldTeamCapturing = 0;
    private int m_CurrentTeamCapturing = 0;

    private bool m_DidOnce = false;

    private float m_SavedTime;

    private Dictionary<int, int> m_PlayerNumbersPerTeam;

    private void Start()
    {
        m_PlayerNumbersPerTeam = new Dictionary<int, int>();
        InitDictionnary();
    }

    private void OnTriggerEnter(Collider other)
    { 
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank

        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] += 1;
        
        m_CurrentTeamCapturing = GetTeamCapturing();
        
        if (!m_DidOnce)
        {
            m_OldTeamCapturing = m_CurrentTeamCapturing;
            m_FillImage.color = ChangeAlpha(m_TeamList.GetColorTeam(m_CurrentTeamCapturing - 1));
            m_DidOnce = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank

        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] -= 1;

        m_CurrentTeamCapturing = GetTeamCapturing();
    }
    
    private void OnTriggerStay(Collider other)
    {
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank

        if (m_CurrentTeamCapturing == 0) return; // No team is capturing
        
        if (m_OldTeamCapturing == m_CurrentTeamCapturing)
        {
            m_Value += (Time.deltaTime * 100) / m_FullCaptureDuration;
        }
        else
        {
            m_Value -= (Time.deltaTime * 100) / (m_FullCaptureDuration / 2);
        }
            
        m_Slider.value = m_Value;
            
        if (m_Value >= 100)
        {
            m_Value = 100;
            
            m_SavedTime += Time.deltaTime;
            if (m_SavedTime >= m_IntervalScoreSeconds)
            {
                m_TeamList.IncrementCaptureScore(m_CurrentTeamCapturing - 1, m_ScoreIncrement);
                m_SavedTime = 0;
            }
            
            return;
        }
        
        if (m_Value < 0)
        {
            m_Value = 0;
            m_OldTeamCapturing = m_CurrentTeamCapturing;
            m_FillImage.color = ChangeAlpha(m_TeamList.GetColorTeam(m_CurrentTeamCapturing - 1));
        }
    }

    private int GetTeamCapturing()
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
    
    private Color ChangeAlpha(Color color)
    {
        color.a = m_AlphaColor;
        return color;
    }

    public void ResetCapture()
    {
        m_Value = 0;
        m_OldTeamCapturing = 0;
        m_CurrentTeamCapturing = 0;
        m_DidOnce = false;
        InitDictionnary();
        
        m_Slider.value = m_Value;
    }

    private void InitDictionnary()
    {
        m_PlayerNumbersPerTeam.Clear();
        for (int i = 1; i <= m_TeamList.GetNumberTeam(); i++)
        {
            m_PlayerNumbersPerTeam.Add(i, 0);
        }
    }
}

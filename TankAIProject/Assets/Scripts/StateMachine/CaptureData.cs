using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Complete;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "State Machine/Data/Capture")]
public class CaptureData : StateDataBase
{
    // TODO : Bug if the tank is destroyed if in while in the zone, continue to capture the zone (not good)
    
    public Dictionary<int, int> m_PlayerNumbersPerTeam;
    public TeamList m_TeamList;
    
    [HideInInspector] public int m_OldTeamCapturing = 0;
    [HideInInspector] public int m_CurrentTeamCapturing = 0;
    public float m_FullCaptureDuration;
    public float m_AlphaColor;

    [HideInInspector] public bool m_TriggerEntered;
    [HideInInspector] public bool m_TriggerExit;
    [HideInInspector] public float m_Value = 0;

    [HideInInspector] public bool m_IsRoundFinished;
    [HideInInspector] public bool m_IsRoundStarting;
    
    [HideInInspector] public Slider m_Slider;
    [HideInInspector] public Image m_FillImage;

    public void ReferenceSliderAndImage(Slider slider, Image image)
    {
        m_Slider = slider;
        m_FillImage = image;
    }
    
    public void ChangeFillColor()
    {
        m_FillImage.color = ChangeAlpha(m_TeamList.GetColorTeam(m_CurrentTeamCapturing - 1));
    }
    public void UpdateSlider()
    {
        m_Slider.value = m_Value;
    }

    public void TriggerEnter(Collider other)
    {
        m_TriggerEntered = true; 
        
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank
        
        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] += 1;
        
        m_CurrentTeamCapturing = GetTeamCapturing();
    }

    public void TriggerExit(Collider other)
    {
        m_TriggerExit = true;
        
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        if (tankMovement == null) return; // This is not a tank

        int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
        m_PlayerNumbersPerTeam[teamNumber] -= 1;

        m_CurrentTeamCapturing = GetTeamCapturing();
    }
    
    public int GetTeamCapturing()
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
    
    public void InitDictionnary()
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
    
    public void ResetCapture()
    {
        m_TriggerEntered = false;
        m_IsRoundFinished = false;
        m_IsRoundStarting = false;
        m_TriggerExit = false;
        
        m_Value = 0;
        
        m_OldTeamCapturing = 0;
        m_CurrentTeamCapturing = 0;
        
        InitDictionnary();
        
        m_Slider.value = m_Value;
    }
}

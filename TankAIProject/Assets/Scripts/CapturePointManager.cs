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
    [Space(10)]
    public float m_FullCaptureDuration;
    public int m_ScoreIncrement;
    
    private float m_Value = 0;
    private int m_OldTeamCapturing = 0;
    private int m_CurrentTeamCapturing = 0;

    private bool m_DidOnce = true;
    private void OnTriggerEnter(Collider other)
    {
        // TODO : Redo that for team and not only player
        
        TankMovement tankMovement = other.GetComponent<TankMovement>();
        m_CurrentTeamCapturing = tankMovement.m_PlayerNumber;

        if (m_DidOnce)
        {
            // m_FillImage.color = m_ColorsPerTeam[m_CurrentTeamCapturing - 1];
            m_DidOnce = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // TODO : Redo that for team and not only player
        
        if (m_OldTeamCapturing == m_CurrentTeamCapturing)
        {
            // m_Value += m_IncrementSpeed * Time.deltaTime;
        }
        else
        {
            // m_Value -= m_IncrementSpeed * Time.deltaTime;
        }
        
        m_Slider.value = m_Value;
        
        if (m_Value >= 100)
        {
            m_Value = 100;
            // TODO : Increment score for the team that captured the point
            return;
        }
        
        if (m_Value < 0)
        {
            m_OldTeamCapturing = m_CurrentTeamCapturing;
            // m_FillImage.color = m_ColorsPerTeam[m_CurrentTeamCapturing - 1];
        }
    }
}

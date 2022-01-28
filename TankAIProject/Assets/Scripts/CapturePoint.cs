using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public CaptureData m_CaptureData;
    
    public Slider m_Slider;
    public Image m_FillImage;
    public Text m_ScoreText;

    public TeamList m_TeamList;
    
    private bool m_DidOnce = false;

    private void Start()
    {
        m_CaptureData.Init(m_Slider, m_FillImage, UpdateScoreText);

        m_CaptureData.m_PlayerNumbersPerTeam = new Dictionary<int, int>();
        m_CaptureData.InitDictionnary();
        
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        string scoreText = string.Empty;
        foreach (Team team in m_TeamList.m_Teams)
        {
            scoreText += team.GetColoredRoundScoreText() + "\n";
        }
        m_ScoreText.text = scoreText;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tank")) return;

        m_CaptureData.TriggerEnter(other);

        if (m_DidOnce) return;
        
        m_CaptureData.m_OldTeamCapturing = m_CaptureData.m_CurrentTeamCapturing;
        m_CaptureData.ChangeFillColor();
        m_DidOnce = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Tank")) return;
        
        m_CaptureData.TriggerExit(other);
    }
}

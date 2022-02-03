using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class CapturePoint : MonoBehaviour
{
    public CaptureData m_CaptureData;
    
    public Slider m_Slider;
    public Image m_FillImage;
    public Text m_ScoreText;

    public TeamList m_TeamList;
    
    private bool m_DidOnce = false;
    private float m_ColliderRadius;

    private float m_SecureRadius = 2;

    private void Awake()
    {
        m_ColliderRadius = GetComponent<SphereCollider>().radius;
        
        m_CaptureData.Init(m_Slider, m_FillImage, UpdateScoreText);
        
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

    public void UpdateCurrentTeamCapturing(GameObject go)
    {
        // Check if the position is not far
        if (Vector3.Distance(transform.position, go.transform.position) > m_ColliderRadius + m_SecureRadius) return;
        
        Dictionary<int, int> playerNumbersPerTeam = new Dictionary<int, int>();
        playerNumbersPerTeam.Clear();
        for (int i = 1; i <= m_TeamList.GetNumberTeam(); i++)
        {
            playerNumbersPerTeam.Add(i, 0);
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ColliderRadius);
        foreach (Collider c in colliders)
        {
            if (!c.CompareTag("Tank")) continue;
            
            TankMovement tankMovement = c.GetComponent<TankMovement>();
        
            int teamNumber = m_TeamList.GetTeamNumberByPlayerNumber(tankMovement.m_PlayerNumber);
            playerNumbersPerTeam[teamNumber] += 1;
        }
        
        m_CaptureData.UpdateDictionnary(playerNumbersPerTeam);
    }
}

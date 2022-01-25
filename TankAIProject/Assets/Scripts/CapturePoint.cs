using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public CaptureData m_CaptureData;
    
    private bool m_DidOnce = false;
    
    public Slider m_Slider;
    public Image m_FillImage;
    
    private void Start()
    {
        m_CaptureData.ReferenceSliderAndImage(m_Slider, m_FillImage);
        
        m_CaptureData.m_PlayerNumbersPerTeam = new Dictionary<int, int>();
        m_CaptureData.InitDictionnary();
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

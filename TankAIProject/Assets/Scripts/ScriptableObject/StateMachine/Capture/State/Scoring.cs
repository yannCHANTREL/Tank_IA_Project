using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Capture/Scoring")]
public class Scoring : StateBase
{
    public float m_ScoreIncrement;
    public float m_IntervalScoreSeconds;

    private float m_SavedTime;

    public override void OnEnter(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
        data.m_Value = 100;
    }

    public override void OnExit(StateMachineManager stateMachineManager)
    { }

    protected override void Execute(StateMachineManager stateMachineManager)
    {
        CaptureData data = (CaptureData)stateMachineManager.m_Data;
 
        m_SavedTime += Time.deltaTime;
        if (m_SavedTime >= m_IntervalScoreSeconds)
        {
            if (data.m_PlayerNumbersPerTeam.TryGetValue(data.m_CurrentTeamCapturing, out int numberPlayer))
            {
                float scoreIncrement = m_ScoreIncrement * numberPlayer;
                data.m_TeamList.IncrementCaptureScore(data.m_CurrentTeamCapturing - 1, scoreIncrement);
                data.UpdateScoreText();
                m_SavedTime = 0;
            }
        }
    }
}

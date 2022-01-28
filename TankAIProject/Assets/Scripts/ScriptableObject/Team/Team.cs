using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Team")]
public class Team : ScriptableObject
{
    public int m_TeamNumber;
    public Color m_TeamColor;
    public bool m_AI;

    public int m_RoundScore;
    public int m_CaptureScore;
    
    public List<TankManager> m_TeamTank;

    public bool HasCaptureScore(int value)
    {
        return m_CaptureScore == value;
    }
    
    public void IncrementCaptureScore(int value)
    {
        m_CaptureScore += value;
    }
    public string GetColoredTeamText()
    {
        return GetColoredText("TEAM " + m_TeamNumber);
    }

    public string GetColoredRoundScoreText()
    {
        return GetColoredText(m_CaptureScore.ToString());
    }

    private string GetColoredText(string text)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(m_TeamColor) + ">" + text + "</color>";
    }

    public void AddTank(TankManager tank)
    {
        m_TeamTank.Add(tank);
    }

    public Transform[] GetTankTransforms()
    {
        List<Transform> transforms = new List<Transform>();
        foreach (TankManager tank in m_TeamTank)
        {
            transforms.Add(tank.m_Instance.transform);
        }

        return transforms.ToArray();
    }

    public void ResetTeamTanks()
    {
        foreach (TankManager tank in m_TeamTank)
        {
            tank.Reset();
        }
    }
    
    public void EnableTeamControl()
    {
        foreach (TankManager tank in m_TeamTank)
        {
            tank.EnableControl();
        }
    }
    
    public void DisableTeamControl()
    {
        foreach (TankManager tank in m_TeamTank)
        {
            tank.DisableControl();
        }
    }

    public bool IsTeamAlive()
    {
        int numTankLeft = 0;
        foreach (TankManager tankManager in m_TeamTank)
        {
            if (tankManager.m_Instance.activeSelf)
                numTankLeft += 1;
        }

        return numTankLeft >= 1;
    }

    public List<int> GetPlayersNumber()
    {
        List<int> playerNumbers = new List<int>();

        foreach (TankManager tank in m_TeamTank)
        {
            playerNumbers.Add(tank.m_PlayerNumber);
        }
        
        return playerNumbers;
    }
    
}

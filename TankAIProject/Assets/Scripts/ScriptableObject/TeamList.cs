using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Team List")]
public class TeamList : ScriptableObject
{
    public Team[] m_Teams;

    public int GetNumberTeam()
    {
        return m_Teams.Length;
    }

    public Color GetColorTeam(int index)
    {
        return m_Teams[index].m_TeamColor;
    }

    public bool IsAI(int index)
    {
        return m_Teams[index].m_AI;
    }
    
    public void ResetTeamScore()
    {
        foreach (Team team in m_Teams)
        {
            team.m_RoundScore = 0;
            team.m_CaptureScore = 0;
        }
    }
    public void GiveTeamNumber()
    {
        for (int i = 0; i < m_Teams.Length; i++)
        {
            m_Teams[i].m_TeamNumber = i + 1;
        }
    }
    
    public void EmptyTeamList()
    {
        foreach (Team team in m_Teams)
        {
            team.m_TeamTank.Clear();
        }
    }
    
    public void AddTankToTeam(TankManager tank, int teamNumber)
    {
        m_Teams[teamNumber].AddTank(tank);
    }

    public Transform[] GetAllTanksTransform()
    {
        List<Transform> transforms = new List<Transform>();

        foreach (Team team in m_Teams)
        {
            transforms.AddRange(team.GetTankTransforms());
        }
        
        return transforms.ToArray();
    }

    public void ResetAllTank()
    {
        foreach (Team team in m_Teams)
        {
            team.ResetTeamTanks();
        }
    }
    
    public void EnableAllTankControl()
    {
        foreach (Team team in m_Teams)
        {
            team.EnableTeamControl();
        }
    }
    
    public void DisableAllTankControl()
    {
        foreach (Team team in m_Teams)
        {
            team.DisableTeamControl();
        }
    }

    public bool OneTeamLeft()
    {
        int numTeamLeft = 0;

        foreach (Team team in m_Teams)
        {
            if (team.IsTeamAlive())
                numTeamLeft += 1;
        }
        
        return numTeamLeft <= 1;
    }

    public Team GetAliveTeam()
    {
        foreach (Team team in m_Teams)
        {
            if (team.IsTeamAlive())
                return team;
        }

        return null;
    }

    public Team GetGameWinner(int numRoundToWin)
    {
        foreach (Team team in m_Teams)
        {
            if (team.m_RoundScore == numRoundToWin)
                return team;
        }

        return null;
    }

    public string GetScores()
    {
        string text = "";

        foreach (Team team in m_Teams)
        {
            text += $"{team.GetColoredTeamText()} : {team.m_RoundScore} WINS\n";
        }

        return text;
    }
}

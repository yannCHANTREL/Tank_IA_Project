using System;
using System.Collections;
using System.Collections.Generic;
using Complete;
using UnityEngine;

[CreateAssetMenu(menuName = "Team List")]
public class TeamList : ScriptableObject
{
    public Team[] m_Teams;
    [HideInInspector] public Transform[] m_TeamsSpawn;

    public void SetTeamSpawn(Transform[] spawns)
    {
        m_TeamsSpawn = spawns;
    }
    
    public void SetOtherTeamsAsAI()
    {
        for (int i = 0; i < m_Teams.Length; i++)
        {
            if (i == 0)
            {
                m_Teams[i].SetTeamAsPlayer();
            }
            else
            {
                m_Teams[i].SetTeamAsAI();
            }
        }
    }

    public void SetAllTeamAsAI()
    {
        foreach (Team team in m_Teams)
        {
            team.SetTeamAsAI();
        }
    }

    public void SetAllTeamAsPlayer()
    {
        foreach (Team team in m_Teams)
        {
            team.SetTeamAsPlayer();
        }
    }
    
    public void IncrementCaptureScore(int index, float value)
    {
        m_Teams[index].IncrementCaptureScore(value);
    }
    
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

    public void ResetCaptureScore()
    {
        foreach (Team team in m_Teams)
        {
            team.m_CaptureScore = 0;
        }
    }
    
    public void ResetAllScore()
    {
        foreach (Team team in m_Teams)
        {
            // team.m_RoundScore = 0;
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
    
    public Team GetTeamMaxScore()
    {
        if (AreAllTeamHaveTheSamePoint())
        {
            return null;
        }
        
        float maxScore = -1;
        Team winner = null;
        
        foreach (Team team in m_Teams)
        {
            if (team.m_CaptureScore > maxScore)
            {
                winner = team;
                maxScore = team.m_CaptureScore;
            }
        }
        return winner;
    }

    private bool AreAllTeamHaveTheSamePoint()
    {
        float currentScore = m_Teams[0].m_CaptureScore;
        foreach (Team team in m_Teams)
        {
            if (Math.Abs(team.m_CaptureScore - currentScore) > float.Epsilon)
                return false;
        }

        return true;
    }

    public int GetTeamNumberByPlayerNumber(int playerNumber)
    {
        foreach (Team team in m_Teams)
        {
            if (team.GetPlayersNumber().Contains(playerNumber))
            {
                return team.m_TeamNumber;
            }
        }
        
        return -1;
    }
}

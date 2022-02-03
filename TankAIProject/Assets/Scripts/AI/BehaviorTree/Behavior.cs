using UnityEngine;

public abstract class Behavior : ScriptableObject
{
    public enum Status {invalid, running, success, failure};

    public Status m_Status = Status.invalid;

    public abstract void OnInitialize();
    public abstract Status BHUpdate(bool debugMode, int teamIndex, int tankIndex = -1);
    public abstract void OnTerminate(Status status);
    public abstract void AddAITank(int teamIndex, int tankIndex = -1);
    public abstract void RemoveAITank(int teamIndex, int tankIndex = -1);

    public Status Tick(bool debugMode, int teamIndex, int tankIndex = -1)
    {
        if (debugMode) DebugLogIn(teamIndex, tankIndex);

        if (m_Status != Status.running) OnInitialize();
        m_Status = BHUpdate(debugMode, teamIndex, tankIndex);
        if (m_Status != Status.running) OnTerminate(m_Status);

        if (debugMode) DebugLogOut(teamIndex, tankIndex, m_Status);
        return m_Status;
    }

    public void DebugLogIn(int teamIndex, int tankIndex)
    {
        if (tankIndex == -1) { Debug.Log("Team n°" + teamIndex + "  IN: " + name); }
        else { Debug.Log("Tank n°" + tankIndex + "  IN: " + name); }
    }
    public void DebugLogOut(int teamIndex, int tankIndex, Status m_Status)
    {
        if (tankIndex == -1) { Debug.Log("Team n°" + teamIndex + " OUT : " + m_Status + ", " + name); }
        else { Debug.Log("Tank n°" + tankIndex + " OUT : " + m_Status + ", " + name); }
    }
}

using UnityEngine;

public abstract class Behavior : ScriptableObject
{
    public enum Status {invalid, running, success, failure};

    public Status m_Status = Status.invalid;

    public abstract void OnInitialize();
    public abstract Status BHUpdate(int teamIndex, int tankIndex = 0);
    public abstract void OnTerminate(Status status);
    public abstract void AddAITank(int teamIndex, int tankIndex = 0);
    public abstract void RemoveAITank(int teamIndex, int tankIndex = 0);

    public Status Tick(int teamIndex, int tankIndex = 0)
    {
        if (m_Status != Status.running) OnInitialize();
        m_Status = BHUpdate(teamIndex, tankIndex);
        if (m_Status != Status.running) OnTerminate(m_Status);
        return m_Status;
    }
}

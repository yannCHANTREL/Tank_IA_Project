using UnityEngine;

public abstract class Behavior : ScriptableObject
{
    public enum Status {invalid, running, success, failure};

    public Status m_Status = Status.invalid;

    public abstract void OnInitialize();
    public abstract Status BHUpdate(int tankIndex);
    public abstract void OnTerminate(Status status);
    public abstract void AddAITank(int tankIndex);
    public abstract void RemoveAITank(int tankIndex);

    public Status Tick(int tankIndex)
    {
        if (m_Status != Status.running) OnInitialize();
        m_Status = BHUpdate(tankIndex);
        if (m_Status != Status.running) OnTerminate(m_Status);
        return m_Status;
    }
}

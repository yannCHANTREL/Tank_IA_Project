using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public Vector3 m_TargetPos;
    public bool m_MoveToTarget;
    public bool m_MoveToFireRange;
    public bool m_MoveForward = true;
    public float m_FireRange = 10;
    public float m_AnleClamp = 20;
    public float m_RadiusTolerance = 0.5f;
    public float m_AngularTolerancePercentage = 0.1f;

    [SerializeField] private TankIndexManager m_TankIndexManager;
    [SerializeField] private FloatListVariable m_MoveAxis;
    [SerializeField] private FloatListVariable m_TurnAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MoveToFireRange)
        {
            ProceedMovements();
        }
        else
        {
            Stop();
        }
    }

    private void ProceedMovements()
    {
        Vector3 tankPos = transform.position;
        Vector3 tankForward = transform.forward;
        Vector3 tankRight = transform.right;
        Vector3 distance = m_TargetPos - tankPos;
        if (distance.magnitude > m_RadiusTolerance)
        {
            Move(distance, tankForward);
            Turn(distance, tankForward, tankRight);
        }
        else
        {
            Stop();
        }
    }

    private void Move(Vector3 distance, Vector3 tankForward)
    {
        if (!m_TankIndexManager || !m_MoveAxis) return;
        m_MoveAxis.m_Values[m_TankIndexManager.m_TankIndex-1] = Vector3.Dot(distance, tankForward) >= 0 ? 1 : -1;
    }

    private void Turn(Vector3 distance, Vector3 tankForward, Vector3 tankRight)
    {
        if (!m_MoveForward) { distance *= -1; }
        if (!m_TankIndexManager || !m_TurnAxis) return;
        float angleSign = Vector3.Dot(tankRight, distance) >= 0 ? 1 : -1;
        float angleToTarget = Mathf.Clamp(Vector3.Angle(distance, tankForward) * angleSign / m_AnleClamp, -1f, 1f);
        print(angleToTarget);
        if (Mathf.Abs(angleToTarget) > m_AngularTolerancePercentage) { m_TurnAxis.m_Values[m_TankIndexManager.m_TankIndex-1] = angleToTarget; }
    }

    private void Stop()
    {
        m_MoveAxis.m_Values[m_TankIndexManager.m_TankIndex-1] = 0;
        m_TurnAxis.m_Values[m_TankIndexManager.m_TankIndex-1] = 0;
    }
}

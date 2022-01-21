using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public bool m_Move;
    public bool m_MoveToFireRange;
    public bool m_MoveForward = true;
    public float m_FirePlacementRange = 13;
    public float m_AnleClamp = 20;
    public float m_RadiusTolerance = 0.5f;
    public float m_AngularTolerancePercentage = 0.1f;

    [SerializeField] private TankIndexManager m_TankIndexManager;
    [SerializeField] private FloatListVariable m_MoveAxis;
    [SerializeField] private FloatListVariable m_TurnAxis;
    [SerializeField] private Vector3ListVariable m_Target;

    // Update is called once per frame
    void Update()
    {
        if (m_Move)
        {
            ProceedMovements(m_MoveToFireRange ? m_FirePlacementRange : 0f);
        }
        else
        {
            StopMove();
            StopTurn();
        }
    }

    private void ProceedMovements(float targetDistanceToTarget)
    {
        Vector3 tankPos = transform.position;
        Vector3 tankForward = transform.forward;
        Vector3 tankRight = transform.right;
        Vector3 distance = m_Target.m_Values[m_TankIndexManager.m_TankIndex] - tankPos;
        
        if (Mathf.Abs(distance.magnitude - targetDistanceToTarget) > m_RadiusTolerance) { Move(distance, tankForward); }
        else { StopMove(); }
        
        if (distance.magnitude > m_RadiusTolerance) { Turn(distance, tankForward, tankRight); }
        else { StopTurn(); }
    }

    private void Move(Vector3 distance, Vector3 tankForward)
    {
        if (!m_TankIndexManager || !m_MoveAxis) return;
        m_MoveAxis.m_Values[m_TankIndexManager.m_TankIndex] = Vector3.Dot(distance, tankForward) >= 0 != (m_MoveToFireRange && distance.magnitude - m_FirePlacementRange < 0)? 1 : -1;
    }
    
    private void Turn(Vector3 distance, Vector3 tankForward, Vector3 tankRight)
    {
        if (!m_MoveForward) { distance *= -1; }
        if (!m_TankIndexManager || !m_TurnAxis) return;
        float angleSign = Vector3.Dot(tankRight, distance) >= 0 ? 1 : -1;
        float angleToTarget = Mathf.Clamp(Vector3.Angle(distance, tankForward) * angleSign / m_AnleClamp, -1f, 1f);
        if (Mathf.Abs(angleToTarget) > m_AngularTolerancePercentage) { m_TurnAxis.m_Values[m_TankIndexManager.m_TankIndex] = angleToTarget; }
        else { StopTurn(); }
    }

    private void StopMove()
    {
        m_MoveAxis.m_Values[m_TankIndexManager.m_TankIndex] = 0;
    }

    private void StopTurn()
    {
        m_TurnAxis.m_Values[m_TankIndexManager.m_TankIndex] = 0;
    }
}

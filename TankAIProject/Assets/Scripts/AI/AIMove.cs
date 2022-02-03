using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public TankMoveInstructionListVariable m_MoveInstructions;
    public float m_FirePlacementRange = 13;
    public float m_AnleClamp = 20;
    public float m_RadiusTolerance = 0.5f;
    public float m_AngularTolerancePercentage = 0.1f;
    public TankIndexManager m_TankIndexManager;
    public FloatListVariable m_MoveAxis;
    public FloatListVariable m_TurnAxis;
    public GameObjectListVariable m_TargetTank;
    public GameObjectListVariable m_LastTargetTank;
    public Vector3ListVariableListVariable m_TargetPosContainer;
    public PointVariableListVariable m_TargetPointContainer;
    public PathManager m_PathManager;

    private Transform m_Transform;
    TargetType m_LastTargetType;

    private void Start()
    {
        m_Transform = transform;
    }

    void Update()
    {
        int tankIndex = m_TankIndexManager.m_TankIndex;
        ProceedMovements(m_MoveInstructions.m_MoveToFireRange[tankIndex] ? m_FirePlacementRange : 0f, tankIndex);
    }


    private void ProceedMovements(float targetDistanceToTarget, int tankIndex)
    {
        bool stopMoving = false;
        Vector3 tankPos = m_Transform.position;
        Vector3 tankForward = m_Transform.forward;
        Vector3 tankRight = m_Transform.right;
        TargetType targetType = m_MoveInstructions.m_TargetType[tankIndex];
        bool changedTargetType = targetType != m_LastTargetType;

        Vector3 targetPos;
        GameObject tankTarget = m_TargetTank.m_Values[tankIndex];
        Vector3ListVariable targetPosList = m_TargetPosContainer.m_Values[tankIndex];
        if (targetType == TargetType.tank && tankTarget && targetPosList)
        {
            targetPos = targetPosList.m_Values[tankIndex];
        }
        else if (targetType == TargetType.point || (targetType == TargetType.tank && m_MoveInstructions.m_UseDefaultPointValue[tankIndex]))
        {
            targetPos = m_TargetPointContainer.m_Values[tankIndex].m_CenterPos;
        }
        else
        {
            targetPos = Vector3.zero;
            stopMoving = true;
        }

        bool usePathfinding = m_MoveInstructions.m_UsePathfinding[tankIndex];

        if (usePathfinding)
        {
            bool targetChanged = m_PathManager.m_TargetPos != targetPos;
            Debug.Log(m_PathManager.m_TargetPos + " | "+ targetPos);
            if (targetType == TargetType.point && (targetChanged || changedTargetType))
            {
                m_PathManager.SearchPath(tankPos, targetPos);
            }
            else if (targetType == TargetType.tank)
            {
                targetChanged = m_LastTargetTank.m_Values[tankIndex] != m_TargetTank.m_Values[tankIndex];
                if (targetChanged || changedTargetType || m_PathManager.m_PathFound)
                {
                    m_PathManager.SearchPath(tankPos, targetPos);
                }
            }
            m_PathManager.UpdatePath(tankPos);
            if ((targetChanged || changedTargetType) && !m_PathManager.m_PathFound)
            {
                stopMoving = true;
            }
            targetPos = m_PathManager.GetActualWaypoint();
        }

        Vector3 distance = targetPos - tankPos;

        if (Mathf.Abs(distance.magnitude - targetDistanceToTarget) > m_RadiusTolerance && m_MoveInstructions.m_Move[tankIndex] && !stopMoving) { Move(distance, tankForward, tankIndex); }
        else { StopMove(tankIndex); }

        if (distance.magnitude > m_RadiusTolerance && m_MoveInstructions.m_Turn[tankIndex] && !stopMoving) { Turn(distance, tankForward, tankRight, tankIndex); }
        else { StopTurn(tankIndex); }

        m_LastTargetType = targetType;
    }

    private void Move(Vector3 distance, Vector3 tankForward, int tankIndex)
    {
        //m_MoveAxis.m_Values[tankIndex] = Mathf.Min(Vector3.Dot(distance, tankForward) >= 0 != (m_MoveInstructions.m_MoveToFireRange[tankIndex] && distance.magnitude - m_FirePlacementRange < 0)? 1 : -1, m_MoveInstructions.m_Follow[tankIndex] ? 1 : 0);
        m_MoveAxis.m_Values[tankIndex] = Mathf.Min(Vector3.Dot(distance.normalized, tankForward) * ((m_MoveInstructions.m_MoveToFireRange[tankIndex] && distance.magnitude - m_FirePlacementRange < 0) ? -1 : 1), m_MoveInstructions.m_Follow[tankIndex] ? 1 : 0);
    }
    
    private void Turn(Vector3 distance, Vector3 tankForward, Vector3 tankRight, int tankIndex)
    {
        if (!m_MoveInstructions.m_MoveForward[tankIndex]) { distance *= -1; }
        if (!m_TankIndexManager || !m_TurnAxis) return;
        float angleSign = Vector3.Dot(tankRight, distance) >= 0 ? 1 : -1;
        float angleToTarget = Mathf.Clamp(Vector3.Angle(distance, tankForward) * angleSign / m_AnleClamp, -1f, 1f);
        m_TurnAxis.m_Values[tankIndex] = Mathf.Abs(angleToTarget) > m_AngularTolerancePercentage ? angleToTarget : 0;
    }

    private void StopMove(int tankIndex)
    {
        m_MoveAxis.m_Values[tankIndex] = 0;
    }

    private void StopTurn(int tankIndex)
    {
        m_TurnAxis.m_Values[tankIndex] = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private TankIndexManager m_TankIndexManager;
    [SerializeField] private TankEvent m_FireCommandEvent;
    [SerializeField] private FloatListVariable m_MoveAxis;
    [SerializeField] private FloatListVariable m_TurnAxis;
    private string m_FireButton;                // The input axis that is used for launching shells.
    private string m_MovementAxisName;                // The input axis that is used for launching shells.
    private string m_TurnAxisName;                // The input axis that is used for launching shells.
    
    // Start is called before the first frame update
    void Start()
    {
        if (m_TankIndexManager)
        {
            // The axes names are based on player number.
            int playerNumber = m_TankIndexManager.m_TankIndex + 1;
            m_FireButton = "Fire" + playerNumber;
            m_MovementAxisName = "Vertical" + playerNumber;
            m_TurnAxisName = "Horizontal" + playerNumber;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAxis();
        if (!m_TankIndexManager) { return; } ;
        Fire();
    }

    private void UpdateAxis()
    {
        if (m_MoveAxis) { m_MoveAxis.m_Values[m_TankIndexManager.m_TankIndex] = Input.GetAxis(m_MovementAxisName); };
        if (m_TurnAxis) { m_TurnAxis.m_Values[m_TankIndexManager.m_TankIndex] = Input.GetAxis(m_TurnAxisName); };
    }

    private void Fire()
    {
        if (Input.GetButton(m_FireButton))
        {
            m_FireCommandEvent.Raise(m_TankIndexManager.m_TankIndex);
        }
    }
}

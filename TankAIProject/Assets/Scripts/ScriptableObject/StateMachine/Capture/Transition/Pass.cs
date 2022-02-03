using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Transition/Capture/Pass")]
public class Pass : TransitionBase
{
    public override bool IsValid(StateMachineManager stateMachineManager)
    {
        return true;
    }
}

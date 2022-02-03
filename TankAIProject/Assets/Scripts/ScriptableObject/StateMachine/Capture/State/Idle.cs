using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(menuName = "State Machine/State/Capture/Idle")]
    public class Idle : StateBase
    {
        public override void OnEnter(StateMachineManager stateMachineManager)
        { }
        public override void OnExit(StateMachineManager stateMachineManager)
        { }
        protected override void Execute(StateMachineManager stateMachineManager)
        { }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int movementRange;
    public MovementStateMachine movementStateMachine { get; private set; }

    private void Awake()
    {
        MovementController movementController = GetComponent<MovementController>();
        movementStateMachine = new MovementStateMachine(this, movementController);
    }

    void Update()
    {
        movementStateMachine.Update();
    }

    public void ToggleMovement(bool isEnabled)
    {
        movementStateMachine.Enable(isEnabled);
    }
}

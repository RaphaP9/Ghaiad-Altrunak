using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementInterruption
{
    public bool IsInterruptingMovement();
    public bool StopMovementOnInterruption();
}

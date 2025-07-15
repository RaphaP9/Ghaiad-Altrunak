using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingInterruption 
{
    public bool IsInterruptingFacing();
    public bool OverrideFacingDirection();
    public Vector2 GetFacingDirection();
}

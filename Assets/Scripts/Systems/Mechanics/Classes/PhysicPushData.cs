using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicPushData 
{
    public float pushForce;
    public IPushSource pushSource;

    public PhysicPushData(float pushForce, IPushSource pushSource)
    {
        this.pushForce = pushForce;
        this.pushSource = pushSource;
    }
}

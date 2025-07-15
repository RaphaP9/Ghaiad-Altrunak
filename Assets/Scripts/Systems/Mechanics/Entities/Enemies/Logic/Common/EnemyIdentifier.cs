using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentifier : EntityIdentifier
{
    public EnemySO EnemySO => entitySO as EnemySO;
}

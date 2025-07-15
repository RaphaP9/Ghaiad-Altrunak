using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public string GetAttackableName();
    public string GetAttackableDescription();
    public Sprite GetAttackableSprite();
}

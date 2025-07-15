using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSource 
{
    public string GetDamageSourceName();
    public string GetDamageSourceDescription();
    public Sprite GetDamageSourceSprite();
    public Color GetDamageSourceColor();

    public DamageSourceClassification GetDamageSourceClassification();
}

public enum DamageSourceClassification
{
    Character,
    Enemy,
    NeutralEntity,
    Ally,
    Treat
}
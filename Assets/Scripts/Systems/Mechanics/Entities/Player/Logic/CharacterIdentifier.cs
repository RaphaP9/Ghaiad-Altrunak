using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdentifier : EntityIdentifier
{
    public CharacterSO CharacterSO => entitySO as CharacterSO;

    public void SetCharacterSO(CharacterSO characterSO) => entitySO = characterSO;
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingData
{
    public InputAction inputAction;
    public int bindingIndex;

    public BindingData(InputAction inputAction, int bindingIndex)
    {
        this.inputAction = inputAction;
        this.bindingIndex = bindingIndex;
    }
}

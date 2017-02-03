using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemController : ItemController
{
    public KeyItemController(string name)
    {
       KeyCode = name;
    }


    public override bool IsThisKeyCode(string code)
    {
        return KeyCode.Equals(code);
    } 
}

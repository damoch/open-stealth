using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemController : ItemController
{
    public KeyItemController(string name)
    {
       keyCode = name;
    }


    public override bool isThisKeyCode(string code)
    {
        return keyCode.Equals(code);
    } 
}

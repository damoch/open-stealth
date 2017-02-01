using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemController : MonoBehaviour {
    public string keyCode;

    public abstract bool isThisKeyCode(string code);

}

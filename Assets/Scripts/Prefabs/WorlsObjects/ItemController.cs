using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemController : MonoBehaviour {
    public string KeyCode;

    public abstract bool IsThisKeyCode(string code);

}

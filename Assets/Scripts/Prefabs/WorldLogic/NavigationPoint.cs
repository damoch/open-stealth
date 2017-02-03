using UnityEngine;
using System.Collections;
using System;

public class NavigationPoint : MonoBehaviour {
    public NavigationPoint Next;

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}

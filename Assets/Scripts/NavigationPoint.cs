using UnityEngine;
using System.Collections;
using System;

public class NavigationPoint : MonoBehaviour {
    public NavigationPoint next;

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }
}

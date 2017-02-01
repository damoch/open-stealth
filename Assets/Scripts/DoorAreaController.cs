using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaController : MonoBehaviour {
    string requiredKeyCode;
    ExitController exit;

    private void Start()
    {
        exit = GetComponentInParent<ExitController>();
        requiredKeyCode = exit.requiredKeyCode;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            if (requiredKeyCode != null && other.gameObject.GetComponent<PlayerController>().hasKeyToDoor(requiredKeyCode))
            {
                exit.openDoor();
            }
            else
            {
                exit.setFlashingLight(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        exit.closeDoor();
        exit.setFlashingLight(false);
    }
}

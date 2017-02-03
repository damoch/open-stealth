using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaController : MonoBehaviour {
    string _requiredKeyCode;
    ExitController _exit;

    private void Start()
    {
        _exit = GetComponentInParent<ExitController>();
        _requiredKeyCode = _exit.RequiredKeyCode;
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "PlayerObject":
                var player = other.gameObject.GetComponent<PlayerController>();
                if (_requiredKeyCode != null && player.HasKeyToDoor(_requiredKeyCode))
                {
                    _exit.OpenDoor();
                }
                else
                {
                    _exit.SetFlashingLight(true);
                }
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _exit.CloseDoor();
        _exit.SetFlashingLight(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
    Light doorLight;
    bool flashingLight;
    public string requiredKeyCode;
    public string nextScene;
    GameObject frame;
    float openedPosition;
    float closedPosition;
    DoorState doorState;

    private void Start()
    {
        frame = transform.GetChild(0).gameObject;
        closedPosition = frame.transform.localPosition.x;
        openedPosition = frame.transform.localPosition.x + 2f;
        doorState = DoorState.CLOSED;
        doorLight = transform.GetChild(1).GetComponent<Light>();

        GetComponentInChildren<ExitAreaController>().nextRoom = nextScene == null ? "SAME_ROOM" : nextScene;
    }

    private void Update()
    {
        switch (doorState)
        {
            case DoorState.CLOSING:
                frame.transform.localPosition = new Vector3(frame.transform.localPosition.x - 0.01f, frame.transform.localPosition.y, frame.transform.localPosition.z);
                if(frame.transform.localPosition.x <= closedPosition)
                {
                    doorState = DoorState.CLOSED;
                }
                break;
            case DoorState.OPENING:
                frame.transform.localPosition = new Vector3(frame.transform.localPosition.x + 0.01f, frame.transform.localPosition.y, frame.transform.localPosition.z);
                if (frame.transform.localPosition.x >= openedPosition)
                {
                    doorState = DoorState.OPENED;
                }
                break;
            //case DoorState.STATIC:
            //    break;
                
        }
    }

    public void setFlashingLight(bool value)
    {
        doorLight.color = Color.red;
        flashingLight = value;
        StartCoroutine("flashLight");
    }

    IEnumerator flashLight()
    {
        doorLight.enabled = false;
        yield return new WaitForSeconds(0.5f);
        doorLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        if (flashingLight == true)
        {
            StartCoroutine("flashLight");
        }

    }

    public void openDoor()
    {
        doorLight.color = Color.green;
        doorState = DoorState.OPENING;

    }
    public void closeDoor()
    {
        doorLight.color = Color.red;
        doorState = DoorState.CLOSING;

    }
}

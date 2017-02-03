using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {
    Light _doorLight;
    bool _flashingLight;
    public string RequiredKeyCode;
    public string NextScene;
    GameObject _frame;
    float _openedPosition;
    float _closedPosition;
    DoorState _doorState;

    private void Start()
    {
        _frame = transform.GetChild(0).gameObject;
        _closedPosition = _frame.transform.localPosition.x;
        _openedPosition = _frame.transform.localPosition.x + 2f;
        _doorState = DoorState.Closed;
        _doorLight = transform.GetChild(1).GetComponent<Light>();

        GetComponentInChildren<ExitAreaController>().NextRoom = NextScene == null ? "SAME_ROOM" : NextScene;
    }

    private void Update()
    {
        switch (_doorState)
        {
            case DoorState.Closing:
                _frame.transform.localPosition = new Vector3(_frame.transform.localPosition.x - 0.01f, _frame.transform.localPosition.y, _frame.transform.localPosition.z);
                if(_frame.transform.localPosition.x <= _closedPosition)
                {
                    _doorState = DoorState.Closed;
                }
                break;
            case DoorState.Opening:
                _frame.transform.localPosition = new Vector3(_frame.transform.localPosition.x + 0.01f, _frame.transform.localPosition.y, _frame.transform.localPosition.z);
                if (_frame.transform.localPosition.x >= _openedPosition)
                {
                    _doorState = DoorState.Opened;
                }
                break;
            default:
                break;
                
        }
    }

    public void SetFlashingLight(bool value)
    {
        _doorLight.color = Color.red;
        _flashingLight = value;
        StartCoroutine("FlashLight");
    }

    IEnumerator FlashLight()
    {
        _doorLight.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _doorLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        if (_flashingLight == true)
        {
            StartCoroutine("FlashLight");
        }

    }

    public void OpenDoor()
    {
        _doorLight.color = Color.green;
        _doorState = DoorState.Opening;

    }
    public void CloseDoor()
    {
        _doorLight.color = Color.red;
        _doorState = DoorState.Closing;

    }
}

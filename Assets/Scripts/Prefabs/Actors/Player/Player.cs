using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public PlayerCameraState CameraState { get; set; }
    public List<KeyItem> Keys { get; set; }
    GameController _gameController;
    Camera _firstPerson;
    Camera _thirdPerson;
    Animator _animator;
    GameObject _model;
    Direction _direction;

    float _healthPoints;
    void Start () {
        _gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        Keys = new List<KeyItem>();
        Keys.Add(new KeyItem("NONE"));
        _thirdPerson = transform.GetChild(1).gameObject.GetComponent<Camera>();
        _firstPerson = GetComponent<Camera>();
        _model = transform.GetChild(2).gameObject;
        _animator = _model.GetComponent<Animator>();
        _animator.speed = 0f;
        SetCamera();
        _direction = Direction.North;
        _healthPoints = 100f;
    }
	
	void Update () {
        float x;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        if (Input.GetKeyUp(KeyCode.C))
        {
            SetCamera();
        }

        if (CameraState.Equals(PlayerCameraState.Thirdperson))
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
            transform.Translate(x, 0, z);
            SetPlayerDirection(z, x);
        }
        else if (CameraState.Equals(PlayerCameraState.Firstperson))
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);
        }


        _animator.speed = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ? 3f : 0f;
    }

    private void SetPlayerDirection(float z, float x)
    {
        if (z > 0 && !_direction.Equals(Direction.North))
        {
            _direction = Direction.North;
            _model.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (z < 0 && !_direction.Equals(Direction.South))
        {
            _direction = Direction.South;
            _model.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        }

        if (x > 0 && !_direction.Equals(Direction.East))
        {
            _direction = Direction.East;
            _model.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }

        if (x < 0 && !_direction.Equals(Direction.West))
        {
            _direction = Direction.West;
            _model.transform.localRotation = Quaternion.Euler(0, 270, 0);
        }
    }

    void SetCamera()
    {
        switch (CameraState)
        {
            case PlayerCameraState.Firstperson:
                _firstPerson.enabled = false;
                _thirdPerson.enabled = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                CameraState = PlayerCameraState.Thirdperson;
                break;
            case PlayerCameraState.Thirdperson:
                _firstPerson.enabled = true;
                _thirdPerson.enabled = false;
                CameraState = PlayerCameraState.Firstperson;
                break;
            default:
                _firstPerson.enabled = false;
                _thirdPerson.enabled = true;
                CameraState = PlayerCameraState.Thirdperson;
                break;
        }
    }


    public bool HasKeyToDoor(string keyCode)
    {
        foreach(KeyItem key in Keys)
        {
            if (key.IsThisKeyCode(keyCode))
            {
                return true;
            } 
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Projectile":

                _healthPoints -= 25f;
                if (_healthPoints <= 0)
                {
                    _gameController.GameOver();
                }
                break;

            case "Goal":

                _gameController.GameFinished();
                break;

            case "Item":

                var key = other.GetComponent<KeyItem>();
                Keys.Add(key);
                GameController.SetItemFlag(key.KeyCode);
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }
}

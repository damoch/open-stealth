using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    public PlayerCameraState cameraState { get; set; }
    public Texture2D crosshairImage;
    public List<KeyItemController> keys { get; set; }
    GameController gameController;
    Camera firstPerson;
    Camera thirdPerson;
    Animator animator;
    GameObject model;
    Direction direction;

    float healthPoints;
    void Start () {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        keys = new List<KeyItemController>();
        keys.Add(new KeyItemController("NONE"));
        thirdPerson = transform.GetChild(1).gameObject.GetComponent<Camera>();
        firstPerson = GetComponent<Camera>();
        model = transform.GetChild(2).gameObject;
        animator = model.GetComponent<Animator>();
        animator.speed = 0f;
        setCamera();
        direction = Direction.NORTH;
        healthPoints = 100f;
    }
	
	void Update () {
        float x;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        if (Input.GetKeyUp(KeyCode.C))
        {
            setCamera();
        }

        if (cameraState.Equals(PlayerCameraState.THIRDPERSON))
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
            transform.Translate(x, 0, z);


            if(z > 0 && !direction.Equals(Direction.NORTH))
            {
                direction = Direction.NORTH;
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if(z < 0 && !direction.Equals(Direction.SOUTH))
            {
                direction = Direction.SOUTH;
                model.transform.localRotation = Quaternion.Euler(0, 180f, 0);
            }

            if (x > 0 && !direction.Equals(Direction.EAST))
            {
                direction = Direction.EAST;
                model.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }

            if (x < 0 && !direction.Equals(Direction.WEST))
            {
                direction = Direction.WEST;
                model.transform.localRotation = Quaternion.Euler(0, 270, 0);
            }
        }
        else if (cameraState.Equals(PlayerCameraState.FIRSTPERSON))
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);
        }
        animator.speed = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ? 3f : 0f;
    }

    void setCamera()
    {
        switch (cameraState)
        {
            case PlayerCameraState.FIRSTPERSON:
                firstPerson.enabled = false;
                thirdPerson.enabled = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                cameraState = PlayerCameraState.THIRDPERSON;
                break;
            case PlayerCameraState.THIRDPERSON:
                firstPerson.enabled = true;
                thirdPerson.enabled = false;
                cameraState = PlayerCameraState.FIRSTPERSON;
                break;
            default:
                firstPerson.enabled = false;
                thirdPerson.enabled = true;
                cameraState = PlayerCameraState.THIRDPERSON;
                break;
        }
    }


    public bool hasKeyToDoor(string keyCode)
    {
        foreach(KeyItemController key in keys)
        {
            if (key.isThisKeyCode(keyCode))
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

                healthPoints -= 25f;
                if (healthPoints <= 0)
                {
                    gameController.GameOver();
                }
                break;

            case "Goal":

                gameController.GameFinished();
                break;

            case "Item":

                KeyItemController key = other.GetComponent<KeyItemController>();
                keys.Add(key);
                GameController.setItemFlag(key.keyCode);
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }
}

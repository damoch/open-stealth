using Assets.Scripts.Prefabs.Actors.Player;
using UnityEngine;

namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public class DoorArea : MonoBehaviour {
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
                    var player = other.gameObject.GetComponent<Player>();
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
}

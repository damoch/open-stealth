using UnityEngine;

namespace Assets.Scripts.Prefabs.Actors.Guard
{
    public class FieldOfView : MonoBehaviour {

        public bool PlayerInRange { get; set; }

        void OnTriggerEnter(Collider collider)
        {
        
            if(collider.tag == "PlayerObject")
            {
                PlayerInRange = true;
            }
        }
        void OnTriggerExit(Collider collider)
        {
            if (collider.tag == "PlayerObject")
            {
                PlayerInRange = false;
            }
        }
    }
}

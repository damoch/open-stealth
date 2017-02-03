using UnityEngine;
using System.Collections;

public class FieldOfViewController : MonoBehaviour {

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

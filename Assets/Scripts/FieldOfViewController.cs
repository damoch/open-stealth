using UnityEngine;
using System.Collections;

public class FieldOfViewController : MonoBehaviour {

    public bool playerInRange { get; set; }

    void OnTriggerEnter(Collider collider)
    {
        
        if(collider.tag == "PlayerObject")
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "PlayerObject")
        {
            playerInRange = false;
        }
    }
}

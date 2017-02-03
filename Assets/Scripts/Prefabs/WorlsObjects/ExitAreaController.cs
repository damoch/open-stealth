using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class ExitAreaController : MonoBehaviour {

    public string NextRoom { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerObject") && !NextRoom.Equals("SAME_ROOM"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextRoom);
        }
    }
}

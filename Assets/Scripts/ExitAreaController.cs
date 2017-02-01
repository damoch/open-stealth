using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class ExitAreaController : MonoBehaviour {

    public string nextRoom { get; set; }

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerObject") && !nextRoom.Equals("SAME_ROOM"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextRoom);
        }
    }
}

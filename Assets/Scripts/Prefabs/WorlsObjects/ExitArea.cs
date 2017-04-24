using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public class ExitArea : MonoBehaviour {

        public string NextRoom { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("PlayerObject") && !NextRoom.Equals("SAME_ROOM"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(NextRoom);
            }
        }
    }
}

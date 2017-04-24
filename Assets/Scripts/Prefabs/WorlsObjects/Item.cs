using UnityEngine;

namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public abstract class Item : MonoBehaviour {
        public string KeyCode;

        public abstract bool IsThisKeyCode(string code);

    }
}

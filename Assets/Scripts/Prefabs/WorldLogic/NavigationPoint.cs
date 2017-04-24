using UnityEngine;

namespace Assets.Scripts.Prefabs.WorldLogic
{
    public class NavigationPoint : MonoBehaviour {
        public NavigationPoint Next;

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }
    }
}

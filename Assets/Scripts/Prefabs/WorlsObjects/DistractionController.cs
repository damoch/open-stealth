using System.Collections;
using UnityEngine;

public class DistractionController : MonoBehaviour {

	void Start () {
        StartCoroutine("destroyObject");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Guard")) {
            other.GetComponent<GuardController>().SetSuspicious(transform.position, false);
        }
    }

    IEnumerator destroyObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}

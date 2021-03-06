﻿using System.Collections;
using Assets.Scripts.Prefabs.Actors.Guard;
using UnityEngine;

namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public class Distraction : MonoBehaviour {

        void Start () {
            StartCoroutine("destroyObject");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Guard")) {
                other.GetComponent<Guard>().SetSuspicious(transform.position, false);
            }
        }

        IEnumerator destroyObject()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
    }
}

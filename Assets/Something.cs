using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Something : MonoBehaviour
{
    public Material black, red;
    Transform triggeredBy = null;
    bool frozen = false;

    void Start() {
    }

    void Update() {
    }

	private void OnTriggerEnter(Collider other) {
        triggeredBy = other.transform;
    }

	private void LateUpdate() {
        if (triggeredBy == null) return;
        if (frozen) return;
        triggeredBy.GetComponent<Rigidbody>().isKinematic = true;
        triggeredBy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        triggeredBy.gameObject.GetComponent<MeshRenderer>().material = red;
        GetComponent<MeshRenderer>().material = red;
        frozen = true;
    }
}

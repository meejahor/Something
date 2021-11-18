using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableObject : MonoBehaviour
{
    public Material red;

    void Start() {
    }

    void Update() {
    }

	//private void OnTriggerEnter2D(Collider2D collision) {
 //       gameObject.SetActive(false);
 //   }

    private void TurnRed(Transform t) {
        t.GetComponent<MeshRenderer>().material = red;
    }

    public void TurnRed() {
        TurnRed(transform);
        foreach(Transform t in transform) {
            TurnRed(t);
        }
    }
}

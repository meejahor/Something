using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kati : MonoBehaviour
{
    public Material katiRed;
    Transform triggeredBy = null;

    void Start() {
    }

    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        triggeredBy = collision.transform;
    }

    private void LateUpdate() {
        if (triggeredBy == null) return;
        triggeredBy.GetComponent<Rigidbody2D>().isKinematic = true;
        triggeredBy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        triggeredBy.GetComponent<Player>().TurnRed();
        TurnRed();
        FindObjectOfType<Game>().PlayerFailed();
    }

    public void TurnRed() {
        GetComponent<MeshRenderer>().material = katiRed;
    }
}

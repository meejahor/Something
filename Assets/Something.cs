using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Something : MonoBehaviour
{
    public Material tryagain, nextlevel;
    Transform triggeredBy = null;
    bool frozen = false;
    bool success = false;

    void Start() {
    }

    void Update() {
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        triggeredBy = collision.transform;
    }

	private void LateUpdate() {
        if (triggeredBy == null) return;
        if (frozen) return;
        triggeredBy.GetComponent<Rigidbody2D>().isKinematic = true;
        triggeredBy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        triggeredBy.GetComponent<Player>().TurnRed();
        TurnRed();
    }

    public void TurnRed() {
        GetComponent<MeshRenderer>().material = tryagain;
        frozen = true;
    }

    public void Success() {
        GetComponent<MeshRenderer>().material = nextlevel;
        frozen = true;
        success = true;
    }

    private void OnMouseDown() {
        if (!frozen) return;

        if (success) {
            FindObjectOfType<Game>().NextScene();
        } else {
            FindObjectOfType<Game>().ReloadScene();
        }
    }
}

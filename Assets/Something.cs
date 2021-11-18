using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Something : MonoBehaviour
{
    public Material red, text_tryagain;
    public Transform textQuad;
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
        TurnRed();
    }

    public void TurnRed() {
        GetComponent<MeshRenderer>().material = red;
        textQuad.GetComponent<MeshRenderer>().material = text_tryagain;
        frozen = true;
    }

    private void OnMouseDown() {
        if (!frozen) return;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

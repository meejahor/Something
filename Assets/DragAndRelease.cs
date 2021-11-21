using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRelease : MonoBehaviour
{
    public Material dragAndRelease;
    public Transform tooltip;
    Vector3 startPos;
    const float RELEASE_DISTANCE = 0.5f;
    bool changed = false;

    void Start() {
        startPos = transform.position;
    }

    void Update() {
    }

	private void OnMouseDrag() {
		if (changed) return;
        if (Vector3.Distance(startPos, transform.position) <= RELEASE_DISTANCE) return;
        tooltip.GetComponent<MeshRenderer>().material = dragAndRelease;
        changed = true;
	}

	private void OnMouseUp() {
		tooltip.gameObject.SetActive(false);
	}
}

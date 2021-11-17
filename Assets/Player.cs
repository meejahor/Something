using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimingGroup, aimingMiddle, aimingEnd;
    bool dragging = false;
    bool launched = false;
    Vector3 dragOffset;
    Vector3 startPos;
    Rigidbody rb;
    const float MAX_DRAG_DISTANCE = 2;

    void Start() {
        startPos = transform.position;
        aimingGroup.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
    }

    void UpdateAimingCapsule() {
        aimingMiddle.position = Vector3.Lerp(transform.position, startPos, 0.5f);
        aimingEnd.position = startPos;
        Vector3 scale = aimingMiddle.localScale;
        scale.y = Vector3.Distance(transform.position, startPos);
        aimingMiddle.localScale = scale;
        Vector3 offset = startPos - transform.position;
        float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
        aimingMiddle.rotation = Quaternion.Euler(0, 0, -angle);
    }

	void OnMouseDown() {
        if (launched) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragOffset = worldPos - transform.position;
        aimingGroup.gameObject.SetActive(true);
        dragging = true;
    }

    void OnMouseDrag() {
        if (launched) return;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = worldPos - dragOffset;
        Vector3 offset = newPos - startPos;
        if (offset.magnitude > MAX_DRAG_DISTANCE) {
            offset = offset.normalized * MAX_DRAG_DISTANCE;
        }
        transform.position = startPos + offset;
        UpdateAimingCapsule();
    }

	private void OnMouseUp() {
        if (launched) return;
        dragging = false;
        aimingGroup.gameObject.SetActive(false);
        rb.isKinematic = false;
        Vector3 direction = startPos - transform.position;
        rb.AddForce(direction * 500);
        launched = true;
    }
}

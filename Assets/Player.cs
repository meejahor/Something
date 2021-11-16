using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimingGroup, aimingMiddle, aimingEnd;
    bool dragging = false;
    Vector3 dragOffset;
    Vector3 startPos;
    Rigidbody rb;

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
        Vector3 scale = Vector3.one;
        scale.y = Vector3.Distance(transform.position, startPos);
        aimingMiddle.localScale = scale;
        Vector3 offset = startPos - transform.position;
        float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
        aimingMiddle.rotation = Quaternion.Euler(0, 0, -angle);
    }

	void OnMouseDown() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragOffset = worldPos - transform.position;
        aimingGroup.gameObject.SetActive(true);
        dragging = true;
    }

    void OnMouseDrag() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = worldPos - dragOffset;
        UpdateAimingCapsule();
    }

	private void OnMouseUp() {
		dragging = false;
        aimingGroup.gameObject.SetActive(false);
        rb.isKinematic = false;
        Vector3 direction = startPos - transform.position;
        rb.AddForce(direction * 500);
    }
}

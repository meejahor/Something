using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimingGroup, aimingMiddle, aimingEnd;
    public Material red;
    public GameObject tryAgain;
    bool launched = false;
    Vector3 dragOffset;
    Vector3 startPos;
    Rigidbody2D rb;
    const float MAX_DRAG_DISTANCE = 2;

    float left, right, top, bottom;

    void Start() {
        startPos = transform.position;
        aimingGroup.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        Vector3 bl = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, Camera.main.nearClipPlane)
        );
        Vector3 tr = Camera.main.ViewportToWorldPoint(
            new Vector3(1, 1, Camera.main.nearClipPlane)
        );
        left = bl.x - 0.5f;
        bottom = bl.y - 0.5f;
        right = tr.x + 0.5f;
        top = tr.y + 0.5f;
        //Debug.Log(left);
        //Debug.Log(right);
        //Debug.Log(top);
        //Debug.Log(bottom);
    }

    void Update() {
        Vector3 pos = transform.position;
        if (pos.x < left) PlayerLeftScreen();
        if (pos.x > right) PlayerLeftScreen();
        if (pos.y < bottom) PlayerLeftScreen();
        if (pos.y > top) PlayerLeftScreen();
    }

    void PlayerLeftScreen() {
        gameObject.SetActive(false);
        if (FindObjectsOfType<RemovableObject>().Length > 0) {
            PlayerFailed();
        } else {
            FindObjectOfType<Game>().PlayerSucceeded();
        }
    }

	//private void PlayerSucceeded() {
	//   }

	private void PlayerFailed() {
		RemovableObject[] removables = FindObjectsOfType<RemovableObject>();
		foreach (RemovableObject removable in removables) {
			removable.TurnRed();
		}
        FindObjectOfType<Game>().PlayerFailed();
    }

    public void TurnRed() {
        GetComponent<MeshRenderer>().material = red;
    }

    void UpdateAimingCapsule() {
        aimingMiddle.position = Vector3.Lerp(transform.position, startPos, 0.5f) + Vector3.forward;
        aimingEnd.position = startPos + Vector3.forward;
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
        aimingGroup.gameObject.SetActive(false);
        rb.isKinematic = false;
        Vector3 direction = startPos - transform.position;
        rb.AddForce(direction * 500);
        launched = true;
    }
}

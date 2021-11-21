using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimingGroup, aimingMiddle, aimingEnd;
    public Transform ghost;
    public Material trajectoryMaterial;
    static bool showGhost = true;
    public static bool ghostIsValid = false;
    static Vector3 ghostPosition;
    public Material red;
    bool launched = false;
    Vector3 dragOffset;
    Vector3 startPos;
    Rigidbody2D rb;
    const float MAX_DRAG_DISTANCE = 2;
    GameObject traj;
    LineRenderer lr;
    Color trajColor;

    float left, right, top, bottom;

    void Start() {
        startPos = transform.position;
        //aimingGroup.gameObject.SetActive(false);
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

        //ghost.SetParent(null);
        //ghost.position = ghostPosition;
        //ghost.gameObject.SetActive(showGhost && ghostIsValid);

        traj = new GameObject();
        traj.transform.position = Vector3.forward;
        lr = traj.AddComponent<LineRenderer>();
        lr.material = trajectoryMaterial;
        lr.startWidth = lr.endWidth = 0.1f;
        trajColor = new Color(63, 63, 63, 0);
        lr.endColor = trajColor;
        lr.startWidth = 1;
        lr.endWidth = 0;
        lr.useWorldSpace = false;
    }

    (Vector2, float, float) CalculateLaunchDirectionAndForce() {
        Vector2 direction = startPos - transform.position;
        float mag = direction.magnitude;
        direction.Normalize();
        return (direction, mag, mag * 500);
    }

    void UpdateTrajectory() {
        List<Vector3> trajPoints = new List<Vector3>();
        traj.transform.position = transform.position;

        (Vector2 direction, float mag, float force) = CalculateLaunchDirectionAndForce();
        float velocity = force * rb.mass * Time.fixedDeltaTime;

		float timeStep = mag * 0.05f;
		for (int i = 0; i < 40; i++) {
            Vector2 trajPos = direction * velocity * timeStep * i;
            trajPos.y += Physics2D.gravity.y / 2 * Mathf.Pow(timeStep * i, 2);
            trajPoints.Add(trajPos);
        }

        lr.SetPositions(trajPoints.ToArray());
        lr.positionCount = trajPoints.Count;
        trajColor.a = Mathf.Clamp01(mag);
        lr.startColor = trajColor;
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

    //void UpdateAimingCapsule() {
    //    aimingMiddle.position = Vector3.Lerp(transform.position, startPos, 0.5f) + Vector3.forward;
    //    aimingEnd.position = startPos + Vector3.forward;
    //    Vector3 scale = aimingMiddle.localScale;
    //    scale.y = Vector3.Distance(transform.position, startPos);
    //    aimingMiddle.localScale = scale;
    //    Vector3 offset = startPos - transform.position;
    //    float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
    //    aimingMiddle.rotation = Quaternion.Euler(0, 0, -angle);
    //}

	void OnMouseDown() {
        if (launched) return;
        traj.SetActive(true);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragOffset = worldPos - transform.position;
        //aimingGroup.gameObject.SetActive(true);
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
        //UpdateAimingCapsule();
        UpdateTrajectory();
    }

	private void OnMouseUp() {
        if (launched) return;
        traj.SetActive(false);
        ghostPosition = transform.position + Vector3.forward * 2;
        ghostIsValid = true;
        ghost.gameObject.SetActive(false);
        aimingGroup.gameObject.SetActive(false);
        rb.isKinematic = false;
        (Vector2 direction, _, float force) = CalculateLaunchDirectionAndForce();
        rb.AddForce(direction * force);
        //rb.velocity = force;
        launched = true;
    }
}

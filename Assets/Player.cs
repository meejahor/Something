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
    const int NUM_TRAJECTORY_STEPS = 30;

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
        lr = traj.AddComponent<LineRenderer>();
		lr.material = trajectoryMaterial;
        trajColor = lr.material.color;
        trajColor.a = 0;
        lr.endColor = trajColor;
        lr.startWidth = 1;
        lr.endWidth = 0;
        lr.useWorldSpace = false;
        lr.textureMode = LineTextureMode.DistributePerSegment;
    }

    (Vector2, float, float) CalculateLaunchDirectionAndForce() {
        Vector2 direction = startPos - transform.position;
        float mag = direction.magnitude;
        direction.Normalize();
        return (direction, mag, mag * 500);
    }

    void UpdateTrajectory() {
        List<Vector3> trajPoints = new List<Vector3>();
        traj.transform.position = transform.position + Vector3.forward;

        (Vector2 direction, float mag, float force) = CalculateLaunchDirectionAndForce();

        // based on fixed-step trajectory maths by Dan Schatzeder and other sources
        // https://schatzeder.medium.com/basic-trajectory-prediction-in-unity-8537b52e1b34

        float velocity = force * rb.mass * Time.fixedDeltaTime;

		float timeStep = mag * 0.1f;
        timeStep /= NUM_TRAJECTORY_STEPS;
        float time = 0;
        float timeToAdd = timeStep * NUM_TRAJECTORY_STEPS;
        float dist = 0;
        Vector2 prev = Vector2.zero;
        trajPoints.Add(prev);

        for (int i = 1; i < NUM_TRAJECTORY_STEPS; i++) {
            time += timeToAdd;
            timeToAdd -= timeStep;
            Vector2 trajPos = direction * velocity * time;
            trajPos.y += Physics2D.gravity.y / 2 * Mathf.Pow(time, 2);
			dist += Vector2.Distance(prev, trajPos);
            trajPoints.Add(trajPos);
            prev = trajPos;
        }

        lr.material.mainTextureScale = new Vector2(dist * 2, 1);

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

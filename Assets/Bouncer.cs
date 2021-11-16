using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    Vector3 startScale;
    float popLerp;
    const float POP_SPEED = 5;
    bool popping = false;

    void Start() {
        startScale = transform.localScale;
    }

    void Update() {
        if (!popping) return;
        popLerp += POP_SPEED * Time.deltaTime;
        if (popLerp >= 1) {
            popLerp = 1;
            popping = false;
        }
        transform.localScale = Vector3.Lerp(startScale * 1.25f, startScale, popLerp);
    }

	private void OnCollisionEnter(Collision collision) {
        Transform player = collision.collider.transform;
        Vector3 direction = player.position - transform.position;
        player.GetComponent<Rigidbody>().AddForce(direction * 500);
        popping = true;
        popLerp = 0f;
	}
}

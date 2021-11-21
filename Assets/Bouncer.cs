using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    Vector3 startScale;
    float popLerp;
    const float POP_SCALE = 1.25f;
    const float POP_SPEED = 5;
    bool popping = false;
    Material mat;
    Color startColor, endColor;
    Collider2D col;

    void Start() {
        startScale = transform.localScale;
        mat = GetComponent<MeshRenderer>().material;
        startColor = endColor = mat.color;
        endColor.a = 0;
        col = GetComponent<Collider2D>();
    }

    void Update() {
        if (!popping) return;
        popLerp += POP_SPEED * Time.deltaTime;
        if (popLerp >= 1) {
            gameObject.SetActive(false);
        }
        transform.localScale = Vector3.Lerp(startScale * POP_SCALE, startScale, popLerp);
        mat.color = Color.Lerp(startColor, endColor, popLerp);
    }

	private void OnCollisionEnter2D(Collision2D collision) {
        Transform player = collision.collider.transform;
        Vector3 direction = player.position - transform.position;
        player.GetComponent<Rigidbody2D>().AddForce(direction * 500);
        popping = true;
        popLerp = 0;
        col.enabled = false;
        Destroy(GetComponent<RemovableObject>());
	}
}

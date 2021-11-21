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
    public Material mat;
    Color startColor, endColor;
    Collider2D col;
    MeshRenderer mr;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        mat = new Material(mr.material);
        mr.material = mat;
        startColor = endColor = mat.color;
        startScale = transform.localScale;
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
        //mr.material = blue;
        Transform player = collision.collider.transform;
        Vector3 direction = player.position - transform.position;
        player.GetComponent<Rigidbody2D>().AddForce(direction * 500);
        popping = true;
        popLerp = 0;
        col.enabled = false;
        Destroy(GetComponent<RemovableObject>());
	}
}

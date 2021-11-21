using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectWhenKatiIsHit : MonoBehaviour
{
    public GameObject objectToShow;
    static bool showWhenHit = true;

    void Start() {
    }

    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        objectToShow.SetActive(showWhenHit);
        showWhenHit = false;
    }
}

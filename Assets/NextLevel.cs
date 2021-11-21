using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    void Start() {
    }

    void Update() {
    }

    private void OnMouseDown() {
        FindObjectOfType<Game>().NextScene();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgain : MonoBehaviour
{
    void Start() {
    }

    void Update() {
    }

    private void OnMouseDown() {
        FindObjectOfType<Game>().ReloadScene();
    }
}

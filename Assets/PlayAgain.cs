using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    void Start() {
    }

    void Update() {
    }

    private void OnMouseDown() {
        SceneManager.LoadScene(0);
    }
}

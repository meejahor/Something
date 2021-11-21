using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject nextLevel, tryAgain;

    void Start() {
    }

    void Update() {
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene() {
        Player.ghostIsValid = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayerSucceeded() {
        nextLevel.SetActive(true);
    }

    public void PlayerFailed() {
        tryAgain.SetActive(true);
    }
}

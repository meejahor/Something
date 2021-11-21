using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectWhenPlayerReleased : MonoBehaviour
{
    static Dictionary<string, bool> showOnLoad = new Dictionary<string, bool>();
    public GameObject objectToHide;

    void Start() {
        if (objectToHide == null) {
            Destroy(this);
            return;
        }

        bool show = showOnLoad.GetValueOrDefault(objectToHide.name, true);
        objectToHide.SetActive(show);

        if (!showOnLoad.ContainsKey(objectToHide.name)) {
            showOnLoad.Add(objectToHide.name, false);
        }
    }

    void Update() {
    }

	private void OnMouseUp() {
        objectToHide.SetActive(false);
    }
}

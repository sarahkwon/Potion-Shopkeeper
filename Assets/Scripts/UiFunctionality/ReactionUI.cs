using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    Transform ui;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>()) {
            if (c.renderMode == RenderMode.WorldSpace) {
                ui = Instantiate(uiPrefab, c.transform).transform;
                ui.gameObject.SetActive(false);

                break;
             }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ui.position = target.position;
        ui.forward = -cam.forward;
    }

    public void ShowReactionUI() {
        ui.gameObject.SetActive(true);
    }

    public void HideReactionUI() {
        ui.gameObject.SetActive(false);
    }

    public void DestroyReactionUI() {
        Destroy(ui.gameObject);
    }

    public void UpdateReactionSprite(Sprite sprite) {
        ui.GetChild(0).GetComponent<Image>().sprite = sprite;
    }
}

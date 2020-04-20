using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnvironment : MonoBehaviour {
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float minThreshold = .1f;
    [SerializeField] Texture2D cursorTexture = null;

    [SerializeField] LightningController lightningController = default;

    float axis;
    IGetClicked iGetClicked;
    Ray ray;
    RaycastHit hit;

    bool isLightning = false;

    void Start() { }

    //this will be heavily refactored once gameplay is proven
    void Update() {
        axis = Input.GetAxisRaw("Horizontal");

        axis = axis > minThreshold ? 1 : axis;
        axis = axis < -minThreshold ? -1 : axis;

        if (axis != 0) {
            transform.Rotate(0, axis * rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetMouseButtonDown(0)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, 102)) {
                return;
            }

            iGetClicked = hit.collider.gameObject.GetComponentInParent<IGetClicked>();
            iGetClicked?.OnClick();

            if (isLightning) {
                isLightning = false;
                lightningController.DoLightning(hit.point);
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            }

            //Debug.DrawLine(ray.origin, hit.point);
            //Debug.Log(hit.point + " " + hit.collider.gameObject.name);
        }
    }

    public void EnableLightning() {
        Cursor.SetCursor(cursorTexture, new Vector2(16f,16f), CursorMode.Auto);
        isLightning = true;
    } 

}

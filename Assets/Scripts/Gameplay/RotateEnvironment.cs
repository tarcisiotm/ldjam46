using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnvironment : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float minThreshold = .1f;

    float axis;

    void Start(){}

    //this will be heavily refactored once gameplay is proven
    void Update()
    {
        axis = Input.GetAxisRaw("Horizontal");

        axis = axis > minThreshold ? 1 : axis;
        axis = axis < -minThreshold ? -1 : axis;

        if (axis != 0) {
            transform.Rotate(0, axis * rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 102)) {
                Debug.DrawLine(ray.origin, hit.point);
                Debug.Log(hit.point + " " + hit.collider.gameObject.name);

            }
        }

    }
}

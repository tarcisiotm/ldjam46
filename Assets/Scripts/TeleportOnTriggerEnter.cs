using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnTriggerEnter : MonoBehaviour
{
    [SerializeField] Vector2 minMaxYPos = new Vector2(-13, -6);
    [SerializeField] Transform targetPos;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        other.transform.position = new Vector3(targetPos.position.x ,Random.Range(minMaxYPos.x, minMaxYPos.y), other.transform.position.z);
    }

}

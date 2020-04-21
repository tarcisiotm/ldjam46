using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalCameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera = default;
    [SerializeField] Vector3 finalPos = default;
    [SerializeField] float finalFov = 70;
    [SerializeField] float duration = 3f;
    [SerializeField] float delay = 2f;

    void Start()
    {
        mainCamera.DOFieldOfView(finalFov, duration).SetDelay(delay);
        mainCamera.transform.DOLocalMove(finalPos, duration).SetDelay(delay);
    }

    void OnFovDone() { }

}
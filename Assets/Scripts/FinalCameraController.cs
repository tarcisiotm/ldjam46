using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class FinalCameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera = default;
    [SerializeField] Vector3 finalPos = default;
    [SerializeField] float finalFov = 70;
    [SerializeField] float duration = 3f;
    [SerializeField] float delay = 2f;

    [Space]
    [SerializeField] UnityEvent onBegin = default;
    [SerializeField] UnityEvent onEnd = default;

    void Start()
    {
        onBegin?.Invoke();
        CenterCamera();
    }

    void OnFovDone() {
        onEnd?.Invoke();
    }

    public void CenterCamera()
    {
        mainCamera.DOFieldOfView(finalFov, duration).SetDelay(delay).OnComplete(OnFovDone);
        mainCamera.transform.DOLocalMove(finalPos, duration).SetDelay(delay);
    }

}
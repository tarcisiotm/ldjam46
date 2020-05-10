using UnityEngine;
using UnityEngine.Events;
using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;

public class CameraRotationCut : MonoBehaviour
{
    [SerializeField] GameObject environment = default;
    [SerializeField] Camera mainCamera = default;
    [SerializeField] ProCamera2D proCamera2D = default;
    [SerializeField] ProCamera2DPanAndZoom proCameraPanAndZoom = default;
    [Space]
    [SerializeField] int targetFov = 26;
    [SerializeField] Vector3 pos = default;
    [SerializeField] Vector3 eulerAngles = default;
    [SerializeField] bool destroyOnStart = true;
    [SerializeField] bool useThisGOPos = false;

    //bool cachedPanAndZoomValue = false;

    bool hasPlayed = false;

    private void Start() {
        
    }

    private void Update() {
        if (hasPlayed) { return; }
        //cachedPanAndZoomValue = proCameraPanAndZoom.AllowZoom;
        proCameraPanAndZoom.AllowZoom = false;
        proCameraPanAndZoom.AllowPan = false;
    }

    private void LateUpdate() {
        if (hasPlayed) { return; }
        SetPos(useThisGOPos ? transform.position : pos);
        SetFov(targetFov);
        SetRotation(eulerAngles);
        proCameraPanAndZoom.CenterPanTargetOnCamera(1);
        StartCoroutine(WaitAndDo());
        hasPlayed = true;
 
    }

    IEnumerator WaitAndDo() {
        yield return null;
        yield return null;

        proCameraPanAndZoom.AllowZoom = proCameraPanAndZoom.AllowPan = true;

        if (destroyOnStart) {
            Destroy(gameObject);
        }
    }

    public void SetFov(int fov) {
        mainCamera.fieldOfView = fov;
    }

    public void SetPos(Vector3 pos) {
        proCamera2D.MoveCameraInstantlyToPosition(new Vector2(pos.x, pos.z));
        //mainCamera.transform.position = pos;
    }

    public void SetRotation(Vector3 rot) {
        environment.transform.rotation = Quaternion.Euler(rot);
    }

}
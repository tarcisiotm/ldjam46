using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TG.Core;
using UnityEngine;
using UnityEngine.Events;

public class RotateEnvironment : MonoBehaviour {
    [SerializeField] float scale = .2f;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float minThreshold = .1f;
    [SerializeField] Texture2D cursorTexture = null;
    [SerializeField] Texture2D butterflyTexture = null;
    [SerializeField] GameObject butterflyPrefab = null;
    [SerializeField] int butterflyLimit = 10;

    [SerializeField] LightningController lightningController = default;

    [Space]
    [SerializeField] int targetButterfliesForEnding = 5;
    [SerializeField] UnityEvent onEnding = null;

    bool hasTriggeredEnding = false;

    List<Butterfly> butterflies = new List<Butterfly>();

    float axis;
    IGetClicked iGetClicked;
    Ray ray;
    RaycastHit hit;

    bool isLightning = false;
    bool isButterfly = false;

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

            if (!isButterfly)
            {
                iGetClicked?.OnClick();
            }
            else
            {
                SpawnButterly(hit.point);
            }

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

    public void EnableButterfly()
    {
        Cursor.SetCursor(butterflyTexture, new Vector2(16f, 16f), CursorMode.Auto);
        isButterfly = true;
    }

    void SpawnButterly(Vector3 pos)
    {
        GameObject go = PoolingManager.I.GetPooledObject(butterflyPrefab);
        pos.y += Random.Range(0, .2f);

        go.transform.position = pos;
        go.transform.localScale = Vector3.one * .001f;
        go.SetActive(true);
        go.transform.DOScale(Vector3.one * scale, .2f);

        butterflies.Add(go.GetComponent<Butterfly>());

        if(!hasTriggeredEnding && butterflies.Count > targetButterfliesForEnding)
        {
            hasTriggeredEnding = true;
            onEnding?.Invoke();
        }

        if(butterflies.Count > butterflyLimit)
        {
            butterflies[0].Die();
            butterflies.Remove(butterflies[0]);
        }

        GameplayManager.I.AudioMelody.Ping(pos);
    }

    public void DisableButterfly()
    {
        isButterfly = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}

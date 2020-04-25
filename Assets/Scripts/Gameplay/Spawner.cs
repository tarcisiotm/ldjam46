using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour {

    [SerializeField] Transform centerPoint;
    [SerializeField] float minDistanceToSpawn = 2f;
    [SerializeField] Transform spawnerParent;

    [SerializeField] GameObject[] plants;

    Ray ray;
    RaycastHit hit;

    bool canSpawn = false;

    public void Spawn() {
        if (!canSpawn) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
            return;
        }

        if (Vector3.Distance(centerPoint.position ,hit.point) < minDistanceToSpawn) {
            Debug.Log("Spawn particles");
            return;
        }

        int index = Random.Range(0, plants.Length);

        GameObject go = Instantiate(plants[index]);
        go.transform.position = hit.point;
        go.transform.SetParent(spawnerParent, true);
        go.transform.localScale = Vector3.one * .001f;
        go.transform.DOScale(Vector3.one, .2f);

        GameplayManager.I.AudioMelody.Ping(hit.point);

        //if (isLightning) {
        //    isLightning = false;
        //    lightningController.DoLightning(hit.point);
        //    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void EnableSpawn() {
        canSpawn = true;
    }

    //Debug.DrawLine(ray.origin, hit.point);
    //Debug.Log(hit.point + " " + hit.collider.gameObject.name);
}
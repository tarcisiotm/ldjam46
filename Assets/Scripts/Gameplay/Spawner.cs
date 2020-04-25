using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour {
    [SerializeField] GameObject[] plants;
    [SerializeField] Transform spawnerParent;

    Ray ray;
    RaycastHit hit;

    public void Spawn() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
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

    //Debug.DrawLine(ray.origin, hit.point);
    //Debug.Log(hit.point + " " + hit.collider.gameObject.name);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour {

    [SerializeField] Transform centerPoint;

    [SerializeField] float minDistanceToSpawnFromCenter = .75f; // from center
    [SerializeField] float minSmallDistanceToSpawn = .2f;
    [SerializeField] float minMediumDistanceToSpawn = .75f;
    [SerializeField] float minBigDistanceToSpawn = 1.5f;

    [SerializeField] Transform spawnerParent;

    [Space]
    [SerializeField] float minSmallRadiusDensityCheck = .25f;
    [SerializeField] float minMediumRadiusDensityCheck = .75f;
    [SerializeField] float minBigRadiusDensityCheck = 1.5f;

    [Space]
    [SerializeField] int smallDensityThreshold = 4;
    [SerializeField] int mediumDensityThreshold = 2;
    [SerializeField] int bigDensityThreshold = 1;

    [Space]
    [SerializeField] GameObject[] smallPlants;
    [SerializeField] GameObject[] plants;
    [SerializeField] GameObject[] bigPlants;

    [Space]
    [SerializeField] GameObject[] water;
    [SerializeField] GameObject[] nearWater;

    Ray ray;
    RaycastHit hit;
    List<GameObject> toRemove = new List<GameObject>();

    bool isBigMaxed, isMediumMaxed, isSmallMaxed = false;


    [Header("Debug")]
    public List<GameObject> smallSpawnedObjects = new List<GameObject>();
    public List<GameObject> mediumSpawnedObjects = new List<GameObject>();
    public List<GameObject> bigSpawnedObjects = new List<GameObject>();
    [Space]

    public bool canSpawn = true;

    bool isMediumAllowed = true;
    bool isBigAllowed = true;

    bool shouldAbortSpawn = false;

    //todo cooldown?

    private void Start() {
    }

    public void Spawn() {
        if (!canSpawn) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
            return;
        }

        //TODO check if should check for this or if center point is null
        if (Vector3.Distance(centerPoint.position ,hit.point) < minDistanceToSpawnFromCenter) {
            Debug.Log("Spawn particles");
            return;
        }

        isBigMaxed = isBigAllowed && IsBigDensityMaxed(hit.point, false);
        isMediumMaxed = isMediumAllowed && IsMediumDensityMaxed(hit.point, false);
        isSmallMaxed = IsSmallDensityMaxed(hit.point, false);

        if (!IsBigDensityMaxed(hit.point, true) && IsMediumDensityMaxed(hit.point, false)) {
            Debug.Log("Big!");
            SpawnFromList(bigPlants, ref bigSpawnedObjects, hit.point);
            return;
        }

        if (IsSmallDensityMaxed(hit.point, true) && !IsMediumDensityMaxed(hit.point, true)) {
            Debug.Log("Medium!");
            SpawnFromList(plants, ref mediumSpawnedObjects, hit.point);
            return;
        }

        Debug.Log("Small!");

        SpawnFromList(smallPlants, ref smallSpawnedObjects, hit.point);
    }

    void SpawnFromList(GameObject[] list, ref List<GameObject> spawnList, Vector3 pos) {

        //if (shouldAbortSpawn) {
        //    Debug.Log("Spawn aborted");
        //    shouldAbortSpawn = false;
        //    return;
        //}

        int index = Random.Range(0, list.Length);

        GameObject go = Instantiate(list[index]);
        go.transform.position = pos;
        go.transform.SetParent(spawnerParent, true);
        go.transform.localScale = Vector3.one * .001f;
        go.transform.DOScale(Vector3.one, .2f);

        spawnList.Add(go);

        GameplayManager.I.AudioMelody.Ping(pos);
    }

    public void EnableSpawn() {
        canSpawn = true;
    }

    bool CheckDensity(
        Vector3 pos,
        List<GameObject> list,
        int maxDensity,
        float minDistanceToSpawn,
        float radiusToCheck,
        bool canRemove
    ) {
        int density = 0;
        float distance = 0;
        bool result = false;
        bool hasRemoved = false;

        //if too close do not spawn

        for (int i = 0; i < list.Count; i++) {
            distance = Vector3.Distance(list[i].transform.position, pos);

            if (distance > radiusToCheck) {
                continue;
            }

            if(canRemove && !hasRemoved && distance < minDistanceToSpawn) {
                GameObject go = list[i];
                go.transform.DOScale(Vector3.one * .001f, .3f).OnComplete(OnFadeOutComplete);
                toRemove.Add(go);
                hasRemoved = true;
            }

            density++;

            if (density > maxDensity) {
                result = true;
            }

        }

        foreach(var g in toRemove) {
            list.Remove(g);
        }

        return result;
    }

    bool IsSmallDensityMaxed(Vector3 pos, bool canRemove) {
        return CheckDensity(pos, smallSpawnedObjects, smallDensityThreshold, minSmallDistanceToSpawn, minSmallRadiusDensityCheck, canRemove);
    }

    bool IsMediumDensityMaxed(Vector3 pos, bool canRemove) {
        return CheckDensity(pos, mediumSpawnedObjects, mediumDensityThreshold, minMediumDistanceToSpawn, minMediumRadiusDensityCheck, canRemove);
    }

    bool IsBigDensityMaxed(Vector3 pos, bool canRemove) {
        return CheckDensity(pos, bigSpawnedObjects, bigDensityThreshold, minBigDistanceToSpawn, minBigRadiusDensityCheck, canRemove);
    }

    void OnFadeOutComplete() {

        foreach (var g in toRemove) {
            Destroy(g);
        }

        toRemove.Clear();
    }

    //Debug.DrawLine(ray.origin, hit.point);
    //Debug.Log(hit.point + " " + hit.collider.gameObject.name);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour {

    [SerializeField] Transform centerPoint;
    [SerializeField] Transform spawnerParent;

    [SerializeField] float minDistanceToSpawnFromCenter = .75f; // from center

    [Header("Density check dist")]
    [SerializeField] float minSmallDistanceToSpawn = .2f;
    [SerializeField] float minMediumDistanceToSpawn = .75f;
    [SerializeField] float minBigDistanceToSpawn = 1.5f;

    [Header("Clear radius")]
    [SerializeField] float smallInnerRadius = .15f;
    [SerializeField] float mediumInnerRadius = .25f;
    [SerializeField] float bigInnerRadius = .35f;
    [SerializeField] float waterInnerRadius = .35f;

    [Space]
    [SerializeField] int smallDensityThreshold = 4;
    [SerializeField] int mediumDensityThreshold = 2;
    [SerializeField] int bigDensityThreshold = 1;
    //[SerializeField] int nearWaterDensity = 4;
    [SerializeField] int waterDensity = 3;

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

    public delegate void Spawned(int smallTotal, int mediumTotal, int bigTotal, int waterTotal, int nearWaterTotal);
    public event Spawned OnSpawned;

    [Header("Debug")]
    public List<GameObject> smallSpawnedObjects = new List<GameObject>();
    public List<GameObject> mediumSpawnedObjects = new List<GameObject>();
    public List<GameObject> bigSpawnedObjects = new List<GameObject>();

    public List<GameObject> nearWaterSpawedObjects = new List<GameObject>();
    public List<GameObject> waterSpawnedObjects = new List<GameObject>();
    [Space]

    public bool canSpawn = true;

    bool isMediumAllowed = true;
    bool isBigAllowed = true;

    bool markedForRemoval = false;

    private void Start() {
    }

    public void Spawn() {
        if (!canSpawn) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
            return;
        }

        //TODO check if should check for this or if center point is null
        if (Vector3.Distance(centerPoint.position, hit.point) < minDistanceToSpawnFromCenter) {
            Debug.Log("Spawn particles");
            return;
        }

        isBigMaxed = isBigAllowed && IsBigDensityMaxed(hit.point);
        isMediumMaxed = isMediumAllowed && IsMediumDensityMaxed(hit.point);
        isSmallMaxed = IsSmallDensityMaxed(hit.point);

        if (isMediumMaxed) {
            ClearOthers(hit.point, bigInnerRadius * 2f, bigSpawnedObjects);
            ClearOthers(hit.point, bigInnerRadius * 1.5f, mediumSpawnedObjects);
            ClearOthers(hit.point, bigInnerRadius, smallSpawnedObjects);
            SpawnFromList(bigPlants, ref bigSpawnedObjects, hit.point);
            return;
        }

        if (isSmallMaxed) {
            ClearOthers(hit.point, mediumInnerRadius * 1.5f, mediumSpawnedObjects);
            ClearOthers(hit.point, mediumInnerRadius, smallSpawnedObjects);
            SpawnFromList(plants, ref mediumSpawnedObjects, hit.point);
            return;
        }

        ClearOthers(hit.point, smallInnerRadius, smallSpawnedObjects);

        SpawnFromList(smallPlants, ref smallSpawnedObjects, hit.point);
    }

    public void SpawnOnWater() {
        //TODO REMOVE DUPLICATE CODE
        if (!canSpawn) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
            return;
        }

        ClearOthers(hit.point, waterInnerRadius, waterSpawnedObjects);
        SpawnFromList(water, ref waterSpawnedObjects, hit.point);
        //if max spawn fish
        //if max fish do not spawn
    }

    public void SpawnNearWater() {
        //TODO REMOVE DUPLICATE CODE
        if (!canSpawn) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 102)) {
            return;
        }

        ClearOthers(hit.point, smallInnerRadius, nearWaterSpawedObjects);
        SpawnFromList(nearWater, ref nearWaterSpawedObjects, hit.point);
    }

    void ClearOthers(Vector3 pos, float innerRadius, List<GameObject> list) {
        float distance = 0;

        for (int i = 0; i < list.Count; i++) {
            distance = Vector3.Distance(list[i].transform.position, pos);

            if (distance > innerRadius) {
                continue;
            }

            GameObject go = list[i];

            if (!markedForRemoval) {
                go.transform.DOScale(Vector3.one * .001f, .3f).OnComplete(OnFadeOutComplete);
                markedForRemoval = true;
            } else {
                go.transform.DOScale(Vector3.one * .001f, .3f);
            }

            toRemove.Add(go);
        }

        foreach (var g in toRemove) {
            list.Remove(g);
        }
    }

    void SpawnFromList(GameObject[] list, ref List<GameObject> spawnList, Vector3 pos) {
        int index = Random.Range(0, list.Length);

        GameObject go = Instantiate(list[index]);
        Vector3 originalScale = go.transform.localScale;

        go.transform.SetParent(spawnerParent, false);
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * .001f;
        go.transform.DOScale(originalScale, .2f);

        spawnList.Add(go);

        GameplayManager.I.AudioMelody.Ping(pos);
        SendSpawnEvent();
    }

    void SendSpawnEvent() {
        OnSpawned?.Invoke(smallSpawnedObjects.Count, mediumSpawnedObjects.Count, bigSpawnedObjects.Count, waterSpawnedObjects.Count, nearWaterSpawedObjects.Count);
    }

    public void EnableSpawn() {
        canSpawn = true;
    }

    bool CheckDensity(
        Vector3 pos,
        List<GameObject> list,
        int maxDensity,
        float radiusToCheck
    ) {
        int density = 0;
        float distance = 0;
        bool result = false;

        for (int i = 0; i < list.Count; i++) {
            distance = Vector3.Distance(list[i].transform.position, pos);

            if (distance > radiusToCheck) {
                continue;
            }

            density++;

            if (density >= maxDensity) {
                result = true;
            }

        }

        return result;
    }

    bool IsSmallDensityMaxed(Vector3 pos) {
        return CheckDensity(pos, smallSpawnedObjects, smallDensityThreshold, minSmallDistanceToSpawn);
    }

    bool IsMediumDensityMaxed(Vector3 pos) {
        return CheckDensity(pos, mediumSpawnedObjects, mediumDensityThreshold, minMediumDistanceToSpawn);
    }

    bool IsBigDensityMaxed(Vector3 pos) {
        return CheckDensity(pos, bigSpawnedObjects, bigDensityThreshold, minBigDistanceToSpawn);
    }

    void OnFadeOutComplete() {
        StartCoroutine(WaitSomeMore());
    }

    IEnumerator WaitSomeMore() {
        yield return new WaitForSeconds(.3f);

        foreach (var g in toRemove) {
            Destroy(g);
        }

        toRemove.Clear();

        markedForRemoval = false;
    }

}
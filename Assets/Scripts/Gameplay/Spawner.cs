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

    bool markedForRemoval = false;

    //todo cooldown?

    private void Start() {
    }

    public void Spawn() {
        if (!canSpawn) { return; }

        if (markedForRemoval) {
            //Debug.Log("Should wait?");
        }

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
            ClearOthers(hit.point, bigInnerRadius, mediumSpawnedObjects);
            ClearOthers(hit.point, bigInnerRadius, smallSpawnedObjects);
            SpawnFromList(bigPlants, ref bigSpawnedObjects, hit.point);
            return;
        }

        if (isSmallMaxed) {
            ClearOthers(hit.point, mediumInnerRadius, mediumSpawnedObjects);
            ClearOthers(hit.point, mediumInnerRadius, smallSpawnedObjects);
            SpawnFromList(plants, ref mediumSpawnedObjects, hit.point);
            return;
        }

        ClearOthers(hit.point, smallInnerRadius, smallSpawnedObjects);

        SpawnFromList(smallPlants, ref smallSpawnedObjects, hit.point);
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
            Debug.Log("Density reached: "+density + "  "+maxDensity);

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

    //Debug.DrawLine(ray.origin, hit.point);
    //Debug.Log(hit.point + " " + hit.collider.gameObject.name);
}
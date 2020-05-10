using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] Transform originalWaypointParent;
    [SerializeField] Transform finalWaypointParent;
    [SerializeField] List<Transform> waypoints = new List<Transform>();

    void Start()
    {
        AddAllWaypointsToList(originalWaypointParent);
    }

    public void AddFinalWaypoints()
    {
        AddAllWaypointsToList(finalWaypointParent);
    }

    void AddAllWaypointsToList(Transform parentTransformm) {
        for (int i = 0; i < parentTransformm.childCount; i++)
        {
            waypoints.Add(parentTransformm.GetChild(i));
        }
    }

    public Transform GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Count)];
    }

}
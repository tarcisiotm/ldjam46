using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectDataController : MonoBehaviour
{
    public enum SpawnedType {
        small,
        medium,
        big,
        water,
        nearWater
    }

    [SerializeField] SpawnedType spawnedType = SpawnedType.small;
}
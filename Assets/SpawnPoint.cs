using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] internal SpawnType spawnType;
}

public enum SpawnType
{
    Small,
    Big,
}
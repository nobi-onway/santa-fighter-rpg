using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _skeleton;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSkeleton), 1, 5);
    }

    private void SpawnSkeleton()
    {
        Instantiate(_skeleton, transform.position, Quaternion.identity);
    }
}

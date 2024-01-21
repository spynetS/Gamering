using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class C_terrainGen : MonoBehaviour
{

    [SerializeField] private int chunkSize;
    [SerializeField] private int detailPerChunk;
    [SerializeField] private float amplitude;
    [SerializeField] private float2 seed;

    void Start()
    {
        seed.x = UnityEngine.Random.Range((int)-5000, (int)5000);
        seed.y = UnityEngine.Random.Range((int)-5000, (int)5000);
    }

    void Update()
    {
    }
}

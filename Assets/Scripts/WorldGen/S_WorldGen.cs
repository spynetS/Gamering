using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class S_WorldGen : MonoBehaviour
{
    [SerializeField] private int chunkX;
    [SerializeField] private int chunkY;
    [SerializeField] private int seed;
    [SerializeField] private float amplitude;
    [SerializeField] private List<float> Magnitudes;
    [SerializeField] private List<Vector3> Vertices;
    [SerializeField] private List<int> Tris;

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Mesh _tMesh;

    private void GenerateMagnitude(Vector2 _chunkVec)
    {
        float delta = 0.04f;
        for (int i = chunkX * Convert.ToInt32(Math.Round(_chunkVec.x)); i < chunkX * ((float)Math.Round(_chunkVec.x) + ((chunkX >= 0) ? 1 : -1)); i++)
        {
            for (int j = chunkY * Convert.ToInt32(Math.Round(_chunkVec.y)); j < chunkY * ((float)Math.Round(_chunkVec.y) + ((chunkY >= 0) ? 1 : -1)); j++)
            {
                float pVal = Mathf.PerlinNoise(
                    (float)(i * delta) + seed + ((float)Math.Round(_chunkVec.x) * chunkX),
                    (float)(j * delta) + seed + ((float)Math.Round(_chunkVec.y) * chunkY)
                );
                Vector3 nVec = new Vector3(i * 4, 0, j * 4);

                Magnitudes.Add(pVal);
                nVec.y = 1 - (pVal * amplitude);
                Vertices.Add(nVec);
            }
        }
    }

    private void SetTriangles()
    {
        for (int x = 0; x < chunkX - 1; x++)
        {
            for (int y = 0; y < chunkY - 1; y++)
            {
                int index = y * chunkX + x;

                Tris.Add(index);
                Tris.Add(index + chunkX);
                Tris.Add(index + 1);

                Tris.Add(index + 1);
                Tris.Add(index + chunkX);
                Tris.Add(index + chunkX + 1);
            }
        }
    }

    public void GenerateTerrainMesh(Vector2 _v)
    {
        Vertices.Clear();
        Tris.Clear();
        
        GenerateMagnitude(_v);
        SetTriangles();

        _tMesh = new Mesh();
        _tMesh.Clear();
        _tMesh.vertices = Vertices.ToArray();
        _tMesh.triangles = Tris.ToArray();
        _tMesh.RecalculateNormals();
        _tMesh.RecalculateBounds();

        _meshFilter.mesh = _tMesh;
    }

    [Header("Player")]
    [SerializeField] private S_PlayerManager swg;

    private void Start()
    {
        swg = GetComponent<S_PlayerManager>();
        GenerateTerrainMesh(swg.chunkPos);

        /* Texture2D grayImage = new Texture2D(chunkX, chunkY);
        List<Color> pixels = new List<Color>();

        for (int i = 0; i < Magnitudes.Count; i++)
        {
            pixels.Add(new Color(Magnitudes[i], Magnitudes[i], Magnitudes[i]));
        }
        grayImage.SetPixels(pixels.ToArray());

        byte[] bytes = grayImage.EncodeToPNG();
        System.IO.File.WriteAllBytes("T:\\Unity\\Gamering\\Assets\\Scripts\\WorldGen\\output.png", bytes); */
    }

    private void LateUpdate()
    {
        print(swg.chunkPos);
    }
}

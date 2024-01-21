using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Compute_TerrainGen : MonoBehaviour
{

    public ComputeShader _computeShader;
    public RenderTexture noiseTexture;

    [SerializeField] private int noise_w = 512;
    [SerializeField] private int noise_h = 512;
    [SerializeField] private int noise_d = 24;

    void Start()
    {
        noiseTexture = new RenderTexture(noise_w, noise_h, 0, RenderTextureFormat.RFloat);
        noiseTexture.enableRandomWrite = true;
        noiseTexture.Create();

        _computeShader.SetFloat("res", noise_w);
        _computeShader.SetTexture(0, "Result", noiseTexture);
        _computeShader.Dispatch(0, noise_w / 8, noise_h / 8, 1);

        Texture2D texture2D = new Texture2D(noise_w, noise_h, TextureFormat.RGBA32, false);
        RenderTexture.active = noiseTexture;
        texture2D.ReadPixels(new Rect(0, 0, noise_w, noise_h), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        byte[] bytes = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes($"T:\\Unity\\Gamering\\Assets\\Scripts\\WorldGen\\noiseGen.png", bytes);
    }

    private static Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }
}

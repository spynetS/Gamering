using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerManager : MonoBehaviour
{
    public GameObject _player;
    public Vector2 chunkPos = new Vector2(0, 0);

    [Header("Chunk Information")]
    [SerializeField] private int chunkX;
    [SerializeField] private int chunkY;
    [SerializeField] private Vector3 chunkSize;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        chunkSize = new Vector3(chunkX, 1, chunkY);
    }

    void updatePlayerChunkPos()
    {
        Vector3 _vec = new Vector3
        (
            _player.transform.position.x / chunkSize.x,
            _player.transform.position.y / chunkSize.y,
            _player.transform.position.z / chunkSize.z
        );
        chunkPos.x = _vec.x;
        chunkPos.y = _vec.z;
    }

    private void LateUpdate()
    {
        updatePlayerChunkPos();
    }
}

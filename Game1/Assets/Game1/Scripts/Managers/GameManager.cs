using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StageManager stageManager;
    public GameObject blockPrefab;
    public GameObject obstacleBlockPrefab;

    // public CameraScript camera;
    public Transform cameratransform;

    private float spawnTime = 1f;
    private float lastSpawnTime = 0;

    public bool isGameOver = false;

    void Awake() {
        instance = this;

        InitObjectPool();
        stageManager.Init();
    }

    // Start is called before the first frame update
    void InitObjectPool()
    {
        ObjectPoolContainer.Instance.Create("Block", blockPrefab, 20);
        ObjectPoolContainer.Instance.Create("ObstacleBlock", obstacleBlockPrefab, 10);
    }
    
    public void EndGame()
    {
        isGameOver = true;
    }
}

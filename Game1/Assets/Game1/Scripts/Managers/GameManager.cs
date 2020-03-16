using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StageManager stageManager;
    public GameObject blockPrefab;

    private float spawnTime = 1f;
    private float lastSpawnTime = 0;

    public RectTransform canvasTransform;
    // private float[,] canvasPos;
    // private float bottom;
    void Awake() {
        instance = this;
        InitObjectPool();
        stageManager.Init();
    }

    // Start is called before the first frame update
    void InitObjectPool()
    {
        ObjectPoolContainer.Instance.Create("Block", blockPrefab, 20);

        // float width = canvasTransform.rect.width;
        // float pos = width / 2 * Mathf.Cos(45 * Mathf.PI / 180);
        // float[,] canvasPos = new float[2, 2] {{-pos, -pos}, {pos, pos}};
        // bottom = 2 * Mathf.Pow(pos, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastSpawnTime + spawnTime) {
            
            // 화면 밖으로 벗어나면 반대로?


            lastSpawnTime = Time.time;
        }
    }

    // bool Check(Vector3 blockPosition) {
    //     float side = Mathf.Pow(blockPosition.x, 2) + Mathf.Pow(blockPosition.z, 2);
    //     float height = ;
    //     return false;
    // }
}

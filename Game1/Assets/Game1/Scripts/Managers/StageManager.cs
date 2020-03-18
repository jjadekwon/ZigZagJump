using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public BallController ballController;
    public GameObject blockPrefab;

    public int blockCount = 20;
    public int limitX = -3;
    public int limitZ = 3;

    private float[,] dirs = {{-1f, 0}, {0, 1f}};
    private Vector3 blockPosition = new Vector3(0, 0, 0);

    public void Init()
    {
        instance = this;

        int dirIdx = Random.Range(0, 2);   // 0, 1

        // ball 시작 방향 설정
        ballController.startDir = new Vector3(blockPosition.x + dirs[dirIdx, 0],
                                              blockPosition.y,
                                              blockPosition.z + dirs[dirIdx, 1]);

        for (int i = 0; i < blockCount; i++)
        {
            if (i < 4)
            {
                GameObject block = ObjectPoolContainer.Instance.Pop("Block");
                block.SetActive(true);
                block.transform.position = blockPosition;
                blockPosition = new Vector3(blockPosition.x + dirs[dirIdx, 0],
                                            blockPosition.y,
                                            blockPosition.z + dirs[dirIdx, 1]);
            }
            else
            {
                SpawnBlock();
            }
        }
    }

    public void SpawnBlock ()
    {
        GameObject block = ObjectPoolContainer.Instance.Pop("Block");
        block.SetActive(true);
        block.transform.position = blockPosition;

        int idx = 0;
        if (blockPosition.x + blockPosition.z - 1 < limitX) {
            idx = 1;
        }
        else if (blockPosition.x + blockPosition.z + 1 > limitZ) {
            idx = 0;
        }
        else {
            idx = Random.Range(0, 2);   // 0, 1
        }

        blockPosition = new Vector3(blockPosition.x + dirs[idx, 0], 
                                    blockPosition.y,
                                    blockPosition.z + dirs[idx, 1]);
    }

    void OnDestroy ()
    {
        instance = null;
    }
}

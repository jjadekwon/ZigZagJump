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

    private float[,] dir = {{-1f, 0}, {0, 1f}};
    private Vector3 blockPosition = new Vector3(0, 0, 0);

    public void Init()
    {
        instance = this;
        int d = Random.Range(0, 2);   // 0, 1
        ballController.startDir = new Vector3(blockPosition.x + dir[d, 0], blockPosition.y, blockPosition.z + dir[d, 1]);

        for (int i = 0; i < blockCount; i++) {
            GameObject block = ObjectPoolContainer.Instance.Pop("Block");
            block.SetActive(true);
            block.transform.position = blockPosition;

            if (i < 4)
            {
                blockPosition = new Vector3(blockPosition.x + dir[d, 0], blockPosition.y, blockPosition.z + dir[d, 1]);
            }
            else
            {
                //Debug.Log(blockPosition);
                SpawnBlock();
            }    



        }

        
                                    
    }

    public void SpawnBlock ()
    {
        GameObject block = ObjectPoolContainer.Instance.Pop("Block");
        block.SetActive(true);
        block.transform.position = blockPosition;

        int idx = Random.Range(0, 2);   // 0, 1

        float nextX = blockPosition.x + dir[idx, 0];
        float nextZ = blockPosition.z + dir[idx, 1];

        if (nextX + nextZ < limitX || nextX + nextZ > limitZ) {
            if (idx == 0) idx = 1;
            else idx = 0;
            blockPosition = new Vector3(blockPosition.x + dir[idx, 0],
                                        blockPosition.y,
                                        blockPosition.z + dir[idx, 1]);
        }
        else {
            blockPosition = new Vector3(nextX,
                                        blockPosition.y,
                                        nextZ);
        }
    }

    void OnDestroy ()
    {
        instance = null;
    }
    
}

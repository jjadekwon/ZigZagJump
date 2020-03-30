using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public BallController ballController;
    public GameObject blockPrefab;
    public GameObject obstacleBlockPrefab;

    public int blockCount = 20;
    public int limitX = -3;
    public int limitZ = 3;

    private float[,] dirs = {{-1f, 0}, {0, 1f}};
    private Vector3 blockPosition = new Vector3(0, 0, 0);
    private float obstacleBlockPer = 50;
    private bool needObstacle = false;


    private int prevIdx;

    public void Init()
    {
        instance = this;

        int dirIdx = Random.Range(0, 2);   // 0, 1

        dirIdx = 1; // test
        prevIdx = dirIdx;

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
        // 장애물블럭은 연속으로 나올 수 없다
        // 장애물블럭은 모서리에 나올 수 없다

        GameObject block;
        bool prevIsObstacle = false;
        if (needObstacle == true)
        {
            needObstacle = false;
            prevIsObstacle = true;
            block = ObjectPoolContainer.Instance.Pop("ObstacleBlock");
        }
        else
        {
            block = ObjectPoolContainer.Instance.Pop("Block");
        }

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
            if (prevIdx == idx && prevIsObstacle == false )
            {
                if ((blockPosition.x + dirs[idx, 0]) + (blockPosition.z + dirs[idx, 1]) != limitX &&
                    (blockPosition.x + dirs[idx, 0]) + (blockPosition.z + dirs[idx, 1]) != limitZ
                )
                {
                    float ran = Random.Range(0f, 100f);
                    if (ran <= obstacleBlockPer)
                    {
                        Debug.Log("prevIdx :" + prevIdx + ", idx :" + idx);
                        needObstacle = true;
                    }
                }
            }
            else if (prevIsObstacle == true)
            {
                Debug.Log("prev Obstacle");
                idx = prevIdx;
            }
        }

        blockPosition = new Vector3(blockPosition.x + dirs[idx, 0], 
                                    blockPosition.y,
                                    blockPosition.z + dirs[idx, 1]);
        prevIdx = idx;
    }

    void OnDestroy ()
    {
        instance = null;
    }
}

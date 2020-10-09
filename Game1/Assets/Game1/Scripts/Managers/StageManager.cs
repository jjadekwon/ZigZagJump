using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public BallController ballController;

    private int blockCount = 25;    // 시작 시 생성되는 블럭 수
    public int limitX = -3;
    public int limitZ = 3;

    private float[,] dirs = {{-1f, 0}, {0, 1f}};
    private Vector3 blockPosition = new Vector3(0, 0, 0);
    private float obstacleBlockPer = 50;    // 장애물 블럭 등장 확률
    private bool needObstacle = false;

    private int prevIdx;
    public int checkPointTerm = 50;     // 체크포인트 체크 단위

    private int spawnBlockCount = 0;    // 생성된 블럭 수

    private int itemPer = 10;           // 아이템 등장 확률

    public void Init()
    {
        instance = this;

        int dirIdx = Random.Range(0, 2);   // 0, 1

        prevIdx = dirIdx;

        // ball 시작 방향 설정
        ballController.dir = new Vector3(blockPosition.x + dirs[dirIdx, 0],
                                         blockPosition.y,
                                         blockPosition.z + dirs[dirIdx, 1]);

        for (int i = 0; i < blockCount; i++)
        {
            // 초기 4개 블럭은 같은 방향으로 생성
            if (i < 4)
            {
                GameObject block = ObjectPoolContainer.Instance.Pop("Block");
                block.SetActive(true);
                block.transform.position = blockPosition;
                blockPosition = new Vector3(blockPosition.x + dirs[dirIdx, 0],
                                            blockPosition.y,
                                            blockPosition.z + dirs[dirIdx, 1]);
                
                spawnBlockCount++;
            }
            else
            {
                SpawnBlock();
            }
        }
    }

    public void SpawnBlock ()
    {
        GameObject block;
        bool prevIsObstacle = false;
        bool needItem = false;
        
        // 체크포인트 블록 생성
        if ((spawnBlockCount + 1) % checkPointTerm == 0) {
            block = ObjectPoolContainer.Instance.Pop("CheckPointBlock");
            needObstacle = false;
        }
        // 장애물 블록 생성
        else if (needObstacle == true)
        {
            needObstacle = false;
            prevIsObstacle = true;
            block = ObjectPoolContainer.Instance.Pop("ObstacleBlock");
        }
        // 일반 블록 생성
        else
        {
            block = ObjectPoolContainer.Instance.Pop("Block");

            float ran = Random.Range(0f, 100f);
            if (ran <= itemPer) needItem = true;
        }

        block.SetActive(true);
        block.transform.position = blockPosition;

        // 블럭 회전
        if (prevIdx == 0) block.transform.rotation = Quaternion.Euler(0, 90, 0);
        else block.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (needItem) {
            var item = ObjectPoolContainer.Instance.Pop("Item");
            item.SetActive(true);
            item.transform.position = new Vector3(blockPosition.x, blockPosition.y + 1, blockPosition.z);
        }

        int idx = 0;
        // 한 방향 최대 개수가 넘어가면 반대 방향으로 생성
        if (blockPosition.x + blockPosition.z - 1 < limitX) {
            idx = 1;
        }
        else if (blockPosition.x + blockPosition.z + 1 > limitZ) {
            idx = 0;
        }
        else {
            idx = Random.Range(0, 2);   // 0, 1
            if (prevIdx == idx && prevIsObstacle == false)
            {
                // 바깥 모서리가 아닌 경우 장애물블럭 생성 가능
                if ((blockPosition.x + dirs[idx, 0]) + (blockPosition.z + dirs[idx, 1]) != limitX &&
                    (blockPosition.x + dirs[idx, 0]) + (blockPosition.z + dirs[idx, 1]) != limitZ
                )
                {
                    float ran = Random.Range(0f, 100f);
                    if (ran <= obstacleBlockPer)
                    {
                        needObstacle = true;
                    }
                }
            }
            // 이전 블럭이 장애물 블럭인 경우 같은 방향으로 기본 블럭 생성
            else if (prevIsObstacle == true)
            {
                idx = prevIdx;
            }
        }

        blockPosition = new Vector3(blockPosition.x + dirs[idx, 0], 
                                    blockPosition.y,
                                    blockPosition.z + dirs[idx, 1]);
        prevIdx = idx;

        spawnBlockCount++;
    }

    void OnDestroy ()
    {
        instance = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour
{

    void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag("Block"))
        {
            col.gameObject.SetActive(false);
            ObjectPoolContainer.Instance.Return(col.gameObject);
            StageManager.instance.SpawnBlock();

        }

    }
    
}

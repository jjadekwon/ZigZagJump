using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChecker : MonoBehaviour
{
    void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag("Block") && !GameManager.instance.isGameOver)
        {
            BlockInfo info = col.GetComponent<BlockInfo>();
            if (info.isScoreCheck == false)
            {
                info.isScoreCheck = true;
                GameManager.instance.UpdateScore(1);
            }
            //col.transform.localScale = new Vector3(1,1.5f,1);
        }
    }
}

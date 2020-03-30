using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform ballTr;

    public Transform targetTransform;
    private Vector3 currentPosition;
    private Quaternion currentEulerAngle;
    void Update()
    {
        if (GameManager.instance.isGameOver)
        {
            transform.rotation = currentEulerAngle;
            transform.position = currentPosition;
            Debug.Log("die pos :" + transform.position + ",rot :" + transform.eulerAngles);
        }
    }

    public void SetPos()
    {
        currentPosition = transform.position;
        currentEulerAngle = transform.rotation;


    }
}

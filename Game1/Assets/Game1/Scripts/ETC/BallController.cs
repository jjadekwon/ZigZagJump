using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody ballRigidbody;
    public float speed = 10f;

    Vector3 left = new Vector3(-1, 0, 0);
    Vector3 right = new Vector3(0, 0, 1);

    public Vector3 startDir;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballRigidbody.velocity = startDir * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 dir;

            Vector3 normalized = ballRigidbody.velocity.normalized;
            Debug.Log(normalized);

            if (Math.Round(normalized.z) == 1) dir = left;
            else dir = right;

            ballRigidbody.velocity = dir * speed;
            ballRigidbody.angularVelocity = Vector3.zero;   // 각속도 벡터

            // ballRigidbody.AddForce(dir * speed);
        }
        transform.eulerAngles = Vector3.zero;
        //Debug.Log("ff");
    }
}

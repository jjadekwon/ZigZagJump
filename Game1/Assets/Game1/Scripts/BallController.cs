using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody ballRigidbody;
    float speed = 150f;

    Vector3 left = new Vector3(-1, 0, 0);
    Vector3 right = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballRigidbody.AddForce(right * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 dir;

            Vector3 normalized = ballRigidbody.velocity.normalized;
            
            if (Math.Round(normalized.z) == 1) dir = left;
            else dir = right;

            Debug.Log("dir : " + dir);

            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.AddForce(dir * speed);
        }
    }
}

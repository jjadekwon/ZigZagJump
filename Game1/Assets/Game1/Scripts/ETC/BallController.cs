using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform cameraTr;
    public Transform emptyParentTr;

    Rigidbody ballRigidbody;
    public float speed = 10f;

    public float jumpSpeed = 1600f;

    private float firstYPosition;

    Vector3 left = new Vector3(-1, 0, 0);
    Vector3 right = new Vector3(0, 0, 1);

    public Vector3 startDir;

    int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballRigidbody.velocity = startDir * speed;

        firstYPosition = transform.position.y - 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver) return;

        if (Input.GetMouseButtonDown(0)) {
            Vector3 normalized = ballRigidbody.velocity.normalized;

            double roundY = Math.Round(normalized.y, 2);
            if (roundY > -0.02 && roundY < 0.02) {
                RaycastHit hit;
                Physics.Raycast(transform.position, normalized, out hit);

                // 점프
                if (hit.collider != null)
                {
                    ballRigidbody.AddForce(new Vector3(0, jumpSpeed, 0));

                    // 다시 뒤로 돌아올 때는..?
                }
                // 방향 전환
                else
                {
                    if (Math.Round(normalized.z) == 1) startDir = left;
                    else startDir = right;

                    ballRigidbody.velocity = startDir * speed;
                    ballRigidbody.angularVelocity = Vector3.zero;   // 각속도 벡터
                }
            }
        }

        ballRigidbody.velocity = startDir * speed;
        transform.eulerAngles = Vector3.zero;

        // 공이 아래로 떨어지는 경우
        if (transform.position.y < firstYPosition) {
            GameManager.instance.EndGame();

            cameraTr.SetParent(emptyParentTr);
        }

        //Debug.Log("vel :" + ballRigidbody.velocity);
    }
}

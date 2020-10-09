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

    private float jumpCheck;

    Vector3 left = new Vector3(-1, 0, 0);
    Vector3 right = new Vector3(0, 0, 1);

    public Vector3 dir;

    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    public void GameStart ()
    {
        ballRigidbody.velocity = dir * speed;
        firstYPosition = transform.position.y - 0.3f;
        jumpCheck = firstYPosition - 0.02f;
    }

    void Update()
    {
        // 게임 종료
        if (GameManager.instance.isGameOver) return;

        // 마찰로인한 속도 감소때문에 리셋
        ballRigidbody.velocity = new Vector3(dir.x * speed, ballRigidbody.velocity.y, dir.z * speed);
        
        // 회전 금지
        transform.eulerAngles = Vector3.zero;

        // 공이 아래로 떨어지는 경우
        if (transform.position.y < firstYPosition) {
            GameManager.instance.EndGame();
            cameraTr.SetParent(emptyParentTr);  // 카메라 고정
        }
    }

    void OnCollisionEnter (Collision col) {
        // 게임 종료
        if (GameManager.instance.isGameOver) return;

        // 장애물 충돌
        if (col.collider.CompareTag("Obstacle")) {
            transform.eulerAngles = Vector3.zero;
            GameManager.instance.EndGame();
            cameraTr.SetParent(emptyParentTr);
        }

        // 아이템 획득
        if (col.collider.CompareTag("Item")) {
            SoundManager.PlaySFX(SfxType.Item);

            // 1점 증가
            GameManager.instance.UpdateItemScore(1);

            col.gameObject.SetActive(false);
            ObjectPoolContainer.Instance.Return(col.gameObject);
        }
    }

    public void OnButtonClick() {
        // 게임 종료
        if (GameManager.instance.isGameOver) return;
        
        Vector3 normalized = ballRigidbody.velocity.normalized;
        double roundY = Math.Round(normalized.y, 2);

        // 점프를 하는 중이 아닌 경우
        if (roundY > -jumpCheck && roundY < jumpCheck) {
            RaycastHit hit;
            Physics.Raycast(transform.position, normalized, out hit);

            // 가는 길에 장애물이 있는 경우 점프
            if (hit.collider != null && !hit.collider.CompareTag("Item"))
            {
                ballRigidbody.AddForce(new Vector3(0, jumpSpeed, 0));
                SoundManager.PlaySFX(SfxType.Jump);
            }
            // 방향 전환
            else
            {
                SoundManager.PlaySFX(SfxType.Turn);

                if (Math.Round(normalized.z) == 1) dir = left;
                else dir = right;

                ballRigidbody.velocity = dir * speed;
                ballRigidbody.angularVelocity = Vector3.zero;   // 각속도 벡터
            }
        }
    }
}

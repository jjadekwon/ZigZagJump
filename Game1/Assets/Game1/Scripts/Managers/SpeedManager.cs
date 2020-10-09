using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager instance;

    public float speed;
    private float maxSpeed = 1.7f;  // 최대 스피드
    public int speedUp = 100;

    void Awake()
    {
        instance = this;
        speed = 1;
        Time.timeScale = speed;
    }

    public void SetSpeed (int score)
    {
        speed = 1 + (score / speedUp) * 0.1f;
        if (speed > maxSpeed) speed = maxSpeed;
        // speed = 1f;

        Time.timeScale = speed;
    }

    void OnDestroy()
    {
        instance = null;
    }
}

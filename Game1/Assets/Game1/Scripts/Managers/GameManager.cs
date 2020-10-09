using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StageManager stageManager;
    public BallController ballController;
    public GameObject blockPrefab;
    public GameObject obstacleBlockPrefab;
    public GameObject checkPointBlockPrefab;

    public GameObject itemPrefab;

    // public CameraScript camera;
    public Transform cameratransform;
    public Image curtainImage;
    public GameObject gameStartBtn;
    public GameObject gamePauseBtn;

    public bool isGameOver = true;

    private int score;
    public Text scoreText;
    private int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value == 0 ? 1 : value;
            scoreText.text = score.ToString();
        }
    }
    public Text checkPointText;
    public Text bestScoreText;
    public Text itemScoreText;
    private int checkPoint;
    private int bestScore;
    private int BestScore
    {
        get
        {
            return bestScore;
        }
        set
        {
            bestScore = value;
            bestScoreText.text = bestScore.ToString();
        }
    }
    private int itemScore;
    private int ItemScore
    {
        get
        {
            return itemScore;
        }
        set
        {
            itemScore = value;
            itemScoreText.text = "+ " + itemScore;
        }
    }

    private Material blockMaterial;
    private Color[] colors = {Color.yellow, Color.green, Color.blue, Color.magenta, Color.cyan, Color.gray, Color.red};

    private int colorIdx = 0;

    void Awake() {
        instance = this;

        InitObjectPool();               // 블럭, 아이템 인스턴스 생성
        stageManager.Init();            // 스테이지 설정

        BackkeyManager.isBlock = true;  // Back key 동작 막기

        // 게임 실행과 동시에 화면이 밝아지게 설정
        curtainImage.gameObject.SetActive(true);
        curtainImage.DOFade(0, 1).OnComplete(() => {
            curtainImage.gameObject.SetActive(false);
            BackkeyManager.isBlock = false;
        });

        // 점수 설정
        Score = 1;
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        checkPoint = PlayerPrefs.GetInt("CheckPoint", 0);
        checkPointText.text = (checkPoint + stageManager.checkPointTerm).ToString();

        // 블럭 색상 설정
        blockMaterial = Resources.Load<Material>("Materials/Block");
        blockMaterial.color = colors[colorIdx++];
    }

    void InitObjectPool()
    {
        ObjectPoolContainer.Instance.Create("Block", blockPrefab, 25);
        ObjectPoolContainer.Instance.Create("ObstacleBlock", obstacleBlockPrefab, 10);
        ObjectPoolContainer.Instance.Create("CheckPointBlock", checkPointBlockPrefab, 5);
        ObjectPoolContainer.Instance.Create("Item", itemPrefab, 10);
    }
    
    public void UpdateScore (int add)
    {
        // 점수 업데이트
        Score += add;
        if (Score > BestScore) BestScore = Score;

        // 게임 속도 설정
        SpeedManager.instance.SetSpeed(Score);

        // 체크포인트 업데이트 및 블럭 색상 변경
        if (Score % stageManager.checkPointTerm == 0)
        {
            if (checkPoint < Score)
            {
                PlayerPrefs.SetInt("CheckPoint", Score);
                checkPointText.text = (Score + stageManager.checkPointTerm).ToString();
            }

            blockMaterial.DOColor(colors[colorIdx++], 1f);

            if (colorIdx >= colors.Length) colorIdx = 0;
        }
    }

    // 아이템 점수 업데이트
    public void UpdateItemScore (int add)
    {
        ItemScore += add;
    }

    public void EndGame()
    {
        SoundManager.PlaySFX(SfxType.Die);
        
        isGameOver = true;
        gamePauseBtn.SetActive(false);
        StartCoroutine(CoEndGame());
        //StopCoroutine("CoScore");

        PlayerPrefs.SetFloat("Speed", SpeedManager.instance.speed);
    }

    IEnumerator CoEndGame ()
    {
        yield return new WaitForSeconds(1);

        // 결과창 팝업
        PopupContainer.CreatePopup(PopupType.ResultPopup).Init(Score + ItemScore);
    }

    public void Retry ()
    {
        curtainImage.gameObject.SetActive(true);
        curtainImage.DOFade(1, 1).OnComplete( () => 
        {
            SceneManager.LoadScene("Game");
        });
    }

    public void OnPlayButtonClick ()
    {
        PopupContainer.CreatePopup(PopupType.StartPopup).Init();
    }

    public void GameStart(bool jumpStart) {
        isGameOver = false;
        ballController.GameStart();
        gameStartBtn.SetActive(false);
        gamePauseBtn.SetActive(true);

        // 체크포인트부터 시작하는 경우
        if (jumpStart) {
            checkPoint = PlayerPrefs.GetInt("CheckPoint", 0);
            Score = checkPoint;
            checkPointText.text = (checkPoint + stageManager.checkPointTerm).ToString();
            SpeedManager.instance.SetSpeed(Score);
        }
        //StartCoroutine("CoScore");
    }

    // IEnumerator CoScore ()
    // {
    //     float checkTime = 0;
    //     while (true)
    //     {
    //         yield return new WaitForFixedUpdate();
    //         checkTime += Time.fixedDeltaTime;
    //         if (checkTime >= 1)
    //         {
    //             checkTime -= 1;
    //             UpdateScore(1);
    //         }
    //     }
    // }


    public void OnPauseButtonClick ()
    {
        PopupContainer.CreatePopup(PopupType.PausePopup).Init();
    }
}

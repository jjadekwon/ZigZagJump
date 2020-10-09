using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPopup : BasePopup
{
    public Text checkPointScoreText;

    public override void Init(int id = -1)
    {
        base.Init();

        checkPointScoreText.text = PlayerPrefs.GetInt("CheckPoint", 0).ToString();
    }

    public void OnYesButtonClick ()
    {
        // AdsManager.instance.WatchAd(() => 
        // {
        //     base.Close();
        //     GameManager.instance.GameStart(true);
        // });
        base.Close();
        GameManager.instance.GameStart(true);
    }
    public void OnNoButtonClick ()
    {
        base.Close();
        GameManager.instance.GameStart(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackkeyManager : MonoBehaviour
{
    static Scene scene;
    public static bool isBlock;

    // Scene이 로드되기 전에 한번 호출됨
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init ()
    {
        GameObject obj = new GameObject("BackkeyManager");
        obj.AddComponent<BackkeyManager>();
        DontDestroyOnLoad(obj);

        // 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoadComplete;
    }

    // 현재 씬 등록 (씬마다 동작을 다르게 할 수 있음)
    static void OnSceneLoadComplete (Scene scene, LoadSceneMode mode)
    {
        BackkeyManager.scene = scene;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isBlock == false)
        {
            switch (scene.name)
            {
                case "Game":
                    // 활성화된 팝업이 없는 경우 일시정지창 팝업
                    if (PopupContainer.GetActivatedPopup() == null)
                    {
                        PopupContainer.CreatePopup(PopupType.PausePopup).Init();
                    }
                    // 활성화된 팝업이 존재하는 경우 팝업 종료
                    else
                    {
                        BasePopup popup = PopupContainer.GetActivatedPopup();
                        ResultPopup resultPopup = popup as ResultPopup;
                        if (resultPopup == null)
                        {
                            popup.Close();
                        }

                    }
                    break;
            }
        }
    }
}

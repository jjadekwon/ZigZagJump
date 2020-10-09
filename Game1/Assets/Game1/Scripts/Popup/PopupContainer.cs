using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupContainer : MonoBehaviour
{
    // 현재 활성화 되어있는 팝업 리스트
    private static List<BasePopup> popupList = new List<BasePopup>();
    private static Transform canvasTr;

    // RuntimeInitializeOnLoadMethod : 게임이 시작될 때 자동으로 해당 함수 호출됨
    // BeforeSceneLoad : 모든 스크립트들의 Awake보다 먼저 호출
    // AfterSceneLoad : 모든 스크립트들의 Awake와 Start 사이에 호출
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init ()
    {
        SceneManager.sceneLoaded += OnSceneLoadComplete;
    }

    static void OnSceneLoadComplete (Scene scene, LoadSceneMode mode)
    {
        popupList = new List<BasePopup>();
        GameObject canvasObj = GameObject.Find("PopupParent");
        canvasTr = canvasObj.transform;
    }

    // PopupParent 하위로 팝업 생성
    public static BasePopup CreatePopup (PopupType popupType)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Popup/" + popupType.ToString());
        if (canvasTr == null)
        {
            GameObject canvasObj = GameObject.Find("PopupParent");
            canvasTr = canvasObj.transform;
        }
        GameObject popupObj = Instantiate(prefab, canvasTr);
        return popupObj.GetComponent<BasePopup>();
    }

    // 팝업리스트에 등록
    // isOverlay == true : 기존 팝업 위에 생성, false : 기존 팝업을 끄고 생성
    public static void Pop (BasePopup basePopup, bool isOverlay = true)
    {
        // 이미 활성화되어있는 팝업이 존재하고, 그 팝업을 안보이게 하고싶다면,
        if (isOverlay == false && popupList.Count > 0)
        {
            popupList[popupList.Count-1].gameObject.SetActive(false);
        }
        popupList.Add(basePopup);
    }

    // 상위 팝업 Close
    public static void Close ()
    {
        popupList.RemoveAt(popupList.Count-1);
        if (popupList.Count > 0)
        {
            popupList[popupList.Count-1].gameObject.SetActive(true);
        }
    }

    // 상위 팝업 반환
    public static BasePopup GetActivatedPopup ()
    {
        if (popupList.Count == 0)
        {
            return null;
        }
        return popupList[popupList.Count-1];
    }
}

public enum PopupType
{
    StartPopup,
    PausePopup,
    ResultPopup,
    ExitPopup,
}

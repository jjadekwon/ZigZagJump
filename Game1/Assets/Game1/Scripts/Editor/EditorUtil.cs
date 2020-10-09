using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorUtil
{
    [MenuItem("Assets/Reset PlayerPrefs")]
    static void ResetPlayerPrefs ()
    {
        PlayerPrefs.DeleteAll();
    }
}

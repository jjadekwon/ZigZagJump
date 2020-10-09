using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public virtual void Init (int id = -1)
    {
        PopupContainer.Pop(this);
    }

    public virtual void Close ()
    {
        PopupContainer.Close();
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgendaEditItemRowController : MonoBehaviour
{
    [SerializeField]
    TMP_InputField contentsInputField;

    Func<AgendaEditItemRowController, bool> onItemDeleteCallback;

    public void setCallback(Func<AgendaEditItemRowController, bool> _onItemDeleteCallback)
    {
        onItemDeleteCallback = _onItemDeleteCallback;
    }

    public void updateItemContents(string _contents)
    {
        contentsInputField.text = _contents;
    }

    public void onDeleteItemBtnClicked()
    {
        onItemDeleteCallback?.Invoke(this);
    }

    public string getContents()
    {
        return contentsInputField.text;
    }
}

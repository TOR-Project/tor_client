using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPopupController : MonoBehaviour
{
    public const string DISMISSING_TRIGGER = "dismissing";

    [SerializeField]
    Text contentsText;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    Text feedbackText;
    [SerializeField]
    Animator animator;

    Func<string, bool> callback;

    public void show(string _contents, string _initInputValue, TMP_InputField.ContentType _contentsType, Func<string, bool> _callback)
    {
        contentsText.text = _contents;
        feedbackText.text = "";
        inputField.text = _initInputValue;
        inputField.contentType = _contentsType;
        callback = _callback;

        gameObject.SetActive(true);
    }

    public void setFeedbackText(string _text)
    {
        feedbackText.text = _text;
    }

    public void setFeedbackText(string _text, Color32 _color)
    {
        feedbackText.text = _text;
        feedbackText.color = _color;
    }

    public void onConfirmButton()
    {
        callback?.Invoke(inputField.text);
    }

    public void dismiss()
    {
        animator.SetTrigger(DISMISSING_TRIGGER);
    }
}

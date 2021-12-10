using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using MedievalKingdomUI.Scripts.Window;

public class LoadingController : MonoBehaviour
{
    [SerializeField]
    AnimatedWindowController windowController;
    [SerializeField]
    GameObject titleWindow;
    [SerializeField]
    Slider slider;
    [SerializeField]
    LanguageTextController textController;
    [SerializeField]
    string findingCharacterTextKey;
    [SerializeField]
    string loadingCharacterTextKey;

    internal void setMaxCharacterCount(int _max)
    {
        if (slider.maxValue != _max * 2)
        {
            slider.maxValue = _max * 2;
        }
    }

    internal void updateFindingCharacter(int _prog)
    {
        if (textController.key != findingCharacterTextKey)
        {
            textController.key = findingCharacterTextKey;
            textController.onLanguageChanged();
        }

        slider.value = _prog + 1;
    }

    internal bool updateLoadingCharacter(int _idx)
    {
        if (textController.key != loadingCharacterTextKey)
        {
            textController.key = loadingCharacterTextKey;
            textController.onLanguageChanged();
        }

        slider.value = slider.maxValue / 2 + _idx + 1;
        return true;
    }

    internal void enterTitlePage()
    {
        windowController.OpenWindow(titleWindow);
    }
}
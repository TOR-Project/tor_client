using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

public class MapWindowController : MonoBehaviour
{
    [SerializeField]
    WindowController windowController;
    [SerializeField]
    GameObject titleWindow;

    [SerializeField]
    GameObject flagGameObject;
    [SerializeField]
    Image flagIcon;
    [SerializeField]
    Text flagTitle;
    [SerializeField]
    Text flagContents;
    [SerializeField]
    Animator flagAnimator;

    [SerializeField]
    Animator mapAnimator;
    [SerializeField]
    Button mapButton;
    [SerializeField]
    Text mapButtonText;

    [SerializeField]
    GameObject slidingPanel;
    [SerializeField]
    Animator slidingPanelAnimator;

    private string zoomOutTrigger = "zoomOut";
    private string dismissingTrigger = "dismissing";
    private string hidingTrigger = "hiding";


    public void showFlag(int _cid)
    {
        flagTitle.text = CountryManager.instance.getCountryName(_cid);
        flagContents.text = CountryManager.instance.getCountryExp(_cid);
        flagGameObject.SetActive(true);
        mapButtonText.text = LanguageManager.instance.getText("ID_BACK");
        flagIcon.sprite = CountryManager.instance.getBigFlagImage(_cid);
    }

    public void closeFlag()
    {
        mapButton.interactable = false;
        flagAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissFlag()
    {
        mapButton.interactable = true;
        flagGameObject.SetActive(false);
        mapButtonText.text = LanguageManager.instance.getText("ID_MAIN");
    }

    public void onMainButtonClicked()
    {
        if (flagGameObject.activeSelf)
        {
            closeFlag();
            mapAnimator.SetTrigger(zoomOutTrigger);
            if (slidingPanel.activeSelf)
            {
                slidingPanelAnimator.SetTrigger(hidingTrigger);
            }
        }
        else
        {
            windowController.OpenWindow(titleWindow);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWindowController : MonoBehaviour
{
    [SerializeField]
    WindowController windowController;
    [SerializeField]
    GameObject titleWindow;
    [SerializeField]
    Camera camera3d;

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
    Button mapButton;
    [SerializeField]
    Text mapButtonText;

    [SerializeField]
    GameObject slidingPanel;
    [SerializeField]
    Animator slidingPanelAnimator;

    [SerializeField]
    CastlePanelController castlePanelController;

    private string dismissingTrigger = "dismissing";
    private string hidingTrigger = "hiding";

    TileController lastSelectedTileController;


    public void showFlag(int _cid)
    {
        flagTitle.text = CountryManager.instance.getCountryName(_cid);
        flagContents.text = CountryManager.instance.getCountryExp(_cid);
        flagGameObject.SetActive(true);
        mapButtonText.text = LanguageManager.instance.getText("ID_BACK");
        flagIcon.sprite = CountryManager.instance.getBigFlagImage(_cid);
    }

    public void showCastlePanel(int _cid)
    {
        slidingPanel.SetActive(true);
        castlePanelController.setCastleId(_cid);
        castlePanelController.gameObject.SetActive(true);
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down " + Input.mousePosition);
            RaycastHit hit;
            Ray ray = camera3d.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("hit = " + hit);
                //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                if (hit.transform != null)
                {
                    TileController controller = hit.transform.GetComponentInParent<TileController>();
                    onTileSelected(controller);
                }
            }
        }
    }

    public void onTileSelected(TileController controller)
    {
        if (controller == lastSelectedTileController)
        {
            return;
        }

        GameObject selection = GameObject.FindGameObjectWithTag("Selection");
        if (selection != null)
        {
            Destroy(selection);
        }

        Instantiate(MapManager.instance.getSelectionPrefab(), controller.transform.position + new UnityEngine.Vector3(0, 0.201f * controller.getTileData().y, 0), UnityEngine.Quaternion.Euler(270, 30, 0), controller.transform);
        lastSelectedTileController = controller;

        showInfoPanel(controller);
    }

    private void showInfoPanel(TileController controller)
    {
        bool hasConstruction = controller.getTileData().constList.Count > 0;

        if (hasConstruction)
        {
            ConstructionsData data = controller.getTileData().constList[0];
            if (data.category.Equals("castle"))
            {
                showFlag(int.Parse(data.name));
                showCastlePanel(int.Parse(data.name));
            }
        } else
        {

        }
    }
}
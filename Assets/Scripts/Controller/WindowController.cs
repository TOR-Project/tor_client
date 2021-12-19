using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public GameObject initialActiveWindow;
    public GraphicRaycaster graphicRaycaster;
    public Animator animator;
    public GameObject loadingWindow;

    private LoadingController loadingController;

    [SerializeField]
    GlobalUIController globalUIController;

    private GameObject _activeWindow;
    private GameObject _windowToOpen;
    private GameObject _targetWindow;
    private static readonly int RunAnimation = Animator.StringToHash("RunAnimation");

    private void Awake()
    {
        _activeWindow = initialActiveWindow;
        loadingController = loadingWindow.GetComponent<LoadingController>();
    }

    public void OpenWindow(GameObject window)
    {
        _targetWindow = window;
        graphicRaycaster.enabled = true;
        _windowToOpen = loadingWindow;
        animator.SetTrigger(RunAnimation);
        Debug.Log("openWindow() loadingWindow");
    }

    private void OpenWindowAfterLoading()
    {
        graphicRaycaster.enabled = true;
        _windowToOpen = _targetWindow;
        animator.SetTrigger(RunAnimation);
        Debug.Log("openWindow() targetWindow");
    }

    public void Proceed()
    {
        Debug.Log("Proceed()");
        _activeWindow.SetActive(false);
        _activeWindow = _windowToOpen;

        if (_activeWindow == loadingWindow)
        {
            loadingWindow.SetActive(true);
            bool completed = loadingController.setupLoading(_targetWindow, OpenWindowAfterLoading);
            if (completed) { 
                loadingWindow.SetActive(false);
                _activeWindow = _targetWindow;
            }
        }

        if (_activeWindow == loadingWindow)
        {
            globalUIController.hideInfoPanel();
        }
        else
        {
            globalUIController.showInfoPanel();
        }

        _activeWindow.SetActive(true);
        graphicRaycaster.enabled = false;
    }
}
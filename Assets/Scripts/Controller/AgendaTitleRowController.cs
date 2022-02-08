using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgendaTitleRowController : MonoBehaviour
{
    public static Color STATE_COLOR_ING = new Color(200, 200, 0);
    public static Color STATE_COLOR_CANCELED = new Color(200, 0, 0);
    public static Color STATE_COLOR_FINISHED = new Color(0, 200, 0);

    [SerializeField]
    Text stateText;
    [SerializeField]
    Text titleText;
    [SerializeField]
    ImageLoadingComponent loadingImage;

    AgendaData agendaData;

    GovernanaceWindowController governanaceWindowController;

    private void OnEnable()
    {
        loadingImage.startLoading();
    }

    public void setGovernanceWindowController(GovernanaceWindowController _governanaceWindowController)
    {
        governanaceWindowController = _governanaceWindowController;
    }

    public void updateAgendaData(AgendaData _agendaData)
    {
        agendaData = _agendaData;
        titleText.text = _agendaData.title;
        bool isFinished = _agendaData.endBlock <= SystemInfoManager.instance.blockNumber;

        if (_agendaData.canceled)
        {
            stateText.text = LanguageManager.instance.getText("ID_PROGRESS_CANCELED");
            stateText.color = STATE_COLOR_CANCELED;
        } else if (isFinished)
        {
            stateText.text = LanguageManager.instance.getText("ID_PROGRESS_DONE");
            stateText.color = STATE_COLOR_FINISHED;
        } else
        {
            stateText.text = LanguageManager.instance.getText("ID_PROGRESS_ING");
            stateText.color = STATE_COLOR_ING;
        }
    }

    public void onClickButton()
    {
        if (governanaceWindowController != null)
        {
            governanaceWindowController.showAgenda(agendaData);
        }
    }
}

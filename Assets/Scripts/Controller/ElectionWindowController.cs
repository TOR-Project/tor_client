using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ElectionWindowController : MonoBehaviour, BlockNumberObserever
{
    [SerializeField]
    Text monarchTermText;
    [SerializeField]
    Text monarchRegistText;
    [SerializeField]
    Text monarchElectionText;
    [SerializeField]
    Text[] hidingTextListWhen0Th;
    [SerializeField]
    Text[] calendarTextList;
    [SerializeField]
    Slider calendarSlider;
    [SerializeField]
    RectTransform sliderRT;
    [SerializeField]
    RectTransform particleRT;
    [SerializeField]
    ParticleSystem particleSystem;
    [SerializeField]
    Color[] particleColor;


    private void OnEnable()
    {
        updateElectionInfo();

        SystemInfoManager.instance.addBlockNumberObserver(this);
    }

    private void OnDisable()
    {
        SystemInfoManager.instance.removeBlockNumberObserver(this);
    }

    private void updateElectionInfo()
    {
        int round = ElectionManager.instance.getElectionRound();
        int year = Utils.getTorYear(SystemInfoManager.instance.blockNumber);

        foreach (Text text in hidingTextListWhen0Th)
        {
            text.enabled = round > 0;
        }
        monarchTermText.text = string.Format(LanguageManager.instance.getText("ID_N_MONARCH_TERM"), round);
        monarchRegistText.text = string.Format(LanguageManager.instance.getText("ID_N_NEXT_MONARCH_REGIST"), round + 1);
        monarchElectionText.text = string.Format(LanguageManager.instance.getText("ID_N_NEXT_MONARCH_ELECTION"), round + 1);
        calendarTextList[0].text = string.Format(LanguageManager.instance.getText("ID_N_YEAR_SPRING"), year);
        calendarTextList[1].text = string.Format(LanguageManager.instance.getText("ID_N_YEAR_SUMMER"), year);
        calendarTextList[2].text = string.Format(LanguageManager.instance.getText("ID_N_YEAR_FALL"), year);
        calendarTextList[3].text = string.Format(LanguageManager.instance.getText("ID_N_YEAR_WINTER"), year);
        calendarTextList[4].text = string.Format(LanguageManager.instance.getText("ID_N_YEAR_SPRING"), year + 1);

        updateSlider();
    }

    public void onBlockNumberChanged(long _num)
    {
        updateSlider();
    }

   private void updateSlider()
    {
        float progress = ElectionManager.instance.getElectionProgressValue();
        calendarSlider.value = progress;
        float xPos = (sliderRT.sizeDelta.x - 10) * progress;
        particleRT.anchoredPosition = new Vector3(xPos + 5, -20);

        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = particleColor[(int)ElectionManager.instance.getElectionState()];
    }

}
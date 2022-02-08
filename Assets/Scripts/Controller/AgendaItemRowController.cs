using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgendaItemRowController : MonoBehaviour
{
    [SerializeField]
    float animatedTime = 1.5f;

    [SerializeField]
    Text contentsText;
    [SerializeField]
    Slider voteSlider;
    [SerializeField]
    Text voteCountText;
    [SerializeField]
    GameObject votePanel;
    [SerializeField]
    Toggle checkboxToggle;

    Predicate<AgendaItemRowController> resetToggleAction;

    private float desiredVoteNumber;
    private float initialVoteNumber;
    private float currentVoteNumber;

    private float desiredVoteMaxNumber;
    private float initialVoteMaxNumber;
    private float currentVoteMaxNumber;

    public void updateItem(string _contents, bool _isHideVoteInfo, int _voteCount, int _voteMax)
    {
        contentsText.text = _contents;

        initialVoteNumber = currentVoteNumber = voteSlider.value = 0;
        desiredVoteNumber = _voteCount;
        desiredVoteMaxNumber = initialVoteMaxNumber = currentVoteMaxNumber = voteSlider.maxValue = _voteMax;
        voteCountText.text = "0";

        votePanel.SetActive(!_isHideVoteInfo);
    }

    internal void setResetToggleAction(Predicate<AgendaItemRowController> _action)
    {
        resetToggleAction = _action;
    }

    public void setActiveToggle(bool _active)
    {
        checkboxToggle.gameObject.SetActive(_active);
    }

    public void onToggleChanged()
    {
        resetToggleAction(this);
    }

    public void resetToggle()
    {
        checkboxToggle.SetIsOnWithoutNotify(false);
    }

    public bool isOn()
    {
        return checkboxToggle.isOn;
    }

    public void updateMaxVoteCount(int _max)
    {
        desiredVoteMaxNumber = _max;
    }

    public void updateVoteCount(int _count)
    {
        desiredVoteNumber = _count;
    }

    private void Update()
    {
        if (currentVoteNumber != desiredVoteNumber)
        {
            float delta = (animatedTime * Time.deltaTime) * (desiredVoteNumber - initialVoteNumber);
            if (initialVoteNumber < desiredVoteNumber)
            {
                currentVoteNumber += delta;
                if (currentVoteNumber >= desiredVoteNumber)
                {
                    currentVoteNumber = desiredVoteNumber;
                }
            }
            else
            {
                currentVoteNumber += delta;
                if (currentVoteNumber <= desiredVoteNumber)
                {
                    currentVoteNumber = desiredVoteNumber;
                }
            }

            voteSlider.value = currentVoteNumber;
            voteCountText.text = ((int)currentVoteNumber).ToString();
        }

        if (currentVoteMaxNumber != desiredVoteMaxNumber)
        {
            float delta = (animatedTime * Time.deltaTime) * (desiredVoteMaxNumber - initialVoteMaxNumber);
            if (initialVoteMaxNumber < desiredVoteMaxNumber)
            {
                currentVoteMaxNumber += delta;
                if (currentVoteMaxNumber >= desiredVoteMaxNumber)
                {
                    currentVoteMaxNumber = desiredVoteMaxNumber;
                }
            }
            else
            {
                currentVoteMaxNumber += delta;
                if (currentVoteMaxNumber <= desiredVoteMaxNumber)
                {
                    currentVoteMaxNumber = desiredVoteMaxNumber;
                }
            }

            voteSlider.maxValue = currentVoteMaxNumber;
        }
    }
}

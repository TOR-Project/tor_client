using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class SlidingSliderController : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    float animatedTime = 1.5f;

    private float desiredValue;
    private float initialValue;
    private float currentValue;

    public void setValue(float _value)
    {
        initialValue = currentValue; 
        desiredValue = _value;
    }

    public void setMaxValue(float _value)
    {
        slider.maxValue = _value;
    }

    public void reset()
    {
        slider.value = initialValue = desiredValue = currentValue = 0;
    }

    private void Update()
    {
        if (currentValue != desiredValue)
        {
            float delta = (animatedTime * Time.deltaTime) * (desiredValue - initialValue);
            if (initialValue < desiredValue)
            {
                currentValue += delta;
                if (currentValue >= desiredValue)
                {
                    currentValue = desiredValue;
                }
            }
            else
            {
                currentValue += delta;
                if (currentValue <= desiredValue)
                {
                    currentValue = desiredValue;
                }
            }

            slider.value = currentValue;
        }
    }
}
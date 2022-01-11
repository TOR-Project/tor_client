using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class SlidingNumberController : MonoBehaviour
{
    [SerializeField]
    Text numberText;
    [SerializeField]
    float animatedTime = 1.5f;

    string prefix = "";
    string postfix = "";

    private BigInteger desiredNumber;
    private BigInteger initialNumber;
    private BigInteger currentNumber;

    public void setNumber(BigInteger _number)
    {
        initialNumber = currentNumber; 
        desiredNumber = _number;
    }

    public void setAdditionalString(string _prefix, string _postfix)
    {
        prefix = _prefix;
        postfix = _postfix;
    }

    private void Update()
    {
        if (currentNumber != desiredNumber)
        {
            BigInteger delta = new BigInteger((animatedTime * Time.deltaTime) * (double)(desiredNumber - initialNumber));
            if (initialNumber < desiredNumber)
            {
                currentNumber += delta;
                if (currentNumber >= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else
            {
                currentNumber -= delta;
                if (currentNumber <= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }

            numberText.text = prefix + Utils.convertPebToTorStr(currentNumber) + postfix;
        }
    }
}
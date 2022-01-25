using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class SlidingBigNumberController : MonoBehaviour
{
    [SerializeField]
    Text numberText;
    [SerializeField]
    float animatedTime = 1.5f;

    string format = "{0}";

    private BigInteger desiredNumber;
    private BigInteger initialNumber;
    private BigInteger currentNumber;

    public void setNumber(BigInteger _number)
    {
        initialNumber = currentNumber; 
        desiredNumber = _number;
    }

    public void reset()
    {
        desiredNumber = initialNumber = currentNumber = BigInteger.Zero;
    }

    public void setFormat(string _format)
    {
        format = _format;
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
                currentNumber += delta; // delta is minus num
                if (currentNumber <= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }

            numberText.text = string.Format(format, Utils.convertPebToTorStr(currentNumber));
        }
    }
}
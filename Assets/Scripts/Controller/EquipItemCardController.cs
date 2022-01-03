using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class EquipItemCardController : MonoBehaviour
{
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Image borderImage;
    [SerializeField]
    GameObject borderEffect;

    EquipItemData data;

    public void setEquipItem(EquipItemData _data)
    {
        data = _data;
        updateCardLayout();
    }

    private void updateCardLayout()
    {
        if (data == null)
        {
            itemImage.gameObject.SetActive(false);
            borderImage.gameObject.SetActive(false);
            borderEffect.SetActive(false);
            return;
        } else
        {
            itemImage.gameObject.SetActive(true);
            borderImage.gameObject.SetActive(true);
        }
        itemImage.sprite = data.imageSpirte;
        borderImage.color = ItemManager.instance.getGradeColor(data.grade);
        borderEffect.SetActive(data.grade == ItemGrade.LEGEND);
    }
}
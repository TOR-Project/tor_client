using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class EquipItemCardController : MonoBehaviour
{
    public const string BACGROUND_IMAGE_URL = "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/staking_page/pub+additional+sample+(22.01.04)/frame/item_frame.jpg";

    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Image itemImage;
    [SerializeField]
    Image borderImage;
    [SerializeField]
    GameObject borderEffect;

    EquipItemData data;

    private void OnEnable()
    {
        AssetsLoadManager.instance.requestSprite(BACGROUND_IMAGE_URL, (_sprite) =>
        {
            backgroundImage.sprite = _sprite;
            backgroundImage.gameObject.SetActive(true);
            return true;
        }, null);
    }

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

    public void setParticleEnabled(bool _set)
    {
        borderEffect.SetActive(_set && data != null && data.grade == ItemGrade.LEGEND);
    }
}
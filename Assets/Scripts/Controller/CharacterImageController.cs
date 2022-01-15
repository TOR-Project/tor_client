using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

public class CharacterImageController : MonoBehaviour
{
    [SerializeField]
    Image avatarImage;
    [SerializeField]
    EquipItemCardController[] equipItemCardControllerList;


    public void updateCharacterImage(CharacterData _data)
    {
        if (_data == null)
        {
            return;
        }

        avatarImage.sprite = CharacterManager.instance.getAvatarImage(_data.job);

        EquipItemData[] equipItemDataList = new EquipItemData[]
        {
            ItemManager.instance.getEquipItem(_data.equipData.head),
            ItemManager.instance.getEquipItem(_data.equipData.weapon),
            ItemManager.instance.getEquipItem(_data.equipData.accessory),
            ItemManager.instance.getEquipItem(_data.equipData.armor),
            ItemManager.instance.getEquipItem(_data.equipData.pants),
            ItemManager.instance.getEquipItem(_data.equipData.shoes),
        };

        for (int i = 0; i < equipItemDataList.Length; i++)
        {
            EquipItemData ed = equipItemDataList[i];

            EquipItemCardController eicc = equipItemCardControllerList[i];
            eicc.setEquipItem(ed);
        }
    }
}
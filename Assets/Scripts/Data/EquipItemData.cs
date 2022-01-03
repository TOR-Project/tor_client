using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipItemData
{
    public long key; // 000 000 0 00 000 - race job grade category id

    public int id;
    public string nameKey;
    public EquipItemCategory category;
    public ItemGrade grade;
    public long jobLimit;
    public long raceLimit;

    public string imageUrl;
    public Sprite imageSpirte;
    private Action loadImagePostAction;

    public int att;
    public int def;
    public int attBoost;
    public int defBoost;

    public EquipItemData(int _id, string _nameKey, EquipItemCategory _category, ItemGrade _grade, long _jobLimit, long _raceLimit, string _imageUrl)
    {
        id = _id;
        nameKey = _nameKey;
        category = _category;
        grade = _grade;
        jobLimit = _jobLimit;
        raceLimit = _raceLimit;
        imageUrl = _imageUrl;

        buildItemProperty();
    }

    public long generateKey()
    {
        key = raceLimit * 1000000000 + jobLimit * 1000000 + (int) grade * 100000 + (int) category * 1000 + id;
        return key;
    }

    public void loadItemImage(Action _postAction)
    {
        loadImagePostAction = _postAction;
        AssetsLoadManager.instance.requestSprite(imageUrl, setItemImageSprite);
    }

    public bool setItemImageSprite(Sprite _sprite)
    {
        imageSpirte = _sprite;
        loadImagePostAction?.Invoke();
        return true;
    }

    private void buildItemProperty()
    {   
        switch (category) {
            case EquipItemCategory.WEAPON:
                switch (grade)
                {
                    case ItemGrade.RARE:
                        att = 100;
                        break;
                    case ItemGrade.LEGEND:
                        att = 150;
                        attBoost = 10;
                        break;
                    default:
                        att = 50;
                        break;
                }
                break;
            case EquipItemCategory.HELMET:
                switch (grade)
                {
                    case ItemGrade.RARE:
                        att = 80;
                        break;
                    case ItemGrade.LEGEND:
                        att = 120;
                        attBoost = 10;
                        break;
                    default:
                        att = 40;
                        break;
                }
                break;
            case EquipItemCategory.ACCESSORY:
                switch (grade)
                {
                    case ItemGrade.RARE:
                        att = 60;
                        break;
                    case ItemGrade.LEGEND:
                        att = 90;
                        if (nameKey.Contains("NECKLACE"))
                        {
                            attBoost = 15;
                        } else
                        {
                            attBoost = 10;
                        }
                        break;
                    default:
                        att = 30;
                        break;
                }
                break;
            default:
                switch (grade)
                {
                    case ItemGrade.RARE:
                        def = 80;
                        break;
                    case ItemGrade.LEGEND:
                        def = 120;
                        defBoost = 10;
                        break;
                    default:
                        def = 40;
                        break;
                }
                break;
        }
    }
}
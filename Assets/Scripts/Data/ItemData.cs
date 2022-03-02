using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;
    public string nameKey;
    public string expKey;
    public ItemCategory category;
    public ItemGrade grade;

    public string imageUrl;
    public Sprite imageSpirte;
    private Action loadImagePostAction;

    public ItemData(int _id, string _nameKey, string _expKey, ItemCategory _category, ItemGrade _grade, string _imageUrl)
    {
        id = _id;
        nameKey = _nameKey;
        expKey = _expKey;
        category = _category;
        grade = _grade;
        imageUrl = _imageUrl;
    }

    public void loadItemImage(Action _postAction)
    {
        loadImagePostAction = _postAction;
        AssetsLoadManager.instance.requestSprite(imageUrl, setItemImageSprite, null);
    }

    public bool setItemImageSprite(Sprite _sprite)
    {
        imageSpirte = _sprite;
        loadImagePostAction?.Invoke();
        return true;
    }

}
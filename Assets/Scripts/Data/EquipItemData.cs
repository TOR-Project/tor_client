using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipItemData
{
    public long key; // 00 00 0 00 000

    public int id;
    public int category;
    public int grade;
    public int jobLimit;
    public int raceLimit;

    public string imageUrl;
    public Sprite imageSpirte;

    public int att;
    public int def;
    public int attBoost;
    public int defBoost;
}
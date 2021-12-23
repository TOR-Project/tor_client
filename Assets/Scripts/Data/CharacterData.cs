using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int tokenId = -1;
    public string url = "";
    public string image = "";
    public string description = "";
    public string name = "";
    public List<AttributeData> attributes = new List<AttributeData>();

    public int level = 1;
    public int exp = 0;
    public int country = -1;
    public int race = -1;
    public int job = -1;
    public int statusBonus = 0;
    public int version = 0;
    public StatusData statusData = new StatusData();
    public EquipData equipData = new EquipData();

    public StakingData stakingData = new StakingData();
}
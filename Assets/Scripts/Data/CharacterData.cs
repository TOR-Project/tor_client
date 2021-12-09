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
}
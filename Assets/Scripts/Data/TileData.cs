using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData 
{
    public int ix;
    public int iy;
    public float x;
    public float y;
    public float z;
    public int tile;
    public List<DecorationsData> decoList = new List<DecorationsData>();

    public void parseData(Dictionary<string, object> _data)
    {
        ix = int.Parse(_data["ix"].ToString());
        iy = int.Parse(_data["iy"].ToString());
        x = float.Parse(_data["x"].ToString());
        y = float.Parse(_data["y"].ToString());
        z = float.Parse(_data["z"].ToString());

        foreach (Dictionary<string, object> dData in JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["decorations"].ToString()))
        {
            DecorationsData decoData = new DecorationsData();
            decoData.parseData(dData);
            decoList.Add(decoData);
        }

    }
}
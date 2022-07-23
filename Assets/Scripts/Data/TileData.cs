using Newtonsoft.Json;
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
    public int country = -1;
    public string owner = "";
    public List<DecorationsData> decoList = new List<DecorationsData>();
    public List<ConstructionsData> constList = new List<ConstructionsData>();

    public void parseData(Dictionary<string, object> _data)
    {
        ix = int.Parse(_data["ix"].ToString());
        iy = int.Parse(_data["iy"].ToString());
        tile = int.Parse(_data["tile"].ToString());
        x = float.Parse(_data["x"].ToString());
        y = float.Parse(_data["y"].ToString());
        z = float.Parse(_data["z"].ToString());

        if (_data.ContainsKey("country"))
        {
            country = int.Parse(_data["country"].ToString());
        }

        if (_data.ContainsKey("owner"))
        {
            owner = _data["owner"].ToString();
        }

        foreach (Dictionary<string, object> dData in JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["decorations"].ToString()))
        {
            DecorationsData decoData = new DecorationsData();
            decoData.parseData(dData);
            decoList.Add(decoData);
        }

        foreach (Dictionary<string, object> dData in JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["constructions"].ToString()))
        {
            ConstructionsData constData = new ConstructionsData();
            constData.parseData(dData);
            constList.Add(constData);
        }

    }
}
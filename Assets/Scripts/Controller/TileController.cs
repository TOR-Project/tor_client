using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class TileController : MonoBehaviour
{
    [SerializeField]
    GameObject selectionObject;
    [SerializeField]
    TileData tileData = new TileData();

    public void parseTileData(Dictionary<string, object> _data)
    {
        tileData.parseData(_data);
    }

}
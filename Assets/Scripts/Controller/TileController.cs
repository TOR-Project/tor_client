using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField]
    TileData tileData;

    MapWindowController mapWindowController;

    public void setTileData(TileData tData)
    {
        tileData = tData;
    }

    public void setMapWindowController(MapWindowController _mapWindowController)
    {
        mapWindowController = _mapWindowController;
    }

    public TileData getTileData()
    {
        return tileData;
    }
}
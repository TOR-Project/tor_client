using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    Dictionary<Vector2, TileController> tileMap = new Dictionary<Vector2, TileController>();

    [SerializeField]
    GameObject[] tilePrefab;

    [SerializeField]
    GameObject[] wastelandDecorationPrefabs;
    [SerializeField]
    GameObject[] desertDecorationPrefabs;
    [SerializeField]
    GameObject[] plainDecorationPrefabs;
    [SerializeField]
    GameObject[] jungleDecorationPrefabs;
    [SerializeField]
    GameObject[] riverDecorationPrefabs;
    [SerializeField]
    GameObject[] snowDecorationPrefabs;
    [SerializeField]
    GameObject[] mountainDecorationPrefabs;

    [SerializeField]
    GameObject[] castleConstuctionsPrefabs;

    [SerializeField]
    GameObject selectionPrefab;
    [SerializeField]
    GameObject borderPartsPrefab;


    static MapManager mInstance;
    public static MapManager instance
    {
        get
        {
            return mInstance;
        }
    }

    private MapManager()
    {
        mInstance = this;

    }

    public void addTileController(int ix, int iy, TileController tileController)
    {
        tileMap.Add(new Vector2(ix, iy), tileController);
    }

    public TileController[] getNearTileControllerArr(int ix, int iy)
    {
        TileController[] arr = new TileController[6];
        arr[0] = tileMap.ContainsKey(new Vector2(ix - 1, iy - 1)) ? tileMap[new Vector2(ix - 1, iy - 1)] : null;
        arr[1] = tileMap.ContainsKey(new Vector2(ix, iy - 1)) ? tileMap[new Vector2(ix, iy - 1)] : null;
        arr[2] = tileMap.ContainsKey(new Vector2(ix + 1, iy)) ? tileMap[new Vector2(ix + 1, iy)] : null;
        arr[3] = tileMap.ContainsKey(new Vector2(ix + 1, iy + 1)) ? tileMap[new Vector2(ix + 1, iy + 1)] : null;
        arr[4] = tileMap.ContainsKey(new Vector2(ix, iy + 1)) ? tileMap[new Vector2(ix, iy + 1)] : null;
        arr[5] = tileMap.ContainsKey(new Vector2(ix - 1, iy)) ? tileMap[new Vector2(ix - 1, iy)] : null;

        return arr;
    }

    public TileController getTileController(int ix, int iy)
    {
        Vector2 key = new Vector2(ix, iy);
        if (tileMap.ContainsKey(key))
        {
            return tileMap[key];
        }

        return null;
    }

    public GameObject getTilePrefab(TileData td)
    {
        return tilePrefab[td.tile];
    }

    public GameObject getDecorationPrefab(DecorationsData decoData)
    {
        GameObject[] decoPrefabs;
        if (decoData.category == "Wasteland")
        {
            decoPrefabs = wastelandDecorationPrefabs;
        }
        else if (decoData.category == "Desert")
        {
            decoPrefabs = desertDecorationPrefabs;
        }
        else if (decoData.category == "Plain")
        {
            decoPrefabs = plainDecorationPrefabs;
        }
        else if (decoData.category == "Jungle")
        {
            decoPrefabs = jungleDecorationPrefabs;
        }
        else if (decoData.category == "River")
        {
            decoPrefabs = riverDecorationPrefabs;
        }
        else if (decoData.category == "Snow")
        {
            decoPrefabs = snowDecorationPrefabs;
        }
        else
        {
            decoPrefabs = mountainDecorationPrefabs;
        }

        return decoPrefabs[int.Parse(decoData.name)];
    }

    public GameObject getConstructionPrefab(ConstructionsData constData)
    {
        GameObject[] constPrefabs = castleConstuctionsPrefabs;
        return constPrefabs[int.Parse(constData.name)];
    }

    public GameObject getSelectionPrefab()
    {
        return selectionPrefab;
    }

    public GameObject getBorderPartsPrefab()
    {
        return borderPartsPrefab;
    }
}
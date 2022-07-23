using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MapGenenrator : MonoBehaviour
{
    [SerializeField]
    Transform fieldTR;

    [SerializeField]
    MapWindowController mapWindowController;

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "world_map.json");

        List<TileController>[] countryTileList = new List<TileController>[CountryManager.COUNTRY_MAX];
        for (int i = 0; i < countryTileList.Length; i++)
        {
            countryTileList[i] = new List<TileController>();
        }

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dataAsJson);

            foreach (Dictionary<string, object> tileDataRaw in values)
            {
                TileData tData = new TileData();
                tData.parseData(tileDataRaw);

                GameObject tileObject = Instantiate(MapManager.instance.getTilePrefab(tData), new UnityEngine.Vector3(tData.x, 0, tData.z), UnityEngine.Quaternion.Euler(0, 0, 0), fieldTR);
                tileObject.transform.GetChild(0).transform.localScale = new Vector3(1, tData.y, 1);
                TileController tileController = tileObject.GetComponent<TileController>();
                tileController.setTileData(tData);
                tileController.setMapWindowController(mapWindowController);

                foreach (DecorationsData decoData in tData.decoList)
                {
                    GameObject decorationObject = Instantiate(MapManager.instance.getDecorationPrefab(decoData), tileObject.transform.position + new UnityEngine.Vector3(decoData.dx, decoData.dy, decoData.dz), UnityEngine.Quaternion.Euler(decoData.rx, decoData.ry, decoData.rz), tileObject.transform);
                    decorationObject.transform.localScale = new Vector3(decoData.sx, decoData.sy, decoData.sz);
                }

                foreach (ConstructionsData constData in tData.constList)
                {
                    GameObject constuctionsObject = Instantiate(MapManager.instance.getConstructionPrefab(constData), tileObject.transform.position + new UnityEngine.Vector3(constData.dx, constData.dy, constData.dz), UnityEngine.Quaternion.Euler(constData.rx, constData.ry, constData.rz), tileObject.transform);
                    constuctionsObject.transform.localScale = new Vector3(constData.sx, constData.sy, constData.sz);
                }

                MapManager.instance.addTileController(tData.ix, tData.iy, tileController);

                if (tData.country != -1)
                {
                    countryTileList[tData.country].Add(tileController);
                }
            }

            for (int i = 0; i < countryTileList.Length; i++)
            {
                foreach(TileController tileController in countryTileList[i])
                {
                    TileData tileData = tileController.getTileData();
                    TileController[] nearTileArr = MapManager.instance.getNearTileControllerArr(tileData.ix, tileData.iy);
                    for (int j = 0; j < 6; j++)
                    {
                        if (nearTileArr[j] == null || nearTileArr[j].getTileData().country == tileData.country)
                        {
                            continue;
                        }

                        GameObject borderParts = Instantiate(MapManager.instance.getBorderPartsPrefab(), tileController.transform.position + new UnityEngine.Vector3(0, tileData.y * 0.2f + 0.1f, 0), UnityEngine.Quaternion.Euler(0, 60 * j - 30, 0), tileController.transform);
                        BorderPartsController borderPartsController = borderParts.GetComponent<BorderPartsController>();
                        if (borderPartsController != null)
                        {
                            borderPartsController.setParticleBaseColor(CountryManager.instance.getBaseColor(tileData.country));
                        }
                    }
                }
            }
        }
    }
}
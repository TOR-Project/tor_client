using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MapGenenrator : MonoBehaviour
{
    [SerializeField]
    Transform fieldTR;
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
    GameObject[] commonConstuctionsPrefabs;

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "world_map.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dataAsJson);

            foreach (Dictionary<string, object> tileData in values)
            {
                GameObject tileObject = Instantiate(tilePrefab[int.Parse(tileData["tile"].ToString())], new UnityEngine.Vector3(float.Parse(tileData["x"].ToString()), 0, float.Parse(tileData["z"].ToString())), UnityEngine.Quaternion.Euler(0, 90, 0), fieldTR);
                tileObject.transform.GetChild(0).transform.localScale = new Vector3(1, float.Parse(tileData["y"].ToString()), 1);
                TileController tileController = tileObject.GetComponent<TileController>();
                tileController.parseTileData(tileData);

                foreach (Dictionary<string, object> decoData in JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(tileData["decorations"].ToString()))
                {
                    GameObject[] decoPrefabs;
                    if (decoData["category"].ToString() == "Wasteland")
                    {
                        decoPrefabs = wastelandDecorationPrefabs;
                    }
                    else if (decoData["category"].ToString() == "Desert")
                    {
                        decoPrefabs = desertDecorationPrefabs;
                    }
                    else if (decoData["category"].ToString() == "Plain")
                    {
                        decoPrefabs = plainDecorationPrefabs;
                    }
                    else if (decoData["category"].ToString() == "Jungle")
                    {
                        decoPrefabs = jungleDecorationPrefabs;
                    }
                    else if (decoData["category"].ToString() == "River")
                    {
                        decoPrefabs = riverDecorationPrefabs;
                    }
                    else if (decoData["category"].ToString() == "Snow")
                    {
                        decoPrefabs = snowDecorationPrefabs;
                    }
                    else
                    {
                        decoPrefabs = mountainDecorationPrefabs;
                    }
                    GameObject decorationObject = (GameObject)Instantiate(decoPrefabs[int.Parse(decoData["name"].ToString()) - 1], tileObject.transform.position + new UnityEngine.Vector3(float.Parse(decoData["dx"].ToString()), float.Parse(decoData["dy"].ToString()), float.Parse(decoData["dz"].ToString())), UnityEngine.Quaternion.Euler(float.Parse(decoData["rx"].ToString()), float.Parse(decoData["ry"].ToString()), float.Parse(decoData["rz"].ToString())), tileObject.transform);
                    decorationObject.transform.localScale = new Vector3(float.Parse(decoData["sx"].ToString()), float.Parse(decoData["sy"].ToString()), float.Parse(decoData["sz"].ToString()));
                }

                foreach(Dictionary<string, object> decoData in JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(tileData["constructions"].ToString()))
                {
                    GameObject[] constPrefabs = commonConstuctionsPrefabs;
                    GameObject decorationObject = (GameObject)Instantiate(constPrefabs[int.Parse(decoData["name"].ToString()) - 1], tileObject.transform.position + new UnityEngine.Vector3(float.Parse(decoData["dx"].ToString()), float.Parse(decoData["dy"].ToString()), float.Parse(decoData["dz"].ToString())), UnityEngine.Quaternion.Euler(float.Parse(decoData["rx"].ToString()), float.Parse(decoData["ry"].ToString()), float.Parse(decoData["rz"].ToString())), tileObject.transform);
                    decorationObject.transform.localScale = new Vector3(float.Parse(decoData["sx"].ToString()), float.Parse(decoData["sy"].ToString()), float.Parse(decoData["sz"].ToString()));
                }
            }
        }
    }
}
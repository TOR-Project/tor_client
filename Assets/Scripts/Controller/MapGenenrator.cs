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
    GameObject[] objectPrefab;

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "world_map.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(dataAsJson);

            foreach (Dictionary<string, object> tileData in values)
            {
                GameObject tr = Instantiate(tilePrefab[int.Parse(tileData["tile"].ToString())], new UnityEngine.Vector3(float.Parse(tileData["x"].ToString()), 0, float.Parse(tileData["z"].ToString())), UnityEngine.Quaternion.Euler(0, 90, 0), fieldTR);
                tr.transform.localScale = new Vector3(1, 1 + float.Parse(tileData["y"].ToString()), 1);

                GameObject ob = Instantiate(objectPrefab[UnityEngine.Random.Range(0, objectPrefab.Length)], tr.transform.position + new UnityEngine.Vector3(0, float.Parse(tileData["y"].ToString()), 0), UnityEngine.Quaternion.Euler(0, 0, 0), tr.transform);
                ob.transform.localScale = new Vector3(1, 1 / (1 + float.Parse(tileData["y"].ToString())), 1);

            }
        }
    }
}
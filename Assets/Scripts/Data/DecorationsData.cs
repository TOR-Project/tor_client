using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecorationsData
{
    public float dx;
    public float dy;
    public float dz;
    public float sx;
    public float sy;
    public float sz;
    public float rx;
    public float ry;
    public float rz;
    public string category;
    public string name;

    internal void parseData(Dictionary<string, object> _data)
    {
        category = _data["category"].ToString();
        name = _data["name"].ToString();
        dx = float.Parse(_data["dx"].ToString());
        dy = float.Parse(_data["dy"].ToString());
        dz = float.Parse(_data["dz"].ToString());
        sx = float.Parse(_data["sx"].ToString());
        sy = float.Parse(_data["sy"].ToString());
        sz = float.Parse(_data["sz"].ToString());
        rx = float.Parse(_data["rx"].ToString());
        ry = float.Parse(_data["ry"].ToString());
        rz = float.Parse(_data["rz"].ToString());
    }
}
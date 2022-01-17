using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommentData
{
    public int id;
    public string nickname;
    public long block;
    public string comment;
    public CommentData(Dictionary<string, object> _dic)
    {
        id = int.Parse(_dic["id"].ToString());
        nickname = _dic["nickname"].ToString();
        block = long.Parse(_dic["block"].ToString());
        comment = _dic["comment"].ToString();
    }

}
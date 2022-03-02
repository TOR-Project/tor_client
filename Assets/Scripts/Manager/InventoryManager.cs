using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public const int INVENTORY_DATA_REQUEST_INTERVAL = 60; // 1 min

    long lastRequestBlock = 0;
    Dictionary<int, int> myItemData = new Dictionary<int, int> ();
    private List<DataObserver<Dictionary<int, int>>> observerList = new List<DataObserver<Dictionary<int, int>>>();

    static InventoryManager mInstance;
    public static InventoryManager instance {
        get {
            return mInstance;
        }
    }

    public int getMyItemCount(int _id)
    {
        return myItemData.ContainsKey(_id) ? myItemData[_id] : 0;
    }

    public void requestInventoryItemList()
    {
        if (SystemInfoManager.instance.blockNumber < lastRequestBlock + INVENTORY_DATA_REQUEST_INTERVAL) {

            notifyInventoryItemListChanged(myItemData);
            return;
        }

        lastRequestBlock = SystemInfoManager.instance.blockNumber;
        ContractManager.instance.reqInventoryItemList();
    }

    public void responseInventoryItemList(Dictionary<string, object> _data)
    {
        myItemData.Clear();

        List<Dictionary<string, object>> itemList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["itemList"].ToString());
        foreach(Dictionary<string, object> item in itemList)
        {
            myItemData[int.Parse(item["id"].ToString())] = int.Parse(item["count"].ToString());
        }

        notifyInventoryItemListChanged(myItemData);
    }

    public void responceInventoryItemData(Dictionary<string, object> _data)
    {
        int id = int.Parse(_data["id"].ToString());
        int count = int.Parse(_data["count"].ToString());
        myItemData[id] = count;

        notifyInventoryItemListChanged(myItemData);
    }

    public void notifyInventoryItemListChanged(Dictionary<int, int> _list)
    {
        foreach (DataObserver<Dictionary<int, int>> ob in observerList)
        {
            ob.onDataReceived(_list);
        }
    }

    public void addObserver(DataObserver<Dictionary<int, int>> ob)
    {
        observerList.Add(ob);
    }

    public void removeObserver(DataObserver<Dictionary<int, int>> ob)
    {
        observerList.Remove(ob);
    }

    private InventoryManager()
    {
        mInstance = this;
    }

}
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretShopManager : MonoBehaviour
{
    public const int DATA_REQUEST_INTERVAL = 300; // 1 min

    long lastRequestBlock = 0;

    Dictionary<int, SellItemData> sellItemData = new Dictionary<int, SellItemData>();
    private List<DataObserver<Dictionary<int, SellItemData>>> observerList = new List<DataObserver<Dictionary<int, SellItemData>>>();


    static SecretShopManager mInstance;

    public static SecretShopManager instance {
        get {
            return mInstance;
        }
    }

    private SecretShopManager()
    {
        mInstance = this;
    }

    public void requestSellItemList()
    {
        if (SystemInfoManager.instance.blockNumber < lastRequestBlock + DATA_REQUEST_INTERVAL)
        {

            notifySellItemListChanged(sellItemData);
            return;
        }

        lastRequestBlock = SystemInfoManager.instance.blockNumber;
        ContractManager.instance.reqSellItemList();
    }

    public SellItemData getSellItemData(int _id)
    {
        return sellItemData.ContainsKey(_id) ? sellItemData[_id] : null;
    }

    public void responseSellItemList(Dictionary<string, object> _data)
    {
        sellItemData.Clear();

        List<Dictionary<string, object>> itemList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["itemList"].ToString());
        foreach (Dictionary<string, object> item in itemList)
        {
            SellItemData sellData = new SellItemData();
            sellData.parse(item);
            sellItemData[sellData.id] = sellData;
        }

        notifySellItemListChanged(sellItemData);
    }

    public void responceSellItemData(Dictionary<string, object> _data)
    {
        SellItemData sellData = new SellItemData();
        sellData.parse(_data);
        sellItemData[sellData.id] = sellData;

        notifySellItemListChanged(sellItemData);
    }

    public void notifySellItemListChanged(Dictionary<int, SellItemData> _list)
    {
        foreach (DataObserver<Dictionary<int, SellItemData>> ob in observerList)
        {
            ob.onDataReceived(_list);
        }
    }

    public void addObserver(DataObserver<Dictionary<int, SellItemData>> ob)
    {
        observerList.Add(ob);
    }

    public void removeObserver(DataObserver<Dictionary<int, SellItemData>> ob)
    {
        observerList.Remove(ob);
    }
}
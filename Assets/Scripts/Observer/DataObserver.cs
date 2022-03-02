using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataObserver<T>
{
    void onDataReceived(T _data);
}

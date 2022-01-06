using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ServerStateObserever
{
    void onServerStateChanged(bool _available);
}

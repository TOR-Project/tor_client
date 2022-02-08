using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GovernanceObserver
{
    void onAgendaListReceived(List<AgendaData> _list);
}

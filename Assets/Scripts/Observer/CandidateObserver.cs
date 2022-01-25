using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CandidateObserver
{
    void onCandidateListReceived(List<CandidateData> _list);
}

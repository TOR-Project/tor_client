using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingRowController : MonoBehaviour
{
    [SerializeField]
    Text numText;
    [SerializeField]
    Text nicknameText;
    [SerializeField]
    Toggle votingToggle;
    [SerializeField]
    GameObject particle;

    Predicate<VotingRowController> resetToggleAction;

    CandidateData candidateData;

    internal void updateCandidateData(CandidateData _candidateData)
    {
        candidateData = _candidateData;
        numText.text = _candidateData.id.ToString();
        nicknameText.text = _candidateData.nickname;
    }

    internal void setResetToggleAction(Predicate<VotingRowController> _action)
    {
        resetToggleAction = _action;
    }

    public void resetToggle()
    {
        votingToggle.SetIsOnWithoutNotify(false);
        particle.SetActive(false);
    }

    public void onToggleChanged()
    {
        resetToggleAction(this);
    }

    public bool isOn()
    {
        return votingToggle.isOn;
    }

    public int getId()
    {
        return candidateData == null ? -1 : candidateData.id;
    }
}

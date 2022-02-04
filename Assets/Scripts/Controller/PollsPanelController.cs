using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollsPanelController : MonoBehaviour
{
    [SerializeField]
    Text votingCountText;

    [SerializeField]
    RectTransform votingContentsRT;

    [SerializeField]
    GameObject votingRowPrefab;

    [SerializeField]
    GameObject noCandidateText;

    public void updatePanel(int _voteCount, List<CandidateData> _list)
    {
        noCandidateText.SetActive(_list.Count <= 0);

        votingCountText.text = string.Format(LanguageManager.instance.getText("ID_VOTING_N_COUNT"), _voteCount);

        for (int i = 0; i < _list.Count; i++)
        {
            CandidateData candidateData = _list[i];

            VotingRowController rowController;
            if (votingContentsRT.childCount > i)
            {
                GameObject childObject = votingContentsRT.GetChild(i).gameObject;
                childObject.SetActive(true);
                rowController = childObject.GetComponent<VotingRowController>();
            }
            else
            {
                GameObject commentRow = Instantiate(votingRowPrefab, votingContentsRT, true);
                commentRow.transform.localScale = UnityEngine.Vector3.one;
                rowController = commentRow.GetComponent<VotingRowController>();
                rowController.setResetToggleAction(resetToggles);
            }

            rowController.updateCandidateData(candidateData);
        }

        for (int i = _list.Count; i < votingContentsRT.childCount; i++)
        {
            GameObject childObject = votingContentsRT.GetChild(i).gameObject;
            childObject.SetActive(false);
        }
    }

    internal bool resetToggles(VotingRowController _trigger)
    {
        for (int i = 0; i < votingContentsRT.childCount; i++)
        {
            GameObject childObject = votingContentsRT.GetChild(i).gameObject;
            VotingRowController rowController = childObject.GetComponent<VotingRowController>();
            if (rowController != null && rowController != _trigger)
            {
                rowController.resetToggle();
            }
        }

        return true;
    }

    public int getSelectedId()
    {
        for (int i = 0; i < votingContentsRT.childCount; i++)
        {
            GameObject childObject = votingContentsRT.GetChild(i).gameObject;
            VotingRowController rowController = childObject.GetComponent<VotingRowController>();
            if (rowController != null && rowController.isOn() && rowController.getId() != -1)
            {
                return rowController.getId();
            }
        }

        return 0;
    }
}

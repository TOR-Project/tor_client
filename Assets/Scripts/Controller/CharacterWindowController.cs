using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class CharacterWindowController : MonoBehaviour
{
    [SerializeField]
    Text allCharacterCountText;
    [SerializeField]
    Text miningCharacterCountText;
    [SerializeField]
    Text electionCharacterCountText;
    [SerializeField]
    Text governanceCharacterCountText;
    [SerializeField]
    Text remainCharacterCountText;

    private void OnEnable()
    {
        CharacterManager.instance.resetLoadingStep();

        updateCharacterInfo();
    }

    private void updateCharacterInfo()
    {
        List<CharacterData> characterList = CharacterManager.instance.getCharacterList();

        int miningCount = 0;
        int electionCount = 0;
        int governanceCount = 0;
        int rebellionCount = 0;
        foreach (CharacterData cd in characterList)
        {
            switch(cd.stakingData.purpose)
            {
                case StakingManager.PURPOSE_MINING:
                    miningCount++;
                    break;
                case StakingManager.PURPOSE_MONARCH:
                    electionCount++;
                    break;
                case StakingManager.PURPOSE_REBELLION:
                    rebellionCount++;
                    break;
                case StakingManager.PURPOSE_GOVERNANCE:
                    governanceCount++;
                    break;
                default:
                    break;
            }
        }

        allCharacterCountText.text = characterList.Count.ToString();
        miningCharacterCountText.text = miningCount.ToString();
        electionCharacterCountText.text = electionCount.ToString();
        governanceCharacterCountText.text = governanceCount.ToString();
        remainCharacterCountText.text = (characterList.Count - miningCount - electionCount - governanceCount - rebellionCount).ToString();
    }
}
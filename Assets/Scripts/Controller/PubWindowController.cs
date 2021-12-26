using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PubWindowController : MonoBehaviour
{
    [SerializeField]
    Button[] filterRegionButton;

    [SerializeField]
    GameObject characterCardPrefab;
    [SerializeField]
    GameObject gridPanel;

    [SerializeField]
    GameObject detailPanel;

    [SerializeField]
    GameObject boostRowPanel;

    [SerializeField]
    Text detailTitleText;
    [SerializeField]
    Text levelText;
    [SerializeField]
    Text boostText;
    [SerializeField]
    Text tokenText;

    private void OnEnable()
    {
        List<CharacterData> list = CharacterManager.instance.getCharacterList();
        list.Sort(SortByTokenIdAscending);

        for (int idx = 0; idx < list.Count; idx++)
        {
            GameObject characterCard;
            if (idx < gridPanel.transform.childCount)
            {
                characterCard = gridPanel.transform.GetChild(idx).gameObject;
                characterCard.SetActive(true);
            }
            else
            {
                characterCard = Instantiate(characterCardPrefab, gridPanel.transform, true);
            }

            CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
            cardController.setCharacterId(list[idx], CharacterCardController.STATE_PUB);
            cardController.setClickCallback(selectCharacterCard);
        }

        for (int idx = list.Count; idx < gridPanel.transform.childCount; idx++)
        {
            gridPanel.transform.GetChild(idx).gameObject.SetActive(false);
        }

    }

    public int SortByTokenIdAscending(CharacterData cd1, CharacterData cd2)
    {
        return cd1.tokenId - cd2.tokenId;
    }

    public bool selectCharacterCard(CharacterCardController _cardController)
    {

        return true;
    }
}
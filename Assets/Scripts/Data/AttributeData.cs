using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeData
{
    public string traitType = "";
    public string valueStr = "";
    public int valueInt = 0;
    public string displayType = "";

    public void applyToCharacterData(CharacterData cd)
    {
        if ("level".Equals(traitType))
        {
            cd.level = valueInt;
        }
        else if ("Region".Equals(traitType))
        {
            if ("Hellvesta".Equals(valueStr))
            {
                cd.country = CountryManager.COUNTRY_HELLVESTA;
            }
            else if ("Evegenis".Equals(valueStr))
            {
                cd.country = CountryManager.COUNTRY_EVEGENIS;
            }
            else if ("Barbaros".Equals(valueStr))
            {
                cd.country = CountryManager.COUNTRY_BARBAROS;
            }
            else if ("Enfiliis".Equals(valueStr))
            {
                cd.country = CountryManager.COUNTRY_ENFILIIS;
            }
            else if ("Tripoli".Equals(valueStr))
            {
                cd.country = CountryManager.COUNTRY_TRIPOLI;
            }
        }
        else if ("Race".Equals(traitType))
        {
            if (valueStr.Contains("Human"))
            {
                cd.race = CharacterManager.RACE_HUMAN;
            }
            else if (valueStr.Contains("Elf"))
            {
                cd.race = CharacterManager.RACE_ELF;
            }
            else if (valueStr.Contains("Orcs"))
            {
                cd.race = CharacterManager.RACE_ORC;
            }
            else if (valueStr.Contains("Darkelf"))
            {
                cd.race = CharacterManager.RACE_DARKELF;
            }
            else if (valueStr.Contains("Dragon"))
            {
                cd.race = CharacterManager.RACE_DRAGON;
            }

            if (valueStr.Contains("Ranger"))
            {
                cd.job = CharacterManager.JOB_RANGER;
            }
            else if (valueStr.Contains("Warrior"))
            {
                cd.job = CharacterManager.JOB_WARRIOR;
            }
            else if (valueStr.Contains("Wizard"))
            {
                cd.job = CharacterManager.JOB_WIZARD;
            }
            else if (valueStr.Contains("Bishop"))
            {
                cd.job = CharacterManager.JOB_BISHOP;
            }
            else if (valueStr.Contains("Infantry"))
            {
                cd.job = CharacterManager.JOB_INFANTRY;
            }
            else if (valueStr.Contains("Witchdoctor"))
            {
                cd.job = CharacterManager.JOB_WITCH_DOCTOR;
            }
            else if (valueStr.Contains("Assassin"))
            {
                cd.job = CharacterManager.JOB_ASSASSIN;
            }
            else if (valueStr.Contains("Sorcerer"))
            {
                cd.job = CharacterManager.JOB_SORCERER;
            }
        }
        else if ("Attack".Equals(traitType))
        {
            cd.statusData.att = valueInt;
        }
        else if ("Defense".Equals(traitType))
        {
            cd.statusData.def = valueInt;
        }
        else if ("Weapon".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.weapon = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.weapon = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.weapon = 2;
            }
            else if (valueStr.Contains("Legend")) {
                cd.equipData.weapon = 3;
            }
        }
        else if ("Acc".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.accessory = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.accessory = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.accessory = 2;
            }
            else if (valueStr.Contains("Legend"))
            {
                if (valueStr.Contains("Earring"))
                {
                    cd.equipData.accessory = 3;
                } else
                {
                    cd.equipData.accessory = 4;
                }
            }
        }
        else if ("Top".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.armor = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.armor = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.armor = 2;
            }
            else if (valueStr.Contains("Legend"))
            {
                cd.equipData.armor = 3;
            }
        }
        else if ("Bottom".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.pants = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.pants = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.pants = 2;
            }
            else if (valueStr.Contains("Legend"))
            {
                cd.equipData.pants = 3;
            }
        }
        else if ("Head".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.head = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.head = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.head = 2;
            }
            else if (valueStr.Contains("Legend"))
            {
                cd.equipData.head = 3;
            }
        }
        else if ("Shoes".Equals(traitType))
        {
            if ("None".Equals(valueStr))
            {
                cd.equipData.shoes = 0;
            }
            else if (valueStr.Contains("Normal"))
            {
                cd.equipData.shoes = 1;
            }
            else if (valueStr.Contains("Rare"))
            {
                cd.equipData.shoes = 2;
            }
            else if (valueStr.Contains("Legend"))
            {
                cd.equipData.shoes = 3;
            }
        }
    }
}
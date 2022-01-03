using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    Dictionary<long, EquipItemData> equipItemDataMap = new Dictionary<long, EquipItemData>();
    int equipItemLoadedCount = 0;

    static ItemManager mInstance;
    public static ItemManager instance {
        get {
            return mInstance;
        }
    }

    private ItemManager()
    {
        mInstance = this;

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ASSASSIN_DAGGER", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.ASSASSIN, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_assassin/3.+weapon/Normal+Assassin+Dagger.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ASSASSIN_DAGGER", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.ASSASSIN, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_assassin/3.+weapon/Rare+Assassin+Dagger.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ASSASSIN_DAGGER", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.ASSASSIN, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_assassin/3.+weapon/Legend+Assassin+Dagger.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_SORCERER_ORB", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.SORCERER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_sorcerer/3.+weapon/Normal+Sorcerer+Orb.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SORCERER_ORB", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.SORCERER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_assassin/3.+weapon/Rare+Sorcerer+Orb.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_SORCERER_ORB", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.SORCERER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/darkelf_assassin/3.+weapon/Legend+Sorcerer+Orb.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ELF_ORB", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.BISHOP, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/3.+weapon/Normal+Elf+Orb.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ELF_ORB", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.BISHOP, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/3.+weapon/Rare+Elf+Orb.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ELF_ORB", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.BISHOP, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/3.+weapon/Legend+Elf+Orb.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WIZARD_WAND", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.WIZARD, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_wizard/3.+weapon/Normal+Wizard+Wand.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WIZARD_WAND", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.WIZARD, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_wizard/3.+weapon/Rare+Wizard+Wand.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_WIZARD_WAND", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.WIZARD, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_wizard/3.+weapon/Legend+Wizard+Wand.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_RANGER_BOW", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/3.+weapon/Normal+Ranger+Bow.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_RANGER_BOW", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/3.+weapon/Rare+Ranger+Bow.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_RANGER_BOW", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/3.+weapon/Legend+Ranger+Bow.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WARRIOR_SWORD", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/3.+weapon/Normal+Warrior+Sword.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SOLDIER_SWORD", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/3.+weapon/Rare+Soldier+Sword.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_KNIGHT_SWORD", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/3.+weapon/Legend+Knight+Sword.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_AXE", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/3.+weapon/Normal+Orc+Axe.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ORC_AXE", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/3.+weapon/Rare+Orc+Axe.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ORC_AXE", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/3.+weapon/Legend+Orc+Axe.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WITCH_WAND", EquipItemCategory.WEAPON, ItemGrade.NORMAL, EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/3.+weapon/Normal_witchdoctor_wand.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WITCH_WAND", EquipItemCategory.WEAPON, ItemGrade.RARE, EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/3.+weapon/Rare_witchdoctor_wand.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_WITCH_WAND", EquipItemCategory.WEAPON, ItemGrade.LEGEND, EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/3.+weapon/Legend_witchdoctor_wand.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ELF_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.NORMAL, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Normal+Elf+Ring.png");
        addEquipItemData(2, "ID_EQUIP_NAME_NORMAL_ELF_NECKLACE",EquipItemCategory.ACCESSORY, ItemGrade.NORMAL, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Normal+Elf+Necklace.png");
        addEquipItemData(3, "ID_EQUIP_NAME_RARE_ELF_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.RARE, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Rare+Elf+Ring.png");
        addEquipItemData(4, "ID_EQUIP_NAME_RARE_ELF_NECKLACE", EquipItemCategory.ACCESSORY, ItemGrade.RARE, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Rare+Elf+Necklace.png");
        addEquipItemData(5, "ID_EQUIP_NAME_LEGEND_ELF_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.LEGEND, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Legend+Elf+Ring.png");
        addEquipItemData(6, "ID_EQUIP_NAME_LEGEND_ELF_NECKLACE", EquipItemCategory.ACCESSORY, ItemGrade.LEGEND, EquipItemJobLimit.ALL, EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/elf_bishop/4.+acc/Legend+Elf+Necklace.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.NORMAL, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Normal+Orc+Earring.png");
        addEquipItemData(2, "ID_EQUIP_NAME_NORMAL_ORC_NECKLACE", EquipItemCategory.ACCESSORY, ItemGrade.NORMAL, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Normal+Orc+Necklace.png");
        addEquipItemData(3, "ID_EQUIP_NAME_RARE_ORC_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.RARE, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Rare+Orc+Earring.png");
        addEquipItemData(4, "ID_EQUIP_NAME_RARE_ORC_NECKLACE", EquipItemCategory.ACCESSORY, ItemGrade.RARE, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Rare+Orc+Necklace.png");
        addEquipItemData(5, "ID_EQUIP_NAME_LEGEND_ORC_EARRING", EquipItemCategory.ACCESSORY, ItemGrade.LEGEND, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Legend+Orc+Earring.png");
        addEquipItemData(6, "ID_EQUIP_NAME_LEGEND_ORC_NECKLACE", EquipItemCategory.ACCESSORY, ItemGrade.LEGEND, EquipItemJobLimit.ALL, EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/4.+acc/Legend+Orc+Necklace.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_HELMET", EquipItemCategory.HELMET, ItemGrade.NORMAL, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/7.+head/Normal+Orc+Helmet.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ORC_HELMET", EquipItemCategory.HELMET, ItemGrade.RARE, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/7.+head/Rare+Orc+Helmet.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ORC_HELMET", EquipItemCategory.HELMET, ItemGrade.LEGEND, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/7.+head/Legend+Orc+Helmet.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WARRIOR_HOOD", EquipItemCategory.HELMET, ItemGrade.NORMAL, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/7.+head/Normal+Warrior+Hood.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SOLDIER_HELMET", EquipItemCategory.HELMET, ItemGrade.RARE, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/7.+head/Rare+Soldier+Helmet.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_KNIGHT_HELMET", EquipItemCategory.HELMET, ItemGrade.LEGEND, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/7.+head/Legend+Knight+Helmet.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_RANGER_HOOD", EquipItemCategory.HELMET, ItemGrade.NORMAL, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/7.+head/Normal+Ranger+Hood.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WIZARD_CROWN", EquipItemCategory.HELMET, ItemGrade.RARE, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/7.+head/Rare+Wizard+Crown.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ELF_CROWN", EquipItemCategory.HELMET, ItemGrade.LEGEND, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/7.+head/Legend+Elf+Crown.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WITCH_CROWN", EquipItemCategory.HELMET, ItemGrade.NORMAL, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/7.+head/Normal+Witch+Crown.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_DARKELF_CROWN", EquipItemCategory.HELMET, ItemGrade.RARE, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/7.+head/Rare+Darkelf+Crown.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_QUEEN_CROWN", EquipItemCategory.HELMET, ItemGrade.LEGEND, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/7.+head/Legend+Queen+Crown.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_TOP", EquipItemCategory.ARMOR, ItemGrade.NORMAL, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/5.+top/Normal+Orc+Top.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ORC_TOP", EquipItemCategory.ARMOR, ItemGrade.RARE, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/5.+top/Rare+Orc+Top.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ORC_TOP", EquipItemCategory.ARMOR, ItemGrade.LEGEND, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/5.+top/Legend+Orc+Top.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WARRIOR_TOP", EquipItemCategory.ARMOR, ItemGrade.NORMAL, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/5.+top/Normal+Warrior+Top.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SOLDIER_TOP", EquipItemCategory.ARMOR, ItemGrade.RARE, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/5.+top/Rare+Soldier+Top.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_KNIGHT_TOP", EquipItemCategory.ARMOR, ItemGrade.LEGEND, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/5.+top/Legend+Knight+Top.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_RANGER_TOP", EquipItemCategory.ARMOR, ItemGrade.NORMAL, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/5.+top/Normal+Ranger+Top.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WIZARD_TOP", EquipItemCategory.ARMOR, ItemGrade.RARE, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/5.+top/Rare+Wizard+Top.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ELF_TOP", EquipItemCategory.ARMOR, ItemGrade.LEGEND, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/5.+top/Legend+Elf+Top.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WITCH_TOP", EquipItemCategory.ARMOR, ItemGrade.NORMAL, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/5.+top/Normal+Witch+Top.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_DARKELF_TOP", EquipItemCategory.ARMOR, ItemGrade.RARE, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/5.+top/Rare+Darkelf+Top.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_QUEEN_TOP", EquipItemCategory.ARMOR, ItemGrade.LEGEND, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/5.+top/Legend+Queen+Top.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_BOTTOM", EquipItemCategory.PANTS, ItemGrade.NORMAL, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/6.+bottom/Normal+Orc+Bottom.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ORC_BOTTOM", EquipItemCategory.PANTS, ItemGrade.RARE, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/6.+bottom/Rare+Orc+Bottom.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ORC_BOTTOM", EquipItemCategory.PANTS, ItemGrade.LEGEND, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/6.+bottom/Legend+Orc+Bottom.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WARRIOR_BOTTOM", EquipItemCategory.PANTS, ItemGrade.NORMAL, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/6.+bottom/Normal+Warrior+Bottom.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SOLDIER_BOTTOM", EquipItemCategory.PANTS, ItemGrade.RARE, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/6.+bottom/Rare+Soldier+Bottom.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_KNIGHT_BOTTOM", EquipItemCategory.PANTS, ItemGrade.LEGEND, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/6.+bottom/Legend+Knight+Bottom.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_RANGER_BOTTOM", EquipItemCategory.PANTS, ItemGrade.NORMAL, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/6.+bottom/Normal+Ranger+Bottom.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WIZARD_BOTTOM", EquipItemCategory.PANTS, ItemGrade.RARE, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/6.+bottom/Rare+Wizard+Bottom.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ELF_BOTTOM", EquipItemCategory.PANTS, ItemGrade.LEGEND, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/6.+bottom/Legend+Elf+Bottom.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WITCH_BOTTOM", EquipItemCategory.PANTS, ItemGrade.NORMAL, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/6.+bottom/Normal+Witch+Bottom.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_DARKELF_BOTTOM", EquipItemCategory.PANTS, ItemGrade.RARE, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/6.+bottom/Rare+Darkelf+Bottom.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_QUEEN_BOTTOM", EquipItemCategory.PANTS, ItemGrade.LEGEND, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/6.+bottom/Legend+Queen+Bottom.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_ORC_SHOES", EquipItemCategory.SHOES, ItemGrade.NORMAL, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/8.+shoes/Normal+Orc+Boots.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_ORC_SHOES", EquipItemCategory.SHOES, ItemGrade.RARE, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/8.+shoes/Rare+Orc+Boots.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ORC_SHOES", EquipItemCategory.SHOES, ItemGrade.LEGEND, EquipItemJobLimit.INFANTRY, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_Infantry/8.+shoes/Legend+Orc+Boots.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WARRIOR_SHOES", EquipItemCategory.SHOES, ItemGrade.NORMAL, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/8.+shoes/Normal+Warrior+Boots.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_SOLDIER_SHOES", EquipItemCategory.SHOES, ItemGrade.RARE, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/8.+shoes/Rare+Soldier+Boots.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_KNIGHT_SHOES", EquipItemCategory.SHOES, ItemGrade.LEGEND, EquipItemJobLimit.WARRIOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_warrior/8.+shoes/Legend+Knight+Boots.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_RANGER_SHOES", EquipItemCategory.SHOES, ItemGrade.NORMAL, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/8.+shoes/Normal+Ranger+Boots.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_WIZARD_SHOES", EquipItemCategory.SHOES, ItemGrade.RARE, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/8.+shoes/Rare+Wizard+Boots.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_ELF_SHOES", EquipItemCategory.SHOES, ItemGrade.LEGEND, EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/human_ranger/8.+shoes/Legend+Elf+Boots.png");

        addEquipItemData(1, "ID_EQUIP_NAME_NORMAL_WITCH_SHOES", EquipItemCategory.SHOES, ItemGrade.NORMAL, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/8.+shoes/Normal+Witch+Boots.png");
        addEquipItemData(2, "ID_EQUIP_NAME_RARE_DARKELF_SHOES", EquipItemCategory.SHOES, ItemGrade.RARE, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/8.+shoes/Rare+Darkelf+Boots.png");
        addEquipItemData(3, "ID_EQUIP_NAME_LEGEND_QUEEN_SHOES", EquipItemCategory.SHOES, ItemGrade.LEGEND, EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR, EquipItemRaceLimit.ALL, "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/asset+layer/orcs_witchdoctor/8.+shoes/Legend+Queen+Boots.png");

    }

    public void startEquipItemSpriteLoading()
    {
        foreach (EquipItemData data in equipItemDataMap.Values)
        {
            AssetsLoadManager.instance.requestSprite(data.imageUrl, (sprite) => setImageSprite(data, sprite));
        }
    }

    private bool setImageSprite(EquipItemData _data, Sprite _sprite)
    {
        _data.imageSpirte = _sprite;
        equipItemLoadedCount++;
        return true;
    }

    public int getEquipItemSpriteLoadedCount()
    {
        return equipItemLoadedCount;
    }

    public int getEquipItemSpriteLoadedMax()
    {
        return equipItemDataMap.Count;
    }

    public EquipItemData getEquipItemData(int _key)
    {
        if (equipItemDataMap.ContainsKey(_key))
        {
            return equipItemDataMap[_key];
        }
        return null;
    }

    public void addEquipItemData(int _id, string _nameKey, EquipItemCategory _category, ItemGrade _grade, long _jobLimit, long _raceLimit, string _imageUrl)
    {
        EquipItemData data = new EquipItemData(_id, _nameKey, _category, _grade, _jobLimit, _raceLimit, _imageUrl);
        equipItemDataMap.Add(data.generateKey(), data);
        if (Application.isEditor)
        {
            Debug.Log("addEquipItemData() key = " + data.key + ", nameKey = " + data.nameKey);
        }
    }

    public Color getGradeColor(ItemGrade _grade)
    {
        switch (_grade)
        {
            case ItemGrade.COMMON:
                return new Color32(0, 0, 0, 255);
            case ItemGrade.RARE:
                return new Color32(68, 192, 98, 255);
            case ItemGrade.UNCOMMON:
                return new Color32(0, 0, 0, 255);
            case ItemGrade.UNIQUE:
                return new Color32(0, 0, 0, 255);
            case ItemGrade.LEGEND:
                return new Color32(249, 177, 8, 255);
            case ItemGrade.NORMAL:
            default:
                return new Color32(234, 233, 232, 255);
        }
    }

    public EquipItemData getEquipItem(EquipItemCategory _category, CharacterData _data)
    {
        long id = 0;
        switch (_category)
        {
            case EquipItemCategory.HELMET:
                id = _data.equipData.head;
                break;
            case EquipItemCategory.WEAPON:
                id = _data.equipData.weapon;
                break;
            case EquipItemCategory.ACCESSORY:
                id = _data.equipData.accessory;
                break;
            case EquipItemCategory.ARMOR:
                id = _data.equipData.armor;
                break;
            case EquipItemCategory.PANTS:
                id = _data.equipData.pants;
                break;
            case EquipItemCategory.SHOES:
                id = _data.equipData.shoes;
                break;
            default:
                break;
        }
        if (id == 0)
        {
            return null;
        }

        long raceLimit = 0;
        long jobLimit = 0;
        long grade = 0;
        switch(id)
        {
            case 1:
                grade = (int) ItemGrade.NORMAL;
                break;
            case 2:
                grade = (int)ItemGrade.RARE;
                break;
            case 3:
            case 4:
                grade = (int)ItemGrade.LEGEND;
                break;
            case 0:
            default:
                break;

        }

        long category = (int)_category;

        if (_category == EquipItemCategory.WEAPON)
        {
            raceLimit = EquipItemRaceLimit.ALL;

            switch (_data.job)
            {
                case CharacterManager.JOB_ASSASSIN:
                    jobLimit = EquipItemJobLimit.ASSASSIN;
                    break;
                case CharacterManager.JOB_BISHOP:
                    jobLimit = EquipItemJobLimit.BISHOP;
                    break;
                case CharacterManager.JOB_INFANTRY:
                    jobLimit = EquipItemJobLimit.INFANTRY;
                    break;
                case CharacterManager.JOB_RANGER:
                    jobLimit = EquipItemJobLimit.RANGER;
                    break;
                case CharacterManager.JOB_SORCERER:
                    jobLimit = EquipItemJobLimit.SORCERER;
                    break;
                case CharacterManager.JOB_WITCH_DOCTOR:
                    jobLimit = EquipItemJobLimit.WITCH_DOCTOR;
                    break;
                case CharacterManager.JOB_WIZARD:
                    jobLimit = EquipItemJobLimit.WIZARD;
                    break;
                case CharacterManager.JOB_WARRIOR:
                    jobLimit = EquipItemJobLimit.WARRIOR;
                    break;
                default:
                    break;
            }
        } else if (_category == EquipItemCategory.ACCESSORY)
        {
            if (_data.race == CharacterManager.RACE_ELF || _data.race == CharacterManager.RACE_HUMAN)
            {
                raceLimit = EquipItemRaceLimit.ELF | EquipItemRaceLimit.HUMAN;
            } else
            {
                raceLimit = EquipItemRaceLimit.ORC | EquipItemRaceLimit.DARKELF;
            }
            // {
            // 0 => 0 None
            // 1 => 1 NORMAL_EARRING
            // 2 => 3 RARE_EARRING
            // 3 => 5 LEGEND_EARRING
            // 4 => 6 LEGEND_NECKLASS
            // }
            if (id == 2)
            {
                id = 3;
            } else if (id > 2)
            {
                id += 2;
            }
            jobLimit = EquipItemJobLimit.ALL;
        } else
        {
            raceLimit = EquipItemRaceLimit.ALL;

            switch (_data.job)
            {
                case CharacterManager.JOB_WIZARD:
                case CharacterManager.JOB_BISHOP:
                case CharacterManager.JOB_RANGER:
                    jobLimit = EquipItemJobLimit.BISHOP | EquipItemJobLimit.WIZARD | EquipItemJobLimit.RANGER;
                    break;
                case CharacterManager.JOB_SORCERER:
                case CharacterManager.JOB_WITCH_DOCTOR:
                case CharacterManager.JOB_ASSASSIN:
                    jobLimit = EquipItemJobLimit.SORCERER | EquipItemJobLimit.ASSASSIN | EquipItemJobLimit.WITCH_DOCTOR;
                    break;
                case CharacterManager.JOB_INFANTRY:
                    jobLimit = EquipItemJobLimit.INFANTRY;
                    break;
                case CharacterManager.JOB_WARRIOR:
                    jobLimit = EquipItemJobLimit.WARRIOR;
                    break;
                default:
                    break;
            }
        }

        long key = raceLimit * 1000000000 + jobLimit * 1000000 + (int)grade * 100000 + (int)category * 1000 + id;
        Debug.Log("tokenId = " + _data.tokenId + ", category = " + _category + ", key = " + key);
        Debug.Log("raceLimit = " + raceLimit + ", jobLimit = " + jobLimit + ", grade = " + grade + ", category = " + category + ", id = " + id);
        if (!equipItemDataMap.ContainsKey(key))
        {
            return null;
        }
        return equipItemDataMap[key];
    }

}
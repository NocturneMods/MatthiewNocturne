// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using BetterNewGamePlus;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(BetterNewGamePlusMod), "Better New Game Plus (0.6 ver.)", "1.0.0", "Matthiew Purple & xenopiece")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace BetterNewGamePlus;
public class BetterNewGamePlusMod : MelonMod
{
    public enum NgpActivationCondition
    {
        Disabled,
        Hooded,
        Leather,
        Always,
    }

    public enum NgpKeepItemsType
    {
        All,
        Key
    },


    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "BetterNewGamePlus.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<NgpActivationCondition> s_cfgKeepItems = null!;
    private static MelonPreferences_Entry<NgpKeepItemsType> s_cfgKeepItemsType = null!; 
    private static MelonPreferences_Entry<NgpActivationCondition> s_cfgKeepMacca = null!;
    private static MelonPreferences_Entry<NgpActivationCondition> s_cfgKeepMagatama = null!;
    private static MelonPreferences_Entry<NgpActivationCondition> s_cfgKeepDemons = null!;
    private static MelonPreferences_Entry<NgpActivationCondition> s_cfgKeepMaxStock = null!;

    private static datUnitWork_t[] s_partyList = new datUnitWork_t[12];
    private static int s_stockCount;
    private static int[] s_stockList = Array.Empty<int>();
    private static byte[] s_itemList = Array.Empty<byte>();
    private static byte[] s_magatamaList = Array.Empty<byte>();
    private static int s_macca = 0;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("BetterNewGamePlus");
        s_cfgKeepItems = s_cfgCategoryMain.CreateEntry("KeepItems", NgpActivationCondition.Disabled, "Keep items on NG+", description: "Keep items on NG+ (Disabled/Hooded/Leather/Always)");
        s_cfgKeepItemsType = s_cfgCategoryMain.CreateEntry("KeepItemsType", NgpKeepItemsType.All, "Item type to keep", description: "The type of items to keep if the option is enabled. (All/Key)");
        s_cfgKeepMacca = s_cfgCategoryMain.CreateEntry("KeepMacca", NgpActivationCondition.Disabled, "Keep macca on NG+", description: "Keep macca on NG+ (Disabled/Hooded/Leather/Always)");
        s_cfgKeepMagatama = s_cfgCategoryMain.CreateEntry("KeepMagatama", NgpActivationCondition.Disabled, "Keep magatama on NG+", description: "Keep magatama on NG+ (Disabled/Hooded/Leather/Always)");
        s_cfgKeepDemons = s_cfgCategoryMain.CreateEntry("KeepDemons", NgpActivationCondition.Disabled, "Keep demons on NG+", description: "Keep demons on NG+ (Disabled/Hooded/Leather/Always)");
        s_cfgKeepMaxStock = s_cfgCategoryMain.CreateEntry("KeepMaxStock", NgpActivationCondition.Disabled, "Keep max stock size on NG+", description: "Keep max stock size on NG+, forcibly enabled if KeepDemons is enabled. (Disabled/Hooded/Leather/Always)");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    private static bool IsConditionMet(NgpActivationCondition cdt, bool leatherJacket)
    {
        return cdt == NgpActivationCondition.Always
            || cdt == NgpActivationCondition.Leather && leatherJacket
            || cdt == NgpActivationCondition.Hooded == !leatherJacket;
    }

    // When destroying a savefile for a NG+
    [HarmonyPatch(typeof(dds3GlobalWork), nameof(dds3GlobalWork.NewGame2Push))]
    private class Patch
    {
        public static void Postfix()
        {
            rstCalcCore.cmbSubHeartsParam(dds3GlobalWork.DDS3_GBWK.heartsequip, dds3GlobalWork.DDS3_GBWK.unitwork[0]); // Remove Demi-fiend's current Magatama
            s_partyList = dds3GlobalWork.DDS3_GBWK.unitwork; // Demi-fiend and his demons in party and stock
            s_stockCount = dds3GlobalWork.DDS3_GBWK.stockcnt; // Number of demons in stock
            s_stockList = dds3GlobalWork.DDS3_GBWK.stocklist; // Demons in stock

            s_itemList = dds3GlobalWork.DDS3_GBWK.item; // Items

            s_macca = dds3GlobalWork.DDS3_GBWK.maka; // Macca

            s_magatamaList = dds3GlobalWork.DDS3_GBWK.hearts; // Collected Magamata
        }
    }

    // When creating a NG+
    [HarmonyPatch(typeof(dds3GlobalWork), nameof(dds3GlobalWork.NewGame2Pop))]
    private class Patch2
    {
        public static void Postfix()
        {

        }
    }

    [HarmonyPatch(typeof(EventBit), nameof(EventBit.evtBitOn))]
    private class Patch4
    {
        public static void Postfix(ref int no)
        {
            // TODO: check that if no != 2224 => hooded jacked (and ONLY this event)
            // If the leather jacket was selected
            bool leatherJacket = no == 2224;

            // If keeping stock
            if (IsConditionMet(s_cfgKeepMaxStock.Value, leatherJacket))
            {
                dds3GlobalWork.DDS3_GBWK.maxstock = 12;
            }

            // If keeping demons
            if (IsConditionMet(s_cfgKeepDemons.Value, leatherJacket))
            {
                dds3GlobalWork.DDS3_GBWK.unitwork = s_partyList; // Demi-fiend and his demons in party and stock
                dds3GlobalWork.DDS3_GBWK.maxstock = 12; // Maximum number of slots in stock
                dds3GlobalWork.DDS3_GBWK.stockcnt = s_stockCount; // Number of demons in stock
                dds3GlobalWork.DDS3_GBWK.stocklist = s_stockList; // Demons in stock
            }

            // If keeping items
            if (IsConditionMet(s_cfgKeepItems.Value, leatherJacket))
            {
                dds3GlobalWork.DDS3_GBWK.item = s_itemList; // Items
            }

            // If keeping macca
            if (IsConditionMet(s_cfgKeepMacca.Value, leatherJacket))
            {
                dds3GlobalWork.DDS3_GBWK.maka = s_macca; // Macca
            }

            // If keeping magatama
            if (IsConditionMet(s_cfgKeepMagatama.Value, leatherJacket))
            {
                foreach (byte item in s_magatamaList) // Collected Magatama
                {
                    datCalc.datAddHearts(item);
                }
            }
        }
    }
}

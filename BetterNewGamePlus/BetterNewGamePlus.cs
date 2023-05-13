// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using BetterNewGamePlus;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using MelonLoader;

[assembly: MelonInfo(typeof(BetterNewGamePlusMod), "Better New Game Plus (0.6 ver.)", "1.0.0", "Matthiew Purple & xenopiece")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace BetterNewGamePlus;
public class BetterNewGamePlusMod : MelonMod
{
    private static datUnitWork_t[] s_partyList = new datUnitWork_t[12];
    private static int s_stockCount;
    private static int[] s_stockList = Array.Empty<int>();
    private static byte[] s_itemList = Array.Empty<byte>();
    private static byte[] s_magatamaList = Array.Empty<byte>();
    private static int s_macca = 0;

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
            dds3GlobalWork.DDS3_GBWK.unitwork = s_partyList; // Demi-fiend and his demons in party and stock
            dds3GlobalWork.DDS3_GBWK.maxstock = 12; // Maximum number of slots in stock
            dds3GlobalWork.DDS3_GBWK.stockcnt = s_stockCount; // Number of demons in stock
            dds3GlobalWork.DDS3_GBWK.stocklist = s_stockList; // Demons in stock

            dds3GlobalWork.DDS3_GBWK.item = s_itemList; // Items

            dds3GlobalWork.DDS3_GBWK.maka = s_macca; // Macca

            foreach (byte item in s_magatamaList) // Collected Magatama
            {
                datCalc.datAddHearts(item);
            }
        }
    }
}

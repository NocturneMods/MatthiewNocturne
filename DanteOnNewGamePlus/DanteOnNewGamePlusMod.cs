// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using DanteOnNewGamePlus;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using MelonLoader;

[assembly: MelonInfo(typeof(DanteOnNewGamePlusMod), "Dante/Raidou on New Game Plus [level 1]", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace DanteOnNewGamePlus;
public class DanteOnNewGamePlusMod : MelonMod
{
    // When creating a NG+
    [HarmonyPatch(typeof(dds3GlobalWork), nameof(dds3GlobalWork.NewGame2Pop))]
    private class Patch
    {
        public static void Postfix()
        {
            // If Dante/Raidou not already in party
            foreach (datUnitWork_t work in dds3GlobalWork.DDS3_GBWK.unitwork)
            {
                if (work.id == 192)
                {
                    return; // Does nothing
                }
            }

            // If he's not in the party already, adds him
            datCalc.datAddDevil(192, 0);

            // Changes his stats to be those of a level 1 demon
            foreach (datUnitWork_t work in dds3GlobalWork.DDS3_GBWK.unitwork)
            {
                if (work.id == 192)
                {
                    work.level = 1;
                    work.exp = 0;

                    work.param[0] = 8;
                    work.param[2] = 6;
                    work.param[3] = 6;
                    work.param[4] = 8;
                    work.param[5] = 3;

                    work.hp = 42;
                    work.maxhp = 42;
                    work.mp = 21;
                    work.maxmp = 21;
                }
            }
        }
    }

    // Uses Demi-fiend exp curve for Dante/Raidou (until level 80)
    [HarmonyPatch(typeof(rstCalcCore), nameof(rstCalcCore.cmbCalcLevelUpExp))]
    private class Patch2
    {
        public static void Postfix(ref datUnitWork_t pStock, ref int Level, ref uint __result)
        {
            if (pStock.id == 192)
            {
                datUnitWork_t demifiend = dds3GlobalWork.DDS3_GBWK.unitwork[0];
                if (pStock.level < 80)
                {
                    __result = rstCalcCore.cmbCalcLevelUpExp(ref demifiend, Level);
                }
                else if (pStock.level == 80 && pStock.exp > 40000)
                {
                    pStock.exp -= rstCalcCore.cmbCalcLevelUpExp(ref demifiend, 80);
                }
            }
        }
    }

    // Displays Demi-fiend exp curve for Dante/Raidou (until level 80)
    [HarmonyPatch(typeof(rstCalcCore), nameof(rstCalcCore.GetNextExpDisp))]
    private class Patch3
    {
        public static void Postfix(ref datUnitWork_t pStock, ref uint now_exp, ref uint __result)
        {
            if (pStock.id == 192)
            {
                if (pStock.level < 80)
                {
                    __result = rstCalcCore.GetNextExpDisp(dds3GlobalWork.DDS3_GBWK.unitwork[0], now_exp);
                }
            }
        }
    }
}

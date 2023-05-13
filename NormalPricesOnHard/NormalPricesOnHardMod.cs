// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using NormalPricesOnHard;

[assembly: MelonInfo(typeof(NormalPricesOnHardMod), "Normal prices on Hard (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace NormalPricesOnHard;
public class NormalPricesOnHardMod : MelonMod
{
    [HarmonyPatch(typeof(fclShopCalc), nameof(fclShopCalc.shpCalcItemPrice))]
    private class Patch
    {
        public static void Postfix(ref sbyte Mode, ref int __result)
        {
            // If buying on Hard then multiply prices by 1/3
            if (Mode == 0 && dds3ConfigMain.cfgGetBit(9u) == 2)
            {
                __result = __result / 3;
            }
        }
    }
}

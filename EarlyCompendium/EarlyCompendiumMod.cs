// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using EarlyCompendium;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(EarlyCompendiumMod), "Early Compendium", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace EarlyCompendium;
public class EarlyCompendiumMod : MelonMod
{
    // After checking for the state of a flag
    [HarmonyPatch(typeof(EventBit), nameof(EventBit.evtBitCheck))]
    private class Patch
    {
        public static void Postfix(ref int no, ref bool __result)
        {
            if (no == 40)
            {
                __result = true; // Compendium's flag always returns true
            }
        }
    }
}

// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PiercingMagmaAxis;

[assembly: MelonInfo(typeof(PiercingMagmaAxisMod), "Piercing Magma Axis [including Repel] (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace PiercingMagmaAxis;
public class PiercingMagmaAxisMod : MelonMod
{
    // After getting the effectiveness of an attack
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetAisyo))]
    private class Patch
    {
        public static void Postfix(ref int nskill, ref uint __result)
        {
            // If the attack is Magma Axis and it's resisted/blocked/drained/repelled
            if (nskill == 161 && (__result < 100 || (__result > 999 && __result < 1000000000)))
            {
                __result = 100; // Forces the affinity to become "neutral"
                nbMainProcess.nbGetMainProcessData().d31_kantuu = 1; // Displays the "Pierced!" message
            }
        }
    }

    // After getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch2
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 161)
            {
                __result = "Fire damage to one foe. \nIgnores all resistances."; // New skill description of Magma Axis
            }
        }
    }
}

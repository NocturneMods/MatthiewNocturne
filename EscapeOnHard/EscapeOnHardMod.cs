// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using EscapeOnHard;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewbattle_H;
using MelonLoader;
using Random = System.Random;

[assembly: MelonInfo(typeof(EscapeOnHardMod), "Escape on Hard [low odds] (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace EscapeOnHard;
public class EscapeOnHardMod : MelonMod
{
    private static bool s_temporaryNormalMode = false; // Remembers if the game was on Hard difficulty before switching to Normal

    // Before checking if the player can escape
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbCheckEscape))]
    private class Patch
    {
        public static void Prefix()
        {
            // If the game is on Hard difficulty
            if (dds3ConfigMain.cfgGetBit(9u) == 2)
            {
                dds3ConfigMain.cfgSetBit(9u, 1); // Switches the game to Normal
                s_temporaryNormalMode = true; // Remembers that it's only temporary
            }
        }
    }

    // After checking if the player can escape
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbCheckEscape))]
    private class Patch2
    {
        public static void Postfix(ref nbMainProcessData_t data, ref int __result)
        {
            // If Demi-fiend is the one escaping and he has Fast Retreat
            if (data.activeunit == 0 && datCalc.datCheckSkillInParty(296) == 1)
            {
                __result = 1; // Always escape successfully
            }

            // If on Normal mode temporarily
            else if (s_temporaryNormalMode)
            {
                // If the escape is supposed to be successful then flip a coin
                if (__result == 1)
                {
                    __result = new Random().Next(0, 2);
                }

                // Switches back to Hard mode
                dds3ConfigMain.cfgSetBit(9u, 2);
            }

            s_temporaryNormalMode = false;
        }
    }

    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch3
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 296)
            {
                __result = "Guarantees escape \nduring user's turn."; // New description for Fast Retreat
            }
        }
    }
}

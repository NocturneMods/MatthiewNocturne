// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using Il2Cppnewbattle_H;
using MelonLoader;
using NoInterruptions;

[assembly: MelonInfo(typeof(NoInterruptionsMod), "No interruptions", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace NoInterruptions;
public class NoInterruptionsMod : MelonMod
{
    private static short s_tmp_enemypcnt = 0; // Remembers the number of enemies before modification

    // After initiating a negotiation sequence
    [HarmonyPatch(typeof(nbNegoProcess), nameof(nbNegoProcess.InitNegoProcessData))]
    private class Patch2
    {
        public static void Postfix(ref nbActionProcessData_t actdata)
        {
            // Changes the numbers of enemies to 1 so the game never triggers an interruption
            s_tmp_enemypcnt = actdata.data.enemypcnt;
            actdata.data.enemypcnt = 1;
        }
    }

    // After displaying a negotiation message
    [HarmonyPatch(typeof(nbNegoProcess), nameof(nbNegoProcess.nbDispNegoMessage))]
    private class Patch5
    {
        public static void Prefix()
        {
            // If the number of enemies has been modified
            if (s_tmp_enemypcnt != 0)
            {
                // Reverts it to its previous value
                nbMainProcess.nbGetMainProcessData().enemypcnt = s_tmp_enemypcnt;
                s_tmp_enemypcnt = 0;
            }
        }
    }
}

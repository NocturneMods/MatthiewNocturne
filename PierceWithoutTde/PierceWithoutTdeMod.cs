// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using PierceWithoutTde;

[assembly: MelonInfo(typeof(PierceWithoutTdeMod), "Pierce without TDE (ver. 0.6)", "1.0.1", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace PierceWithoutTde;
public class PierceWithoutTdeMod : MelonMod
{
    // After checking for a flag state
    [HarmonyPatch(typeof(EventBit), nameof(EventBit.evtBitCheck))]
    private class Patch
    {
        public static void Postfix(ref int no, ref bool __result)
        {
            // Checks the flag responsible for unlocking Pierce
            if (no == 2241)
            {
                if (!__result)
                {
                    __result = true; // Artificially makes it obtainable
                }
                else
                {
                    tblHearts.fclHeartsTbl[1].Skill[5].TargetLevel = 21; // If unlocked normally, you can get it early
                }
            }
        }
    }

    // When booting the game
    public override void OnInitializeMelon()
    {
        tblHearts.fclHeartsTbl[1].Skill[5].TargetLevel = 80; // Make Pierce obtainable at level 80 instead of level 21
    }
}

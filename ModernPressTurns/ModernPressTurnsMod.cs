// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using ModernPressTurns;

[assembly: MelonInfo(typeof(ModernPressTurnsMod), "Modern Press Turns [SMT V] (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace ModernPressTurns;
public class ModernPressTurnsMod : MelonMod
{
    // PASS
    [HarmonyPatch(typeof(nbActionProcess), nameof(nbActionProcess.SetAction_WAIT))]
    private class Patch
    {
        public static void Prefix()
        {
            // If there are blinking press turns AND full press turns
            if (nbMainProcess.nbGetMainProcessData().press4_ten != nbMainProcess.nbGetMainProcessData().press4_p && nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
            }

            // If there are only full press turns or only blinking press turns, the default behavior is the same as in SMT V
        }
    }

    // DISMISS
    [HarmonyPatch(typeof(nbActionProcess), nameof(nbActionProcess.SetAction_RETURN))]
    private class Patch2
    {
        public static void Postfix()
        {
            // If there is at least one full press turn
            if (nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                // If there are no blinking press turns
                if (nbMainProcess.nbGetMainProcessData().press4_p == nbMainProcess.nbGetMainProcessData().press4_ten)
                {
                    PressTurnsAdjustements.MainFullToBlinking(); // Changes the main press turn from full to blinking
                }

                // If there is at least one blinking press turn
                else
                {
                    PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
                }
            }

            // If there are no full press turns, the default behavior is the same as in SMT V
        }
    }

    // DISMISS (removes the 10 frames delay in press turn consumption for aesthetic reasons)
    [HarmonyPatch(typeof(nbMakePacket), nameof(nbMakePacket.nbMakeNewPressPacket))]
    private class Patch22
    {
        public static void Prefix(ref int startframe, ref int ptype)
        {
            if (ptype == 9)
            {
                startframe = 0; // If using DISMISS then create the PressPacket immediately
            }
        }
    }

    // SUMMON (also works when enemies do it)
    [HarmonyPatch(typeof(nbKoukaProcess), nameof(nbKoukaProcess.RunSummonKouka))]
    private class Patch3
    {
        public static void Prefix()
        {
            // If there is at least one full press turn
            if (nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                // If there are no blinking press turns
                if (nbMainProcess.nbGetMainProcessData().press4_p == nbMainProcess.nbGetMainProcessData().press4_ten)
                {
                    PressTurnsAdjustements.MainFullToBlinking(); // Changes the main press turn from full to blinking
                }

                // If there is at least one blinking press turn
                else
                {
                    PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
                }
            }

            // If there are no full press turns, the default behavior is the same as in SMT V
        }
    }

    // ANALYZE
    [HarmonyPatch(typeof(nbPanelProcess), nameof(nbPanelProcess.nbPanelAnalyzeStart))]
    private class Patch4
    {
        public static void Postfix()
        {
            // If there is at least one full press turn
            if (nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                PressTurnsAdjustements.SecondaryFullToBlinking(); // Only this line is required as the game consumes the press turn AFTER this is called
            }
        }
    }

    public static class PressTurnsAdjustements
    {
        public static void MainFullToBlinking()
        {
            // The game automatically consumes the main full press turn
            nbMainProcess.nbGetMainProcessData().press4_ten++; // Adds a blinking press turn
        }

        public static void SecondaryFullToBlinking()
        {
            // The game automatically consumes the main blinking press turn
            nbMainProcess.nbGetMainProcessData().press4_ten++; // Adds the blinking press turn back
            nbMainProcess.nbGetMainProcessData().press4_p--; // Converts the first full press turn into a blinking press turn
        }
    }
}

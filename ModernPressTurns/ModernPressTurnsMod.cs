// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using ModernPressTurns;

[assembly: MelonInfo(typeof(ModernPressTurnsMod), "Modern Press Turns [SMT V] (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace ModernPressTurns;
public class ModernPressTurnsMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "ModernPressTurns.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<bool> s_cfgSmt5HalfPtBehaviour = null!;
    private static MelonPreferences_Entry<bool> s_cfgPassCostsHalfPt = null!;
    private static MelonPreferences_Entry<bool> s_cfgDismissCostsHalfPt = null!;
    private static MelonPreferences_Entry<bool> s_cfgSummonCostsHalfPt = null!;
    private static MelonPreferences_Entry<bool> s_cfgAnalyzeCostsHalfPt = null!;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("ModernPressTurns");
        s_cfgSmt5HalfPtBehaviour = s_cfgCategoryMain.CreateEntry<bool>("Smt5HalfPtBehaviour", false, "SMTV Half press turns behaviour", description: "Use SMTV press turn behaviour : for half PT consumption, prioritize available full PTs and only consume press turns when there are none full available.");
        s_cfgPassCostsHalfPt = s_cfgCategoryMain.CreateEntry<bool>("PassCostsHalfPt", false, "Pass costs half a press turn", description: "Using Pass in combat consumes only a half press turn.");
        s_cfgDismissCostsHalfPt = s_cfgCategoryMain.CreateEntry<bool>("DismissCostsHalfPt", false, "Dismiss costs half a press turn", description: "Dismissing a demon in combat consumes only a half press turn.");
        s_cfgSummonCostsHalfPt = s_cfgCategoryMain.CreateEntry<bool>("SummonCostsHalfPt", false, "Summon costs half a press turn", description: "Summoning a demon in combat consumes only a half press turn.");
        s_cfgAnalyzeCostsHalfPt = s_cfgCategoryMain.CreateEntry<bool>("AnalyzeCostsHalfPt", false, "The Analyze skill costs half a press turn", description: "Using the Analyze skill consumes only a half press turn.");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    // PASS TURN
    [HarmonyPatch(typeof(nbActionProcess), nameof(nbActionProcess.SetAction_WAIT))]
    private class Patch
    {
        public static void Prefix()
        {
            // Only proceed if the configuration requires it
            if (!s_cfgPassCostsHalfPt.Value)
            {
                return;
            }

            // If there are blinking press turns AND full press turns
            if (nbMainProcess.nbGetMainProcessData().press4_ten != nbMainProcess.nbGetMainProcessData().press4_p && nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
            }

            // If there are only full press turns or only blinking press turns, the default behavior is the same as in SMT V
        }
    }

    // DISMISS DEMON
    [HarmonyPatch(typeof(nbActionProcess), nameof(nbActionProcess.SetAction_RETURN))]
    private class Patch2
    {
        public static void Postfix()
        {
            // Only proceed if the configuration requires it
            if (!s_cfgDismissCostsHalfPt.Value)
            {
                return;
            }

            // If there is at least one full press turn (if only blinking left, just apply vanilla behaviour)
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
                    // And we want to have smt5 half press turn behaviour
                    if (s_cfgSmt5HalfPtBehaviour.Value)
                    {
                        PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
                    }
                }
            }

            // If there are no full press turns, the default behavior is the same as in SMT V
        }
    }

    // DISMISS DEMON (removes the 10 frames delay in press turn consumption for aesthetic reasons)
    [HarmonyPatch(typeof(nbMakePacket), nameof(nbMakePacket.nbMakeNewPressPacket))]
    private class Patch22
    {
        public static void Prefix(ref int startframe, ref int ptype)
        {
            // Only proceed if the configuration requires it
            if (!s_cfgDismissCostsHalfPt.Value)
            {
                return;
            }

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
            // Only proceed if the configuration requires it
            if (!s_cfgSummonCostsHalfPt.Value)
            {
                return;
            }

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
                    // And we want to have smt5 half press turn behaviour
                    if (s_cfgSmt5HalfPtBehaviour.Value)
                    {
                        PressTurnsAdjustements.SecondaryFullToBlinking(); // Changes a secondary full press turn into a blinking press turn
                    }
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
            // Only proceed if the configuration requires it
            if (!s_cfgAnalyzeCostsHalfPt.Value)
            {
                return;
            }

            // If there is at least one full press turn
            if (nbMainProcess.nbGetMainProcessData().press4_p != 0)
            {
                PressTurnsAdjustements.SecondaryFullToBlinking(); // Only this line is required as the game consumes the press turn AFTER this is called
            }
        }
    }

    public static class PressTurnsAdjustements
    {
        // p=full : number of full press turn
        // ten=total : number of press turn left (INCLUDING half press turns)
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

// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using EscapeOnHard;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewbattle_H;
using MelonLoader;
using MelonLoader.Utils;
using Random = System.Random;

[assembly: MelonInfo(typeof(EscapeOnHardMod), "Escape on Hard [low odds] (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace EscapeOnHard;
public class EscapeOnHardMod : MelonMod
{

    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "EscapeOnHard.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<bool> s_cfgUseNormalEscapeOnHard = null!;
    private static MelonPreferences_Entry<float> s_cfgLowerEscapeRate = null!;
    private static MelonPreferences_Entry<bool> s_cfgLowerEscapeRateHardOnly = null!;
    private static MelonPreferences_Entry<float> s_cfgFastRetreatHigherRate = null!;

    private static bool s_temporaryNormalMode = false; // Remembers if the game was on Hard difficulty before switching to Normal

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("EscapeOnHard");
        s_cfgUseNormalEscapeOnHard = s_cfgCategoryMain.CreateEntry<bool>("UseNormalEscapeOnHard", false, "Normal difficulty escape rate in Hard", description: "Use Normal difficulty escape rate in Hard difficulty");
        s_cfgLowerEscapeRateHardOnly = s_cfgCategoryMain.CreateEntry<bool>("LowerEscapeRateHardOnly", false, "Lower escape rate only in Hard", description: "Enables the lowers escape rate percentage only in hard mode.");
        s_cfgLowerEscapeRate = s_cfgCategoryMain.CreateEntry<float>("LowerEscapeRate", 0.0f, "Lower escape rate", description: "Lowers the vanilla escape rate. If the escape should work you get an bonus x% chance to fail the escape. 0% is vanilla escape rate, 100% is impossible to escape. Consider enabling UseNormalEscapeOnHard if you tweak this to keep reasonable escape odds.");
        s_cfgFastRetreatHigherRate = s_cfgCategoryMain.CreateEntry<float>("FastRetreatHigherRate", 0.0f, "Higher Fast Retreat escape rate", description: "Increase the odds of escaping with Fast Retreat. If the escape should fail, you get an additional x% percent chance to succeed escaping the battle. 100% makes the skill always work.");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    // Before checking if the player can escape
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbCheckEscape))]
    private class Patch
    {
        public static void Prefix()
        {
            // If the game is on Hard difficulty and we want to use normal difficulty escape rate
            if (dds3ConfigMain.cfgGetBit(9u) == 2 && s_cfgUseNormalEscapeOnHard.Value)
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
        public static void Postfix(ref int __result)
        {
            // If Demi-fiend is the one escaping and he has Fast Retreat and he would fail the escape
            if (datCalc.datCheckSkillInParty(296) == 1 && __result == 0)
            {
                bool escapedSecondRoll = MelonUtils.RandomDouble() * 100 <= (double)s_cfgFastRetreatHigherRate.Value;
                __result = escapedSecondRoll ? 1 : 0;
            }

            // If the escape is supposed to be successful then reroll with alternate probabilities
            // only if we want it to happen anytime, or if we want it to happen only in hard and we 
            // are in temporary normal mode (=in hard the rest of the time)
            if (__result == 1 && (!s_cfgLowerEscapeRateHardOnly.Value || s_temporaryNormalMode))
            {
                bool escapedSecondRoll = MelonUtils.RandomDouble() * 100 <= (double)s_cfgLowerEscapeRate.Value;
                __result = escapedSecondRoll ? 0 : 1;
            }

            // If on Normal mode temporarily
            if (s_temporaryNormalMode)
            {
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
                __result = "Guarantees escape \nwhen possible."; // New description for Fast Retreat
            }
        }
    }
}

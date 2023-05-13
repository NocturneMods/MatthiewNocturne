// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using EveryoneGetsExp;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(EveryoneGetsExpMod), "Everyone Gets Experience 25% (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace EveryoneGetsExp;
public class EveryoneGetsExpMod : MelonMod
{
    public const string ConfigPath = "ModsCfg/EveryoneGetsExp.toml";

    private static uint[] s_unitExpList = Array.Empty<uint>();

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<float> s_cfgSharedXp = null!;
    private static MelonPreferences_Entry<float> s_cfgSharedXpWatchful = null!;

    public override void OnInitializeMelon()
    {
        s_cfgCategoryMain = MelonPreferences.CreateCategory("EveryoneGetsExp");
        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgSharedXp = s_cfgCategoryMain.CreateEntry<float>("SharedXp", 50.0f, "Shared XP", description: "How much XP goes to the party members, in percents.");
        s_cfgSharedXpWatchful = s_cfgCategoryMain.CreateEntry<float>("SharedXpWatchful", 50.0f, "Shared XP Watchful", description: "In addition to the default 50% XP given to the team members with Watchful, how much additional XP should they get, in percents.");
    }

    [HarmonyPatch(typeof(nbResultProcess), nameof(nbResultProcess.nbResultLoad))]
    private class Patch
    {
        public static void Prefix()
        {
            // Registers everyone's exp after exp attribution at the end of a battle
            int teamSize = dds3GlobalWork.DDS3_GBWK.unitwork.Length;
            s_unitExpList = new uint[teamSize];
            for (int i = 0; i < teamSize; i++)
            {
                s_unitExpList[i] = dds3GlobalWork.DDS3_GBWK.unitwork[i].exp;
            }
        }
    }

    [HarmonyPatch(typeof(nbResultProcess), nameof(nbResultProcess.nbResultShutdown))]
    private class Patch2
    {
        public static void Postfix()
        {
            // Give passive demons either 25% or 100% exp if they have Watchful
            for (int i = 0; i < dds3GlobalWork.DDS3_GBWK.unitwork.Length; i++)
            {
                if (dds3GlobalWork.DDS3_GBWK.unitwork[i].hp > 0)
                {
                    // If a demon didn't get any exp yet (i.e. if it's a passive demon without Watchful)
                    if (s_unitExpList[i] == dds3GlobalWork.DDS3_GBWK.unitwork[i].exp)
                    {
                        datCalc.datAddExp(dds3GlobalWork.DDS3_GBWK.unitwork[i], (int)(nbResultProcess.AllExp * s_cfgSharedXp.Value / 100)); // Get 25% exp
                    }
                    // If they didn't get 100% exp and have Watchful
                    else if (s_unitExpList[i] + nbResultProcess.AllExp != dds3GlobalWork.DDS3_GBWK.unitwork[i].exp && dds3GlobalWork.DDS3_GBWK.unitwork[i].skill.Contains(354))
                    {
                        datCalc.datAddExp(dds3GlobalWork.DDS3_GBWK.unitwork[i], (int)Math.Ceiling(nbResultProcess.AllExp * s_cfgSharedXpWatchful.Value / 100)); // Get 50% exp (+ vanilla 50% exp)
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch3
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 354)
            {
                __result = $"Earn {50f + s_cfgSharedXpWatchful.Value}% EXP when \nnot participating in battle."; // Overwrites Watchful's skill description
            }
        }
    }
}

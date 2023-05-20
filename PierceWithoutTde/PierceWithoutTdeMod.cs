// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using PierceWithoutTde;

[assembly: MelonInfo(typeof(PierceWithoutTdeMod), "Pierce without TDE (ver. 0.6)", "2.0.0", "Matthiew Purple & Kraby")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace PierceWithoutTde;
public class PierceWithoutTdeMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "PierceWithoutTde.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<byte> s_cfgUnlockLevelCustom = null!;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("PierceWithoutTde");
        s_cfgUnlockLevelCustom = s_cfgCategoryMain.CreateEntry<byte>("UnlockLevelCustom", 255, "Unlock level of Pierce", description: "Unlock level of Pierce without any condition.");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();

        if (s_cfgUnlockLevelCustom.Value > 0)
        {
            tblHearts.fclHeartsTbl[1].Skill[5].TargetLevel = s_cfgUnlockLevelCustom.Value; // Make Pierce obtainable at level 80 instead of level 21
        }
    }

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
}

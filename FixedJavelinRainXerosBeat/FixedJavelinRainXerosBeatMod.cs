// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using FixedJavelinRainXerosBeat;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(FixedJavelinRainXerosBeatMod), "Fixed Javelin Rain and Xeros Beat", "1.0.0", "Matthiew Purple & Kraby")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace FixedJavelinRainXerosBeat;
public class FixedJavelinRainXerosBeatMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "FixedJavelinRainXerosBeat.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<bool> s_cfgFix = null!;
    private static MelonPreferences_Entry<byte> s_cfgXerosBeatAilmentRate = null!;
    private static MelonPreferences_Entry<byte> s_cfgJavelinRainAilmentRate = null!;

    private const int JavelinId = 143;
    private const int XerosId = 133;
    private static int NewJavelinId => s_cfgFix.Value ? 133 : 143;
    private static int NewXerosId => s_cfgFix.Value ? 143 : 133;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("FixedJavelinRainXerosBeat");
        s_cfgFix = s_cfgCategoryMain.CreateEntry("Fix", false, "Fix Javelin Rain & Xeros Beat inversion", description: "Fix Javelin Rain & Xeros Beat inversion.");
        s_cfgXerosBeatAilmentRate = s_cfgCategoryMain.CreateEntry<byte>("XerosBeatAilmentRate", 30, "Xeros Beat ailment rate", description: "Xeros Beat ailment rate");
        s_cfgJavelinRainAilmentRate = s_cfgCategoryMain.CreateEntry<byte>("JavelinRainAilmentRate", 20, "Javelin Rain ailment rate", description: "Javelin Rain ailment rate");
        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();

        // Swaps the "datNormalSkills" objects
        (datNormalSkill.tbl[NewXerosId], datNormalSkill.tbl[NewJavelinId]) = (datNormalSkill.tbl[XerosId], datNormalSkill.tbl[JavelinId]);

        // Swaps the "datSkills" objects
        (datSkill.tbl[NewXerosId], datSkill.tbl[NewJavelinId]) = (datSkill.tbl[XerosId], datSkill.tbl[JavelinId]);

        // Swaps the Magatamas they belong to
        (tblHearts.fclHeartsTbl[15].Skill[2], tblHearts.fclHeartsTbl[20].Skill[2]) = (tblHearts.fclHeartsTbl[20].Skill[2], tblHearts.fclHeartsTbl[15].Skill[2]);

        // Buff their ailment rate
        datNormalSkill.tbl[NewXerosId].badlevel = s_cfgXerosBeatAilmentRate.Value;
        datNormalSkill.tbl[NewJavelinId].badlevel = s_cfgJavelinRainAilmentRate.Value;
    }

    // Before getting the name of a skill
    [HarmonyPatch(typeof(datSkillName), nameof(datSkillName.Get))]
    private class Patch
    {
        public static void Prefix(ref int id)
        {
            // Swaps the skills names
            if (id == JavelinId)
            {
                id = NewJavelinId;
            }
            else if (id == XerosId)
            {
                id = NewXerosId;
            }
        }
    }

    // Before getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch2
    {
        public static void Prefix(ref int id)
        {
            // Swaps the skills names
            if (id == JavelinId)
            {
                id = NewJavelinId;
            }
            else if (id == XerosId)
            {
                id = NewXerosId;
            }
        }
    }
}

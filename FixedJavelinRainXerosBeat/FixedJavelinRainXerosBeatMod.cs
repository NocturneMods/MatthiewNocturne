// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using FixedJavelinRainXerosBeat;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(FixedJavelinRainXerosBeatMod), "Fixed Javelin Rain and Xeros Beat", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace FixedJavelinRainXerosBeat;
public class FixedJavelinRainXerosBeatMod : MelonMod
{
    // Before getting the name of a skill
    [HarmonyPatch(typeof(datSkillName), nameof(datSkillName.Get))]
    private class Patch
    {
        public static void Prefix(ref int id)
        {
            // Swaps the skills names
            if (id == 133)
            {
                id = 143;
            }
            else if (id == 143)
            {
                id = 133;
            }
        }
    }

    // Before getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch2
    {
        public static void Prefix(ref int id)
        {
            // Swaps the skills descriptions
            if (id == 133)
            {
                id = 143;
            }
            else if (id == 143)
            {
                id = 133;
            }
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Swaps the "datNormalSkills" objects
        (datNormalSkill.tbl[143], datNormalSkill.tbl[133]) = (datNormalSkill.tbl[133], datNormalSkill.tbl[143]);

        // Swaps the "datSkills" objects
        (datSkill.tbl[143], datSkill.tbl[133]) = (datSkill.tbl[133], datSkill.tbl[143]);

        // Swaps the Magatamas they belong to
        (tblHearts.fclHeartsTbl[15].Skill[2], tblHearts.fclHeartsTbl[20].Skill[2]) = (tblHearts.fclHeartsTbl[20].Skill[2], tblHearts.fclHeartsTbl[15].Skill[2]);
    }
}

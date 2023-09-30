// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using FiendsNerf;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(FiendsNerfMod), "Fiends nerf", "1.0.1", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace FiendsNerf;
public class FiendsNerfMod : MelonMod
{
    private static bool s_isAnalyzing = false;

    // After getting the affinities of a demon
    [HarmonyPatch(typeof(datAisyoName), nameof(datAisyoName.Get))]
    private class Patch
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (!s_isAnalyzing)
            {
                switch (id)
                {
                    case 199:
                        __result = "Null: Force/Light/Dark • Str: Nerve • Weak: Elec"; // Matador
                        break;
                    case 201:
                        __result = "Null: Light/Dark/Mind • Str: Curse • Weak: Fire"; // Daisoujou
                        break;
                    case 200:
                        __result = "Drn: Force • Null: Fire/Light/Dark • Str: Nerve • Weak: Ice"; // Hell Biker
                        break;
                    case 196:
                        __result = "Null: Fire/Light/Dark/Nerve • Weak: Elec"; // White Rider
                        break;
                    case 197:
                        __result = "Null: Elec/Force/Light/Dark/Mind • Weak: Ice"; // Red Rider
                        break;
                    case 198:
                        __result = "Drn: Ice • Null: Light/Dark/Curse • Weak: Force"; // Black Rider
                        break;
                    case 195:
                        __result = "Null: Ice/Light/Dark/Curse/Mind • Weak: Fire"; // Pale Rider
                        break;
                    case 202:
                        __result = "Rpl: Phys • Drn: Elec • Null: Light/Dark • Str: Ailments • Weak Force"; // Mother Harlot
                        break;
                }
            }

            s_isAnalyzing = false;
        }
    }

    [HarmonyPatch(typeof(datDevilFormat), nameof(datDevilFormat.Analyze))]
    private class Patch2
    {
        public static void Prefix()
        {
            s_isAnalyzing = true;
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Changes Matador's affinities
        datAisyo.tbl[199][3] = 2147483798; // Weak to Elec
        datAisyo.tbl[199][8] = 100; // Neutral to Curse
        datAisyo.tbl[199][9] = 50; // Resist Nerve
        datAisyo.tbl[199][10] = 100; // Neutral to Mind

        // Changes Daisoujou's affinities
        datAisyo.tbl[201][1] = 2147483798; // Weak to Fire
        datAisyo.tbl[201][6] = 65536; // Null Light
        datAisyo.tbl[201][7] = 65536; // Null Dark
        datAisyo.tbl[201][8] = 50; // Resist Curse
        datAisyo.tbl[201][9] = 100; // Neutral to Nerve
        datAisyo.tbl[201][10] = 65536; // Null Mind

        // Removes Prayer from Daisoujou's upcoming skills
        tblSkill.fclSkillTbl[201].Event[3] = tblSkill.fclSkillTbl[201].Event[4];
        tblSkill.fclSkillTbl[201].Event[4] = tblSkill.fclSkillTbl[201].Event[5];

        // Changes Hell Biker's affinities
        datAisyo.tbl[200][2] = 2147483798; // Weak to Ice
        datAisyo.tbl[200][8] = 100; // Neutral to Curse
        datAisyo.tbl[200][9] = 50; // Resist Nerve
        datAisyo.tbl[200][10] = 100; // Neutral to Mind

        // Changes White Rider's affinities
        datAisyo.tbl[196][3] = 2147483798; // Weak to Elec
        datAisyo.tbl[196][8] = 100; // Neutral to Curse
        datAisyo.tbl[196][10] = 100; // Neutral to Mind

        // Changes Red Rider's affinities
        datAisyo.tbl[197][2] = 2147483798; // Weak to Ice
        datAisyo.tbl[197][8] = 100; // Neutral to Curse
        datAisyo.tbl[197][9] = 100; // Neutral to Nerve

        // Changes Black Rider's affinities
        datAisyo.tbl[198][4] = 2147483798; // Weak to Force
        datAisyo.tbl[198][9] = 100; // Neutral to Nerve
        datAisyo.tbl[198][10] = 100; // Neutral to Mind

        // Changes Pale Rider's affinities
        datAisyo.tbl[195][1] = 2147483798; // Weak to Fire
        datAisyo.tbl[195][9] = 100; // Neutral to Nerve

        // Changes Mother Harlot's affinities
        datAisyo.tbl[202][4] = 2147483798; // Weak to Force
        datAisyo.tbl[202][8] = 50; // Resist Curse
        datAisyo.tbl[202][9] = 50; // Resist Nerve
        datAisyo.tbl[202][10] = 50; // Resist Mind

        // Changes Trumpeter's affinities
        datAisyo.tbl[203][1] = 80; // 20% Resist Fire
        datAisyo.tbl[203][2] = 80; // 20% Resist Ice
        datAisyo.tbl[203][3] = 80; // 20% Resist Elec
        datAisyo.tbl[203][4] = 80; // 20% Resist Force
    }
}

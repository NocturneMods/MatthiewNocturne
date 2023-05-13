using MelonLoader;
using Il2Cpp;
using ActuallyUniqueDanteRaidouSkills;
using HarmonyLib;

[assembly: MelonInfo(typeof(ActuallyUniqueDanteRaidouSkillsMod), "Actually unique Dante/Raidou skills", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace ActuallyUniqueDanteRaidouSkills;
public class ActuallyUniqueDanteRaidouSkillsMod : MelonMod
{
    private static bool isHolyStar; // is true when the last used skill was Holy Star / Raptor Guardian

    // After getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch
    {
        public static void Postfix(ref int id, ref string __result)
        {
            switch (id)
            {
                case 265: // Provoke
                    __result = "Greatly lower all foes' \nDefense. \nUser: Slight MP recovery.";
                    break;
                case 274: // Holy Star/Raptor Guardian
                    __result = "Negates -kaja & \n-nda effects on \nall foes & allies.";
                    break;
                default:
                    break;
            }
        }
    }

    // Before displaying the text box
    [HarmonyPatch(typeof(nbHelpProcess), nameof(nbHelpProcess.nbDispText))]
    private class Patch2
    {
        public static void Prefix(ref string text1, ref int type)
        {
            // If the text box is displaying the effect of Holy Star / Raptor Guardian
            if (type == 1 && isHolyStar)
            {
                type = 0;
                text1 = "Negated all -kaja & -nda effects!";
                isHolyStar = false;
            }
        }
    }

    // Before displying a skill name in the text box
    [HarmonyPatch(typeof(nbHelpProcess), nameof(nbHelpProcess.nbDispSkillName))]
    private class Patch3
    {
        public static void Prefix(ref int id)
        {
            isHolyStar = id == 274;
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Buffs Raptor Guardian
        datNormalSkill.tbl[274].hojotype = 263168; // Dekaja + Dekunda
        datNormalSkill.tbl[274].targettype = 2; // Targets foes and allies

        // Changes the camera view to show everyone on the battle field
        nbCamera_SkillPtrTable.tbl[274] = nbCamera_SkillPtrTable.tbl[152];

        // Buffs Provoke
        datNormalSkill.tbl[265].hojotype = 128; // Rakunda + Rakunda

        // Buffs Mishaguji Raiden
        datNormalSkill.tbl[267].badlevel = 30; // Doubles the chance of shock

        // Buffs Hitokoto Storm
        datNormalSkill.tbl[268].hpn = 75; // As strong as Zandyne (instead of Mazandyne)

        // Buffs Jiraiya Dance
        datNormalSkill.tbl[269].hpn = 95; // A bit stronger than Megidolaon
        datNormalSkill.tbl[269].cost = 50; // Same price as Megidolaon
    }
}

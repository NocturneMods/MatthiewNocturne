using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using FixedJavelinRainXerosBeat;
using Il2Cppnewdata_H;
using Il2Cppresult2_H;

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
            if (id == 133) id = 143;
            else if (id == 143) id = 133;
        }
    }

    // Before getting the description of a skill
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch2
    {
        public static void Prefix(ref int id)
        {
            // Swaps the skills descriptions
            if (id == 133) id = 143;
            else if (id == 143) id = 133;
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Swaps the "datNormalSkills" objects
        datNormalSkill_t tmpNormalSkill = datNormalSkill.tbl[133];
        datNormalSkill.tbl[133] = datNormalSkill.tbl[143];
        datNormalSkill.tbl[143] = tmpNormalSkill;
        
        // Swaps the "datSkills" objects
        datSkill_t tmpSkill = datSkill.tbl[133];
        datSkill.tbl[133] = datSkill.tbl[143];
        datSkill.tbl[143] = tmpSkill;

        // Swaps the Magatamas they belong to
        fclHeartsSkill_t tmpHeartsSkill = tblHearts.fclHeartsTbl[20].Skill[2];
        tblHearts.fclHeartsTbl[20].Skill[2] = tblHearts.fclHeartsTbl[15].Skill[2];
        tblHearts.fclHeartsTbl[15].Skill[2] = tmpHeartsSkill;
    }
}

// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using DisplayFutureSkills;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using Il2Cppresult2_H;
using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(DisplayFutureSkillsMod), "Display Future Skills (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace DisplayFutureSkills;
public class DisplayFutureSkillsMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "DisplayFutureSkills.cfg");
    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<bool> s_cfgTease = null!;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("DisplayFutureSkills");
        s_cfgTease = s_cfgCategoryMain.CreateEntry("TeaseDemifiendSkills", true, description: "Keeps some cool Demi-fiend skills as '?'.");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    [HarmonyPatch(typeof(cmpDrawStatus), nameof(cmpDrawStatus.cmpDrawSkill))]
    private class Patch
    {
        public static void Postfix(datUnitWork_t pStock, rstSkillInfo_t pSkillInfo)
        {
            for (int i = 0; i < pSkillInfo.SkillID.Length; i++)
            {
                ushort skillID = pSkillInfo.SkillID[i];
                if (skillID == 0)
                {
                    break;
                }

                if (s_cfgTease.Value && skillID == 357 && pStock.id == 0)
                {
                    // If you can get Pierce without TDE (mod) but aren't high level level enough
                    if (EventBit.evtBitCheck(2241) && tblHearts.fclHeartsTbl[1].Skill[5].TargetLevel > pStock.level + 1)
                    {
                        cmpStatus._statusUIScr.awaitText[i].text = "<material=\"TMC14\">？"; // Displays a "？"
                    }

                    continue; //Skip Pierce on Demi-fiend
                }

                string name = datSkillName.Get(skillID, pStock.id);
                cmpStatus._statusUIScr.awaitText[i].text = "<material=\"TMC14\">" + name;
            }
        }
    }
}

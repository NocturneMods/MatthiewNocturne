// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using TruePierce;

[assembly: MelonInfo(typeof(TruePierceMod), "True Pierce (ver. 0.6)", "2.0.0", "Matthiew Purple & Kraby")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace TruePierce;
public class TruePierceMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "TruePierce.cfg");
    private const bool DisplaySelfDestructInDesc = false;

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<bool> s_cfgAllowRepel = null!;
    private static MelonPreferences_Entry<bool> s_cfgPhysical = null!;
    private static MelonPreferences_Entry<bool> s_cfgFire = null!;
    private static MelonPreferences_Entry<bool> s_cfgIce = null!;
    private static MelonPreferences_Entry<bool> s_cfgElec = null!;
    private static MelonPreferences_Entry<bool> s_cfgForce = null!;
    private static MelonPreferences_Entry<bool> s_cfgAlmighty = null!;
    private static MelonPreferences_Entry<bool> s_cfgLight = null!;
    private static MelonPreferences_Entry<bool> s_cfgDark = null!;
    private static MelonPreferences_Entry<bool> s_cfgCurse = null!;
    private static MelonPreferences_Entry<bool> s_cfgNerve = null!;
    private static MelonPreferences_Entry<bool> s_cfgMind = null!;
    private static MelonPreferences_Entry<bool> s_cfgSelfDestruct = null!;

    private static bool s_hasPierce; // Remembers if the attacker has Pierce or Son's Oath/Raidou the Eternal

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("TruePierce");
        s_cfgAllowRepel = s_cfgCategoryMain.CreateEntry("AllowRepel", false, "Allow true pierce with repels", description: "Allow True Pierce with repels.");
        s_cfgPhysical = s_cfgCategoryMain.CreateEntry("Physical", false, "Physical true pierce", description: "Allow True Pierce with Physical attacks.");
        s_cfgFire = s_cfgCategoryMain.CreateEntry("Fire", false, "Fire true pierce", description: "Allow True Pierce with Fire attacks.");
        s_cfgIce = s_cfgCategoryMain.CreateEntry("Ice", false, "Ice true pierce", description: "Allow True Pierce with Ice attacks.");
        s_cfgElec = s_cfgCategoryMain.CreateEntry("Elec", false, "Elec true pierce", description: "Allow True Pierce with Elec attacks.");
        s_cfgForce = s_cfgCategoryMain.CreateEntry("Force", false, "Force true pierce", description: "Allow True Pierce with Force attacks.");
        s_cfgAlmighty = s_cfgCategoryMain.CreateEntry("Almighty", false, "Almighty true pierce", description: "Allow True Pierce with Almighty attacks.");
        s_cfgLight = s_cfgCategoryMain.CreateEntry("Light", false, "Light true pierce", description: "Allow True Pierce with Light attacks.");
        s_cfgDark = s_cfgCategoryMain.CreateEntry("Dark", false, "Dark true pierce", description: "Allow True Pierce with Dark attacks.");
        s_cfgCurse = s_cfgCategoryMain.CreateEntry("Curse", false, "Curse true pierce", description: "Allow True Pierce with Curse attacks.");
        s_cfgNerve = s_cfgCategoryMain.CreateEntry("Nerve", false, "Nerve true pierce", description: "Allow True Pierce with Nerve attacks.");
        s_cfgMind = s_cfgCategoryMain.CreateEntry("Mind", false, "Mind true pierce", description: "Allow True Pierce with Mind attacks.");
        s_cfgSelfDestruct = s_cfgCategoryMain.CreateEntry("SelfDestruct", false, "SelfDestruct true pierce", description: "Allow True Pierce with SelfDestruct attacks.");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    private static bool IsTruePierceEnabled(int type)
    {
        return type switch
        {
            0 => s_cfgPhysical.Value,
            1 => s_cfgFire.Value,
            2 => s_cfgIce.Value,
            3 => s_cfgElec.Value,
            4 => s_cfgForce.Value,
            5 => s_cfgAlmighty.Value,
            6 => s_cfgLight.Value,
            7 => s_cfgDark.Value,
            8 => s_cfgCurse.Value,
            9 => s_cfgNerve.Value,
            10 => s_cfgMind.Value,
            11 => s_cfgSelfDestruct.Value,
            _ => false,
        };
    }

    private static string GetTruePierceElementsDescription()
    {
        var truePieceElements = Enumerable.Range(0, 12).Where(i => IsTruePierceEnabled(i));
        var qualifiers = new List<string>();

        // Special case
        if (truePieceElements.SequenceEqual(Enumerable.Range(0, 12)))
        {
            return "All attacks";
        }

        // Physical
        if (truePieceElements.Contains(0))
        {
            qualifiers.Add("Physical");
        }

        // Magic
        int magicalTypesTruePierced = new int[] { 1, 2, 3, 4, 5 }.Except(truePieceElements).Count();
        if (magicalTypesTruePierced == 5)
        {
            // All magic types
            qualifiers.Add("Magic");
        }
        else if (magicalTypesTruePierced > 0)
        {
            var magic = new List<string>();
            if (truePieceElements.Contains(1))
            {
                magic.Add("Fir");
            }

            if (truePieceElements.Contains(2))
            {
                magic.Add("Ice");
            }

            if (truePieceElements.Contains(3))
            {
                magic.Add("Elec");
            }

            if (truePieceElements.Contains(4))
            {
                magic.Add("For");
            }

            if (truePieceElements.Contains(5))
            {
                magic.Add("Alm");
            }

            qualifiers.Add(string.Join("/", magic));
        }

        // Light / Dark
        if (truePieceElements.Contains(6) && truePieceElements.Contains(7))
        {
            qualifiers.Add("Light/Dark");
        }
        else if (truePieceElements.Contains(6))
        {
            qualifiers.Add("Light");
        }
        else if (truePieceElements.Contains(7))
        {
            qualifiers.Add("Dark");
        }

        // Ailments
        int ailmentsTypesTruePierced = new int[] { 8, 9, 10 }.Except(truePieceElements).Count();
        if (ailmentsTypesTruePierced == 3)
        {
            // All ailments types
            qualifiers.Add("Ailment");
        }
        else if (ailmentsTypesTruePierced > 0)
        {
            var ailments = new List<string>();
            if (truePieceElements.Contains(8))
            {
                ailments.Add("Curse");
            }

            if (truePieceElements.Contains(9))
            {
                ailments.Add("Nerve");
            }

            if (truePieceElements.Contains(10))
            {
                ailments.Add("Mind");
            }

            qualifiers.Add(string.Join("/", ailments));
        }

        // Self destruct
        if (DisplaySelfDestructInDesc && truePieceElements.Contains(11))
        {
            qualifiers.Add("Self-Destruct");
        }

        string desc = "";
        for (int i = 0; i < qualifiers.Count; ++i)
        {
            desc += qualifiers[i];
            if (i != qualifiers.Count - 1)
            {
                if (i % 2 == 1)
                {
                    desc += " \n";
                }

                if (i == qualifiers.Count - 2)
                {
                    desc += " and ";
                }
                else if (i % 2 != 1)    // Not a newline
                {
                    desc += ", ";
                }
            }
        }

        return desc;
    }

    // Before getting the effectiveness of a skill
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetKoukaType))]
    private class Patch
    {
        public static void Prefix(ref int sformindex, ref int nskill)
        {
            // If the skill in question is NOT a self-switch (from Zephhyr's mod) nor Analyze
            if (nbMainProcess.nbGetUnitWorkFromFormindex(sformindex) != null)
            {
                // 357 = Pierce and 361 = Son's Oath/Raidou the Eternal
                s_hasPierce = nskill != 71 && (nbMainProcess.nbGetUnitWorkFromFormindex(sformindex).skill.Contains(357) || nbMainProcess.nbGetUnitWorkFromFormindex(sformindex).skill.Contains(361));
            }
        }
    }

    // After getting the effectiveness of an attack on 1 target
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetAisyo))]
    private class Patch2
    {
        public static void Postfix(ref uint __result, ref int attr, ref int formindex, ref int nskill)
        {
            // If the attack has Pierce (or equivalent) and the attack is physical and it's resisted/blocked/drained/repelled
            bool isTypeTruePierce = IsTruePierceEnabled(attr);
            bool isRepel = nskill == -1;
            if (s_hasPierce && isTypeTruePierce && (!isRepel || s_cfgAllowRepel.Value) && (__result < 100 || (__result >= 65536 && __result < 2147483648)))
            {
                __result = 100; // Forces the affinity to become "neutral"
                nbMainProcess.nbGetMainProcessData().d31_kantuu = 1; // Displays the "Pierced!" message
            }
        }
    }

    // After getting a skill description
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch3
    {
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 357)
            {
                __result = $"{GetTruePierceElementsDescription()} \nignore all resistances."; // New skill description for Pierce
            }
        }
    }
}

// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using AntiBecomesResist;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using MelonLoader;

[assembly: MelonInfo(typeof(AntiBecomesResistMod), "Anti becomes Resist (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace AntiBecomesResist;
public class AntiBecomesResistMod : MelonMod
{
    private static readonly List<Tuple<datUnitWork_t, int, int>> s_demonsWithResist = new();

    // After checking for the element of an attack
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbGetAisyo))]
    private class Patch
    {
        public static void Postfix(ref int formindex, ref int attr, ref uint __result)
        {
            datUnitWork_t targetUnit = nbMainProcess.nbGetUnitWorkFromFormindex(formindex);

            // If the target with "Resist X" has already been added to the list
            foreach (Tuple<datUnitWork_t, int, int> demonInfo in s_demonsWithResist)
            {
                if (demonInfo.Item1.id == targetUnit.id)
                {
                    __result = 50; // Resist
                    return;
                }
            }

            //If not neutral, pseudo-weak nor weak, act as normal
            if ((__result < 100 || __result > 999) && __result < 1000000000)
            {
                return;
            }

            int antiSkillID;

            switch (attr)
            {
                //Anti-Phys
                case 0:
                    antiSkillID = 313;
                    break;
                //Anti-Fire
                case 1:
                    antiSkillID = 314;
                    break;
                //Anti-Ice
                case 2:
                    antiSkillID = 315;
                    break;
                //Anti-Elec
                case 3:
                    antiSkillID = 316;
                    break;
                //Anti-Force
                case 4:
                    antiSkillID = 317;
                    break;
                //Anti-Light
                case 6:
                    antiSkillID = 318;
                    break;
                //Anti-Dark
                case 7:
                    antiSkillID = 319;
                    break;
                //Anti-Curse
                case 8:
                    antiSkillID = 320;
                    break;
                //Anti-Nerve
                case 9:
                    antiSkillID = 321;
                    break;
                //Anti-Mind
                case 10:
                    antiSkillID = 322;
                    break;
                default:
                    return;
            }

            //If the target has the corresponding Anti-X skill
            for (int i = 0; i < targetUnit.skill.Count; i++)
            {
                if (targetUnit.skill[i] == antiSkillID)
                {
                    __result = 50; // Resist

                    s_demonsWithResist.Add(new Tuple<datUnitWork_t, int, int>(targetUnit, i, antiSkillID)); // Adds the demon to the list
                    targetUnit.skill[i] = 0; // Removes the skill temporarily
                    return;
                }
            }
        }
    }

    // After getting the name of a skill
    [HarmonyPatch(typeof(datSkillName), nameof(datSkillName.Get))]
    private class Patch2
    {
        public static void Postfix(ref int id, ref string __result)
        {
            switch (id)
            {
                //Phys
                case 313:
                    __result = "Resist Phys";
                    return;
                //Fire
                case 314:
                    __result = "Resist Fire";
                    return;
                //Ice
                case 315:
                    __result = "Resist Ice";
                    return;
                //Elec
                case 316:
                    __result = "Resist Elec";
                    return;
                //Force
                case 317:
                    __result = "Resist Force";
                    return;
                //Light
                case 318:
                    __result = "Resist Light";
                    return;
                //Dark
                case 319:
                    __result = "Resist Dark";
                    return;
                //Curse
                case 320:
                    __result = "Resist Curse";
                    return;
                //Nerve
                case 321:
                    __result = "Resist Nerve";
                    return;
                //Mind
                case 322:
                    __result = "Resist Mind";
                    return;
            }
        }
    }

    // After a "turn"
    [HarmonyPatch(typeof(nbUnitProcess), nameof(nbUnitProcess.nbUnitProcess2))]
    private class Patch3
    {
        public static void Postfix()
        {
            // For each demon with "Resist X"
            foreach (Tuple<datUnitWork_t, int, int> demonInfo in s_demonsWithResist)
            {
                demonInfo.Item1.skill[demonInfo.Item2] = demonInfo.Item3; // Puts the removed skill back
            }

            // Clears the list
            s_demonsWithResist.Clear();
        }
    }
}

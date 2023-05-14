// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using MelonLoader;
using MelonLoader.Utils;
using OneMoreEnemyPressTurn;

[assembly: MelonInfo(typeof(OneMoreEnemyPressTurnMod), "One more enemy Press Turn (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace OneMoreEnemyPressTurn;
public class OneMoreEnemyPressTurnMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "OneMoreEnemyPressTurn.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<int> s_cfgMorePressTurnRegularEnemies = null!;
    private static MelonPreferences_Entry<int> s_cfgMorePressTurnBosses = null!;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("OneMoreEnemyPressTurn");
        s_cfgMorePressTurnRegularEnemies = s_cfgCategoryMain.CreateEntry<int>("MorePressTurnRegularEnemies", 0, "Additional press turn count (regular enemies)", description: "");
        s_cfgMorePressTurnBosses = s_cfgCategoryMain.CreateEntry<int>("MorePressTurnBosses", 0, "Additional press turn count (bosses)", description: "In hard difficulty, items will cost their vanilla price multiplied by this value. (default: x3)");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    // After initiating a phase
    [HarmonyPatch(typeof(nbMainProcess), nameof(nbMainProcess.nbSetPressMaePhase))]
    private class Patch
    {
        public static void Postfix()
        {
            short activeunit = nbMainProcess.nbGetMainProcessData().activeunit; // Get the formindex of the first active demon
            bool hasBoss = Utility.HasABossOnTheirSide();

            // If that demon is an enemy and the enemies don't already have 8 press turns
            if (activeunit >= 4 && nbMainProcess.nbGetMainProcessData().press4_p < 8)
            {
                if (hasBoss)
                {
                    nbMainProcess.nbGetMainProcessData().press4_p += (short)s_cfgMorePressTurnBosses.Value; // Add 1 full press turn
                    nbMainProcess.nbGetMainProcessData().press4_ten += (short)s_cfgMorePressTurnBosses.Value; // Add 1 total press turn
                }
                else if (!hasBoss)
                {
                    nbMainProcess.nbGetMainProcessData().press4_p += (short)s_cfgMorePressTurnRegularEnemies.Value; // Add 1 full press turn
                    nbMainProcess.nbGetMainProcessData().press4_ten += (short)s_cfgMorePressTurnRegularEnemies.Value; // Add 1 total press turn
                }
            }
        }
    }
    
    // List of enemies with an ID greater than 255 but that aren't bosses
    private static readonly List<ushort> s_fakeBosses = new ()
    {
        318, // Will o' Wisp (Tutorial)
        319, // Preta (Tutorial)
        260, // Incubus (Nihilo)
        261, // Koppa Tengu (Nihilo)
        359, // Virtue (White Rider)
        360, // Power (Red Rider)
        361, // Legion (Black Rider)
        358, // Loa (Pale Rider)
        279, // Urthona
        280, // Urizen
        281, // Luvah
        282, // Tharmus
        290, // Flauros Hallel
        289  // Ose Hallel
    };

    private class Utility
    {
        // Checks if there is a boss on the enemy's side
        public static bool HasABossOnTheirSide()
        {
            foreach (datUnitWork_t item in nbMainProcess.nbGetMainProcessData().enemyunit)
            {
                // If it's an actual boss (and not a mini-boss) who's still alive
                if (item.id >= 256 && item.id < 362 && !s_fakeBosses.Contains(item.id) && item.hp != 0)
                {
                    return true;
                }

                // Special case for the Succubus chest boss
                else if (item.id == 117 && nbMainProcess.nbGetMainProcessData().encno == 990)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

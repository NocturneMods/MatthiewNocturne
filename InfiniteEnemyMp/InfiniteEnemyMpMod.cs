using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using InfiniteEnemyMp;

[assembly: MelonInfo(typeof(InfiniteEnemyMpMod), "Infinite Enemy MP", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace InfiniteEnemyMp;
public class InfiniteEnemyMpMod : MelonMod
{
    // List of demons that should be able to run out of MP
    static public List<ushort> bossesWithMana = new List<ushort>()
    {
        273, // Specter 2
        299, // Sakahagi
        355  // Sakahagi
    };

    // Before removing HP or MP from someone during combat
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbAddHpMp))]
    private class Patch
    {
        public static void Prefix(ref int formindex, ref int type, ref int n)
        {
            // Gets id of the unit
            ushort id = nbMainProcess.nbGetUnitWorkFromFormindex(formindex).id;

            // If an enemy (that shouldn't be able to run out of MP) is about to lose MP
            if (formindex >= 4 && !bossesWithMana.Contains(id) && type == 1 && n < 0) n = 0; // Makes it lose 0 MP
        }
    }
}

using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using early_compendium;
using Il2Cppnewdata_H;

[assembly: MelonInfo(typeof(EarlyCompendiumMod), "Early Compendium", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace early_compendium;
public class EarlyCompendiumMod : MelonMod
{
    // After checking for the state of a flag
    [HarmonyPatch(typeof(EventBit), nameof(EventBit.evtBitCheck))]
    private class Patch
    {
        public static void Postfix(ref int no, ref bool __result)
        {
            if (no == 40) __result = true; // Compendium's flag always returns true
        }
    }
}

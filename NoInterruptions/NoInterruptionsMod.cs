using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using NoInterruptions;
using Il2Cppnewdata_H;
using Il2Cppnewbattle_H;
using Il2Cppkernel_H;
using System.Xml.Linq;

[assembly: MelonInfo(typeof(NoInterruptionsMod), "No interruptions", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace NoInterruptions;
public class NoInterruptionsMod : MelonMod
{
    public static short tmp_enemypcnt = 0; // Remembers the number of enemies before modification

    // After initiating a negotiation sequence
    [HarmonyPatch(typeof(nbNegoProcess), nameof(nbNegoProcess.InitNegoProcessData))]
    private class Patch2
    {
        public static void Postfix(ref nbActionProcessData_t actdata)
        {
            // Changes the numbers of enemies to 1 so the game never triggers an interruption
            tmp_enemypcnt = actdata.data.enemypcnt;
            actdata.data.enemypcnt = 1;
        }
    }

    // After displaying a negotiation message
    [HarmonyPatch(typeof(nbNegoProcess), nameof(nbNegoProcess.nbDispNegoMessage))]
    private class Patch5
    {
        public static void Prefix()
        {
            // If the number of enemies has been modified
            if (tmp_enemypcnt != 0)
            {
                // Reverts it to its previous value
                nbMainProcess.nbGetMainProcessData().enemypcnt = tmp_enemypcnt;
                tmp_enemypcnt = 0;
            }
        }
    }
}

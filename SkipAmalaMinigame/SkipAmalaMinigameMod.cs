using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using SkipAmalaMinigame;

[assembly: MelonInfo(typeof(SkipAmalaMinigameMod), "Skip Amala minigame (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace SkipAmalaMinigame;
public class SkipAmalaMinigameMod : MelonMod
{
    [HarmonyPatch(typeof(pipeSection), nameof(pipeSection.pipeLoadSection))]
    private class Patch
    {
        public static void Postfix()
        {
            dds3SequenceList.ExitPipe(); // Immediately exit the "pipe" after loading it
        }
    }
}

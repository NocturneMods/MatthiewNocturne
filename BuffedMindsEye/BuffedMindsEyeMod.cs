// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using BuffedMindsEye;
using HarmonyLib;
using Il2Cpp;
using MelonLoader;

[assembly: MelonInfo(typeof(BuffedMindsEyeMod), "Buffed Mind's Eye (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace BuffedMindsEye;
public class BuffedMindsEyeMod : MelonMod
{
    // After checking for a back attack
    [HarmonyPatch(typeof(nbCalc), nameof(nbCalc.nbCheckBackAttack))]
    private class Patch
    {
        public static void Postfix(ref int __result)
        {
            // If someone has Mind's Eye, then always avoid back attacks
            if (datCalc.datCheckSkillInParty(298) == 1)
            {
                __result = 0;
            }
        }
    }

    // When getting a skill's description
    [HarmonyPatch(typeof(datSkillHelp_msg), nameof(datSkillHelp_msg.Get))]
    private class Patch2
    {
        public static void Postfix(ref int id, ref string __result)
        {
            // New skill description for Mind's Eye
            if (id == 298)
            {
                __result = "Prevents being attacked \nfrom behind.";
            }
        }
    }
}

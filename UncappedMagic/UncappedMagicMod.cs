// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Il2Cpp;
using MelonLoader;
using UncappedMagic;

[assembly: MelonInfo(typeof(UncappedMagicMod), "Uncapped magic (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace UncappedMagic;
public class UncappedMagicMod : MelonMod
{
    // When booting up the game
    public override void OnInitializeMelon()
    {
        // For each skill in the game
        for (int i = 0; i < datNormalSkill.tbl.Length; i++)
        {
            // If the skill is a magic skill, then uncap its limit
            if (datNormalSkill.tbl[i].magiclimit != 0)
            {
                datNormalSkill.tbl[i].magiclimit = short.MaxValue;
            }
        }
    }
}

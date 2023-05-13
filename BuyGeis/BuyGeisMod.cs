// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using BuyGeis;
using HarmonyLib;
using Il2Cpp;
using Il2Cppfacility_H;
using MelonLoader;

[assembly: MelonInfo(typeof(BuyGeisMod), "Buy Geis (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace BuyGeis;
public class BuyGeisMod : MelonMod
{
    [HarmonyPatch(typeof(fclShopCalc), nameof(fclShopCalc.shpCreateItemList))]
    private class Patch
    {
        // Adds Geis to shop number 4 (Asakusa) if you don't already have it
        public static void Postfix(ref fclDataShop_t pData)
        {
            if (pData.Place == 4 && !dds3GlobalWork.DDS3_GBWK.hearts.Contains(13))
            {
                pData.BuyItemList[pData.BuyItemCnt] = 76; // Adds Geis to the shop list
                pData.BuyItemCnt++; // Adds a slot to the shop list
                datItemHelp_msg.txt[76] = datItemHelp_msg.txt[66]; // Gives Geis the same description as Ankh ("Healing-type Magatama")
            }
        }
    }

    [HarmonyPatch(typeof(datItemHelp_msg), nameof(datItemHelp_msg.Get))]
    private class Patch2
    {
        // Adds a description for Geis in the shop list
        public static void Postfix(ref int id, ref string __result)
        {
            if (id == 76)
            {
                __result = datItemHelp_msg.Get(66); // Gives Geis the same description as Ankh ("Healing-type Magatama")
            }
        }
    }
}

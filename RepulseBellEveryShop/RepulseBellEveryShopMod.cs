// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using Il2Cppfacility_H;
using MelonLoader;
using RepulseBellEveryShop;

[assembly: MelonInfo(typeof(RepulseBellInEveryShopMod), "Repulse Bell/Attract Pipe in every shop (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace RepulseBellEveryShop;
public class RepulseBellInEveryShopMod : MelonMod
{
    // After creating the shop
    [HarmonyPatch(typeof(fclShopCalc), nameof(fclShopCalc.shpCreateItemList))]
    private class Patch
    {
        public static void Postfix(ref fclDataShop_t pData)
        {
            pData.BuyItemList[pData.BuyItemCnt++] = 52; // Repulse Bell
            pData.BuyItemList[pData.BuyItemCnt++] = 53; // Attract Pipe
        }
    }
}

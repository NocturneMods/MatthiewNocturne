using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using RepulseBellEveryShop;
using Il2Cppfacility_H;

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

// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Utils;
using NormalPricesOnHard;

[assembly: MelonInfo(typeof(NormalPricesOnHardMod), "Normal prices on Hard (ver. 0.6)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace NormalPricesOnHard;
public class NormalPricesOnHardMod : MelonMod
{
    public static readonly string ConfigPath = Path.Combine(MelonEnvironment.UserDataDirectory, "ModsCfg", "NormalPricesOnHard.cfg");

    private static MelonPreferences_Category s_cfgCategoryMain = null!;
    private static MelonPreferences_Entry<float> s_cfgPriceMultiplierNormal = null!;
    private static MelonPreferences_Entry<float> s_cfgPriceMultiplierHard = null!;

    public override void OnInitializeMelon()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);

        s_cfgCategoryMain = MelonPreferences.CreateCategory("NormalPricesOnHard");
        s_cfgPriceMultiplierNormal = s_cfgCategoryMain.CreateEntry<float>("PriceMultiplierNormal", 1.0f, "Price multiplier", description: "Items will cost their vanilla price multiplied by this value. (default: x1)");
        s_cfgPriceMultiplierHard = s_cfgCategoryMain.CreateEntry<float>("PriceMultiplierHard", 3.0f, "Price multiplier (Hard difficulty)", description: "In hard difficulty, items will cost their vanilla price multiplied by this value. (default: x3)");

        s_cfgCategoryMain.SetFilePath(ConfigPath);
        s_cfgCategoryMain.SaveToFile();
    }

    [HarmonyPatch(typeof(fclShopCalc), nameof(fclShopCalc.shpCalcItemPrice))]
    private class Patch
    {
        public static void Postfix(ref sbyte Mode, ref int __result)
        {
            // If buying
            if (Mode == 0)
            {
                // then apply the multiplier,
                // on Hard items costs three times as much, divide the price by three
                if (dds3ConfigMain.cfgGetBit(9u) == 2)
                {
                    __result = (int)Math.Round(__result / 3 * s_cfgPriceMultiplierHard.Value);
                }
                // on normal / merciful (prices x1)
                else
                {
                    __result = (int)Math.Round(__result * s_cfgPriceMultiplierNormal.Value);
                }
            }
        }
    }
}

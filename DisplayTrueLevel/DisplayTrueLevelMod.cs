// Copyright (c) MatthiewPurple.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using DisplayTrueLevel;
using HarmonyLib;
using Il2Cpp;
using Il2Cppnewdata_H;
using Il2CppTMPro;
using MelonLoader;

[assembly: MelonInfo(typeof(DisplayTrueLevelMod), "Display true level (0.6 ver.)", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace DisplayTrueLevel;
public class DisplayTrueLevelMod : MelonMod
{
    // After capping the level for display
    [HarmonyPatch(typeof(datCalc), nameof(datCalc.datGetLevelForDraw))]
    private class Patch
    {
        public static void Postfix(ref datUnitWork_t work, ref int __result)
        {
            __result = work.level; // Displays the actual level (on two digits)

            // Removes the first digit if it's 0 for aesthetic purposes
            while (__result > 99)
            {
                __result -= 100;
            }
        }
    }

    // After drawing the status panel
    [HarmonyPatch(typeof(cmpDrawStatus), nameof(cmpDrawStatus.cmpDrawStatusPanel))]
    private class Patch2
    {
        public static void Postfix(ref datUnitWork_t pStock)
        {
            // Changes the color every 100 levels and updates the displayed object
            if (pStock.level > 99 && pStock.level != 255)
            {
                cmpStatus._statusUIScr.transform.Find("sinfo_basic/slvnum").gameObject.GetComponent<CounterCtr>().colorIndex = pStock.level / 100 + 1;
                cmpStatus._statusUIScr.transform.Find("sinfo_basic/slvnum").gameObject.GetComponent<CounterCtr>().Change();
            }
            // Special case for level 255 which is the maximum level
            else if (pStock.level == 255)
            {
                cmpStatus._statusUIScr.transform.Find("sinfo_basic/slvnum").gameObject.GetComponent<CounterCtr>().colorIndex = 6; // Displays 55 in black instead of red
                cmpStatus._statusUIScr.transform.Find("sinfo_basic/slvnum").gameObject.GetComponent<CounterCtr>().Change();
            }
        }
    }

    // After drawing the command menu
    [HarmonyPatch(typeof(cmpDrawStock), nameof(cmpDrawStock.cmpDrawStockInfo))]
    private class Patch3
    {
        public static void Postfix()
        {
            // For the party
            for (int i = 1; i <= 4; i++)
            {
                string name = cmpInit._campUIScr.transform.Find($"party/party_status/party_status{Utility.GetNumberForPath(i)}/Text_nameTM").gameObject.GetComponent<TextMeshProUGUI>().text;
                name = Utility.RemoveMaterial(name);
                ushort level = Utility.GetLevelFromName(name);
                if (level > 99)
                {
                    cmpInit._campUIScr.transform.Find($"party/party_status/party_status{Utility.GetNumberForPath(i)}/num_lv").gameObject.GetComponent<CounterCtr>().colorIndex = level / 100 + 1;
                    cmpInit._campUIScr.transform.Find($"party/party_status/party_status{Utility.GetNumberForPath(i)}/num_lv").gameObject.GetComponent<CounterCtr>().Change();
                }
            }

            // For the rest of the stock
            for (int i = 1; i <= 12; i++)
            {
                string name = cmpInit._campUIScr.transform.Find($"party/party_status/stock_status{Utility.GetNumberForPath(i)}/Text_nameTM").gameObject.GetComponent<TextMeshProUGUI>().text;
                name = Utility.RemoveMaterial(name);
                ushort level = Utility.GetLevelFromName(name);
                if (level > 99)
                {
                    cmpInit._campUIScr.transform.Find($"party/party_status/stock_status{Utility.GetNumberForPath(i)}/num_lv").gameObject.GetComponent<CounterCtr>().colorIndex = level / 100 + 1;
                    cmpInit._campUIScr.transform.Find($"party/party_status/stock_status{Utility.GetNumberForPath(i)}/num_lv").gameObject.GetComponent<CounterCtr>().Change();
                }
            }
        }
    }

    private class Utility
    {
        // Returns the string for the corresponding path of the object that displays the level of the demon in the command menu
        public static string GetNumberForPath(int i)
        {
            if (i < 10)
            {
                return "0" + i;
            }
            else
            {
                return i.ToString();
            }
        }

        // Removes the beginning of the name (the material)
        public static string RemoveMaterial(string text)
        {
            return text[(text.IndexOf(">") + 1)..];
        }

        // Returns level from name
        public static ushort GetLevelFromName(string name)
        {
            // For each demon in the stock
            foreach (datUnitWork_t work in dds3GlobalWork.DDS3_GBWK.unitwork)
            {
                if (datDevilName.Get(work.id) == name)
                {
                    return work.level; // Compares names
                }
            }

            // Special case for Demi-fiend's nickname
            if (frName.frGetCNameString(0) == name)
            {
                return dds3GlobalWork.DDS3_GBWK.unitwork[0].level;
            }

            return 0; // Not gonna happen
        }
    }
}

**UNSTABLE DEV BRANCH** : adds support for configurations in various mods !

Objective : make it easy to check Matthiew's Nocture Remaster mods, compile them easily, and add configuration support.

# How to compile
Please put these dlls in a `_dlls` folder at the root of the solution :
- `smt3hd\MelonLoader\net6\MelonLoader.dll`
- `smt3hd\MelonLoader\net6\Il2CppInterop.Runtime.dll`
- `smt3hd\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll`
- `smt3hd\MelonLoader\Il2CppAssemblies\Il2Cppmscorlib.dll`
- `smt3hd\MelonLoader\Il2CppAssemblies\UnityEngine.CoreModule.dll`
- `smt3hd\MelonLoader\Il2CppAssemblies\Unity.TextMeshPro.dll`
- `smt3hd\MelonLoader\Dependencies\Il2CppAssemblyGenerator\0Harmony.dll`
- `smt3hd\MelonLoader\Il2CppAssemblies\Unity.TextMeshPro.dll`

# Mods

## Bonus / Non-canonical mods

List of standalone mods that won't be part of the modpack (fate to be determined):
- PiercingMagmaAxis
- UncappedMagic
- DanteOnNewGamePlus 
- OneMoreAllyPressTurn 

## Modpack content

Everything else.

# TODO

## Unsupported variants

In bold, the only currently supported variant:
- BetterNewGamePlus (**magatama+items+demons+macca** / demons / items / macca / magatama / nothing / modern)
- PiercingMagmaAxis (**including repel** / no repel)
- FocusMagic (**standalone** / fixed focus compatible)
- PierceWithoutTde (**80** / 90)
- TruePierce (**phys including repel** / everything / phys+magic / phys+magic no repel)
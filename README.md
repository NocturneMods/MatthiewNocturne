**DEV BRANCH** : adds support for configurations in various mods

Please report any **bug** you have with the mods by **opening an issue** !
You can also submit feature request to add configurations in existing mods.

# Mods

## Modpack content

Some mods can be further configured (these), these will have Nocturne vanilla behaviour by default (=~ disabled). To enable them, check out their available presets.


| Mod | Configurable | Config presets |
|---|---|---|
| [ActuallyUnique](https://gamebanana.com/mods/443171) |  | |
| [AnalyzeBosses](https://gamebanana.com/mods/417551) |  | |
| [AntiBecomesResist](https://gamebanana.com/mods/407509) |  | |
| [BetterNewGamePlus](https://gamebanana.com/mods/433294) | :heavy_check_mark: | [presets](_cfg_presets/BetterNewGamePlus/) |
| [BuffedMindsEye](https://gamebanana.com/mods/407332) |  | |
| [BuffedRecarm](https://gamebanana.com/mods/407640) |  | |
| [BuffedRepulseBell](https://gamebanana.com/mods/380387) |  | |
| [BuyGeis](https://gamebanana.com/mods/381668) |  | |
| [CursedGospel](https://gamebanana.com/mods/443167) |  | |
| [DisplayBuffs](https://gamebanana.com/mods/436369) |  | |
| [DisplayFutureSkills](https://gamebanana.com/mods/374425) | :heavy_check_mark: | [presets](_cfg_presets/DisplayFutureSkills/) |
| [DisplayTrueLevel](https://gamebanana.com/mods/417721) |  | |
| [EarlyCompendium](https://gamebanana.com/mods/441592) |  | |
| [EscapeOnHard](https://gamebanana.com/mods/379292) | :heavy_check_mark: | [presets](_cfg_presets/EscapeOnHard/) |
| [EveryoneGetsExp](https://gamebanana.com/mods/378534) | :heavy_check_mark: | [presets](_cfg_presets/EveryoneGetsExp/) |
| [FiendsNerf](https://gamebanana.com/mods/442339) |  | |
| [FixedJavelinRainXerosBeat](https://gamebanana.com/mods/441774) | TODO (fix & buff) | |
| [FocusMagic](https://gamebanana.com/mods/416965) |  | |
| [HealEveryone](https://gamebanana.com/mods/406830) |  | |
| [HourglassItem](https://gamebanana.com/mods/441740) |  | |
| [InfiniteEnemyMp](https://gamebanana.com/mods/442187) |  | |
| [InfiniteMagatamaSkills](https://gamebanana.com/mods/439135) |  | |
| [ModernPressTurns](https://gamebanana.com/mods/376130) | :heavy_check_mark: | [presets](_cfg_presets/ModernPressTurn/) |
| [NoInterruptions](https://gamebanana.com/mods/439208) |  | |
| [NormalInstakillResistances](https://gamebanana.com/mods/443155) |  | |
| [NormalPricesOnHard](https://gamebanana.com/mods/379115) | :heavy_check_mark: | [presets](_cfg_presets/NormalPricesOnHard/) |
| [OneMoreEnemyPressTurn](https://gamebanana.com/mods/412716) | :heavy_check_mark: | [presets](_cfg_presets/OneMoreEnemyPressTurn/) |
| [PierceWithoutTde](https://gamebanana.com/mods/412714) | :heavy_check_mark: | [presets](_cfg_presets/PierceWithoutTde/) |
| [RepulseBellEveryShop](https://gamebanana.com/mods/382242) |  | |
| [SkipAmalaMinigame](https://gamebanana.com/mods/415578) |  | |
| [TruePierce](https://gamebanana.com/mods/411510) | :heavy_check_mark: | [presets](_cfg_presets/TruePierce/) |

### How to configure mods

After starting the game once with mods loaded, open the SMT3 steam folder, and head to `smt3hd\UserData\ModsCfg\`, there will be one `.cfg` configuration file for each configurable mod.
You can open them with any text editor, and edit the values as you see fit, each entry you can edit is preceded by a comment line describing it (starting with a `#`). You can find configuration presets for each mod in the [_cfg_presets](_cfg_presets) folder (in the `Presets` folder in releases).

## Bonus / Non-canonical mods

List of standalone mods that won't be part of the modpack (fate to be determined):
- [PiercingMagmaAxis](https://gamebanana.com/mods/380326)
- [UncappedMagic](https://gamebanana.com/mods/416903)
- [DanteOnNewGamePlus](https://gamebanana.com/mods/439012)
- [OneMoreAllyPressTurn](https://gamebanana.com/mods/439139)


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


# TODO

## Unsupported variants

In bold, the only currently supported variant:
- PiercingMagmaAxis (**including repel** / no repel)
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace CoolerEpicLootChests
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency("randyknapp.mods.epicloot", BepInDependency.DependencyFlags.HardDependency)]
    public class CoolerEpicLootChests : BaseUnityPlugin
    {
        private const string ModName = "CoolerEpicLootChests";
        private const string ModVersion = "0.0.1";
        private const string ModGUID = "com.zarboz.CoolerEpicLootChests";
        public static GameObject testfx2 { get; set; }
        public static GameObject testfx1 { get; set; }

        public static ConfigEntry<IconThing.IconType> TypeOfIcon;

        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(ModGUID);
            harmony.PatchAll(assembly);

            var assetbundle = Utilities.LoadAssetBundle("coolerloot");
            testfx1 = assetbundle.LoadAsset<GameObject>("IconGameObject");
            testfx2 = assetbundle.LoadAsset<GameObject>("LootFX");

            TypeOfIcon = Config.Bind("Cooler Epic Loot", "Value For Icon",IconThing.IconType.Exclaim,
                new ConfigDescription(" This is the Default kind of icon"));
        }

        
    }
}
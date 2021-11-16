using System;
using EpicLoot.Adventure;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoolerEpicLootChests
{
    public class Patches
    {
        [HarmonyPatch(typeof(TreasureMapChest), nameof(TreasureMapChest.Reinitialize))]
        public static class ChestPatcher2
        {
            public static bool IsFound;
            public static void Prefix(TreasureMapChest __instance)
            {
                var tmp = GameObject.Instantiate(CoolerEpicLootChests.testfx1, __instance.gameObject.transform.Find("New"), false);
                var iconthing = tmp.GetComponent<IconThing>();
                tmp.transform.localPosition = new Vector3(0, 1.46f, 0);
                iconthing.gameObject.transform.RotateAround(tmp.transform.position, tmp.transform.up, 90 * Time.deltaTime);
                iconthing._iconType = IconThing.IconType.Exclaim;
            }

            
        }
        
        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.CopyOtherDB))]
        public static class ObjectDBpatch
        {
            public static void Prefix(ObjectDB __instance)
            {
                if(__instance.GetItemPrefab("Wood") == null) return;
                var Legendary = __instance.GetItemPrefab("ShardLegendary");
                GameObject.Instantiate(CoolerEpicLootChests.testfx2, Legendary.transform);

                var Essence = __instance.GetItemPrefab("EssenceLegendary");
                GameObject.Instantiate(CoolerEpicLootChests.testfx2, Essence.transform);

                var rune = __instance.GetItemPrefab("RunestoneLegendary");
                GameObject.Instantiate(CoolerEpicLootChests.testfx2, rune.transform);

                var shard = __instance.GetItemPrefab("ShardEpic");
                GameObject.Instantiate(CoolerEpicLootChests.testfx2, shard.transform);
                
                __instance.m_items.Add(CoolerEpicLootChests.treasurebox);
            }
        }

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(typeof(EpicLoot.Adventure.Feature.TreasureMapsAdventureFeature),
            "CreateTreasureChest")]
        public static class NewChestUsePatch
        {
            public static bool Prefix(EpicLoot.Adventure.Feature.TreasureMapsAdventureFeature __instance, Heightmap.Biome biome, Player player, Vector3 spawnPoint, Vector3 normal, AdventureSaveData saveData, Action<bool, Vector3> callback)
            {
                const string treasureChestPrefabName = "piece_loot_chest";
                var treasureChestPrefab = ZNetScene.instance.GetPrefab(treasureChestPrefabName);
                var treasureChestObject = Object.Instantiate(treasureChestPrefab, spawnPoint, Quaternion.FromToRotation(Vector3.up, normal));
                var treasureChest = treasureChestObject.AddComponent<TreasureMapChest>();
                treasureChest.Setup(player, biome, __instance.GetCurrentInterval());

                var offset2 = UnityEngine.Random.insideUnitCircle * AdventureDataManager.Config.TreasureMap.MinimapAreaRadius;
                var offset = new Vector3(offset2.x, 0.05f, offset2.y);
                saveData.PurchasedTreasureMap(__instance.GetCurrentInterval(), biome, spawnPoint, offset);
                player.SaveAdventureSaveData();
                Minimap.instance.ShowPointOnMap(spawnPoint + offset);

                callback?.Invoke(true, spawnPoint);
                return false;
            }
        }

        [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
        public static class ZnetPatch
        {
            public static void Prefix(ZNetScene __instance)
            {
                if (__instance.m_prefabs.Count <= 0) return;
                __instance.m_prefabs.Add(CoolerEpicLootChests.treasurebox);
                __instance.m_prefabs.Add(CoolerEpicLootChests.testfx1);
                __instance.m_prefabs.Add(CoolerEpicLootChests.testfx2);
                __instance.m_prefabs.Add(CoolerEpicLootChests.vfx_open);
            }
        }
    }
}
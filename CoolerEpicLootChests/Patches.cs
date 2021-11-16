using EpicLoot.Adventure;
using HarmonyLib;
using UnityEngine;

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
                iconthing.gameObject.transform.RotateAround(tmp.transform.position, tmp.transform.up, 45 * Time.deltaTime);
                iconthing._iconType = IconThing.IconType.Exclaim;
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
                }
            }
        }
    }
}
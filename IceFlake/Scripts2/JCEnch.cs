using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanCore.API;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class JCEnch : Script
    {
        public JCEnch()
            : base("JC&Ench", "Profession")
        { }

        /* JC:
         * Jasper Ring (1x Jeweler's Setting, 1x Jasper)
         * Hessonite Ring (1x Jeweler's Setting, 2x Hessonite)
         * Nightstone Ring (1x Jeweler's Setting, 2x Nightstone)
         * Alicite Ring (1x Jeweler's Setting, 2x Alicite)
         */

        private readonly List<string> cOres = new List<string>()
        {
            "Obsidium Ore",
            "Elementium Ore",
            "Pyrite Ore",
        };

        private readonly List<string> cGems = new List<string>()
        {
            "Jasper",
            "Hessonite",
            "Nightstone",
            "Zephiryte",
            "Alicite",
        };

        private readonly List<string> cMats = new List<string>()
        {
            "Jeweler's Setting",
        };

        private IEnumerable<WoWItem> BagItems = null;

        private DateTime lastBagCheck = DateTime.MinValue;

        private int oreCount = 0;
        private int gemCount = 0;
        private int matCount = 0;

        public override void OnStart()
        {
            UpdateBags();
        }

        public override void OnTick()
        {
            var span = DateTime.Now - lastBagCheck;
            if (span.TotalSeconds >= 20)
            {
                UpdateBags();
            }
        }

        public override void OnTerminate()
        {
            BagItems = null;
            oreCount = 0;
            gemCount = 0;
            matCount = 0;
            lastBagCheck = DateTime.MinValue;
        }

        private void UpdateBags()
        {
            lastBagCheck = DateTime.Now;

            BagItems = Manager.LocalPlayer.InventoryItems;
            Print("Dumping materials");

            Print("Ores:");
            foreach (var i in cOres)
            {
                var ore = BagItems.Where(x => x.Name == i);
                oreCount = (int)ore.Sum(x => x.StackCount);
                Print("\t{0}: {1}", i, oreCount);
            }

            Print("Gems:");
            foreach (var i in cGems)
            {
                var gem = BagItems.Where(x => x.Name == i);
                gemCount = (int)gem.Sum(x => x.StackCount);
                Print("\t{0}: {1}", i, gemCount);
            }

            Print("Materials:");
            foreach (var i in cMats)
            {
                var mat = BagItems.Where(x => x.Name == i);
                matCount = (int)mat.Sum(x => x.StackCount);
                Print("\t{0}: {1}", i, matCount);
            }
        }

        public enum ProfState
        {
            Prospecting,
            Crafting,
            Disenchanting,
        }
    }
}

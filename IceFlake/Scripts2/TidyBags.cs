using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class TidyBags : Script
    {
        public TidyBags()
            : base("TidyBags", "Uncatalogued")
        { }

        #region Items
        private List<uint> _itemList = new List<uint>()
      {
         5523,  // Small Barnacled Clam
         5524,  // Thick-shelled Clam
         7973,  // Big-mouth Clam
         24476, // Jaggal Clam
         33567, // Borean Leather Scraps
         36781, // Darkwater Clam
         44700, // Brooding Darkwater Clam
         45909, // Giant Darkwater Clam
		 52340, // Abyssal Clam
		 67495, // Strange Bloated Stomach
         37700, // Crystallized Air
         37701, // Crystallized Earth
         37702, // Crystallized Fire
         37703, // Crystallized Shadow
         37704, // Crystallized Life
         37705, // Crystallized Water
         22572, // Mote of Air
         22573, // Mote of Earth 
         22574, // Mote of Fire
         22575, // Mote of Life
         22576, // Mote of Mana
         22577, // Mote of Shadow
         22578,	// Mote of Water
		 3352,	// Ooze-covered Bag
		 20766, // Slimy Bag
		 20767,	// Scum Covered Bag
		 20768, // Oozing Bag
		 44663,	// Abandoned Adventurer's Satchel
		 27511, // Inscribed Scrollcase
		 20708, // Tightly Sealed Trunk
		 6351, // Dented Crate
		 6352, // Waterlogged Crate
		 6353, // Small Chest
		 6356, // Battered Chest
		 6357, // Sealed Crate
		 13874, // Heavy Crate
		 21113, // Watertight Trunk
		 21150, // Iron Bound Trunk
		 21228, // Mithril Bound Trunk
		 27513, // Curious Crate
		 27481, // Heavy Supply Crate
		 44475, // Reinforced Crate
	     67539, // Tiny Treasure Chest
		 67597, // Sealed Crate (level 85 version)
		 24881, // Satchel of Helpful Goods (5-15 1st)
		 24889, // Satchel of Helpful Goods (5-15 others)
		 24882, // Satchel of Helpful Goods (15-25 1st)
		 24890, // Satchel of Helpful Goods (15-25 others)
		 51999, // Satchel of Helpful Goods (iLevel 25)
		 52000, // Satchel of Helpful Goods (31)
		 67248, // Satchel of Helpful Goods (39)
		 52001, // Satchel of Helpful Goods (41)
		 52002, // Satchel of Helpful Goods (50)
		 52003, // Satchel of Helpful Goods (57)
		 52004, // Satchel of Helpful Goods (62)
		 52005, // Satchel of Helpful Goods (66)
		 67250, // Satchel of Helpful Goods (85)
		 69903, // Satchel of Exotic Mysteries (LFD - Extra Reward)
		 32724, // Sludge Covered Object
		 61387 // Hidden Stash
      };
        #endregion

        public override void OnStart()
        {
        }

        public override void OnTerminate()
        {
        }

        public override void OnTick()
        {
            if (!Manager.IsInGame)
                return;

            foreach (var bi in Manager.LocalPlayer.InventoryItems)
            {
                if (_itemList.Contains(bi.Entry))
                {
                    if (bi.ItemSparseInfo.MaxCount >= bi.StackCount)
                        bi.Use();
                }
            }
            Sleep(5000);
        }
    }
}

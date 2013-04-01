using System.Collections.Generic;
using System.Linq;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.API
{
    public class Merchant : Frame
    {
        public Merchant()
            : base("MerchantFrame")
        {
        }

        public bool CanRepair
        {
            get { return WoWScript.Execute<bool>("CanMerchantRepair()"); }
        }

        public int RepairAllCost
        {
            get { return WoWScript.Execute<int>("GetRepairAllCost()"); }
        }

        public int NumItems
        {
            get { return WoWScript.Execute<int>("GetMerchantNumItems()"); }
        }

        public List<MerchantItem> Items
        {
            get { return Enumerable.Range(1, NumItems + 1).Select(i => GetItemInfo(i)).ToList(); }
        }

        public void RepairAll()
        {
            WoWScript.ExecuteNoResults("RepairAllItems()");
        }

        public void SellAll(ItemQuality quality)
        {
            WoWScript.ExecuteNoResults(
                "for i=0,4 do for j=1, GetContainerNumSlots(i) do l=GetContainerItemLink(i,j) if l then _,_,q=GetItemInfo(l) if q == " +
                (int) quality + " then UseContainerItem(i,j) end end end end");
        }

        public MerchantItem GetItemInfo(int index)
        {
            return new MerchantItem(index);
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseMerchant()");
        }
    }

    public class MerchantItem
    {
        public MerchantItem(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public string Name
        {
            get { return WoWScript.Execute<string>("GetMerchantItemInfo(" + Index + ")", 0); }
        }

        public int Price
        {
            get { return WoWScript.Execute<int>("GetMerchantItemInfo(" + Index + ")", 2); }
        }

        public int Quantity
        {
            get { return WoWScript.Execute<int>("GetMerchantItemInfo(" + Index + ")", 3); }
        }

        public int NumAvailable
        {
            get { return WoWScript.Execute<int>("GetMerchantItemInfo(" + Index + ")", 4); }
        }

        public bool IsUsable
        {
            get { return WoWScript.Execute<bool>("GetMerchantItemInfo(" + Index + ")", 5); }
        }

        public bool HasExtendedCost
        {
            get { return WoWScript.Execute<bool>("GetMerchantItemInfo(" + Index + ")", 6); }
        }

        public int MaxStack
        {
            get { return WoWScript.Execute<int>("GetMerchantItemMaxStack(" + Index + ")"); }
        }

        public int HonorCost
        {
            get { return WoWScript.Execute<int>("GetMerchantItemCostInfo(" + Index + ")", 0); }
        }

        public int ArenaCost
        {
            get { return WoWScript.Execute<int>("GetMerchantItemCostInfo(" + Index + ")", 1); }
        }

        public int ItemCost
        {
            get { return WoWScript.Execute<int>("GetMerchantItemCostInfo(" + Index + ")", 2); }
        }

        public void Buy()
        {
            WoWScript.ExecuteNoResults("BuyMerchantItem(" + Index + ")");
        }

        public void Buy(int quantity)
        {
            while (quantity > 0)
            {
                int amount = (quantity > MaxStack ? MaxStack : quantity);
                WoWScript.ExecuteNoResults("BuyMerchantItem(" + Index + ", " + amount + ")");
                quantity -= amount;
            }
        }
    }
}
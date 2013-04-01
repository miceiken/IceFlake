namespace IceFlake.Client.API
{
    public class Auction : Frame
    {
        public Auction()
            : base("AuctionFrame")
        {
        }

        //public void PostItem(WoWItem item, int minbid, int buyout, int runtime, int stacksize, int numStacks)
        //{
        //    int container, slot;
        //    if (item.GetSlotIndexes(out container, out slot))
        //    {
        //        WoWScript.ExecuteNoResults(
        //            "PickupContainerItem(" + container + ", " + slot + ")" +
        //            "ClickAuctionSellItemButton()" +
        //            "ClearCursor()" +
        //            "StartAuction(" + minbid + ", " + buyout + ", " + runtime + ", " + stacksize + ", " + numStacks + ")"
        //        );
        //    }
        //}

        //public void Search(string name, int minLvl = 0, int maxLvl = 0, int invType = 0, int classIdx = 0, int subclassIdx = 0, bool isUsable = false, int page = 0, int qualityIdx = 0, bool getAll = false)
        //{
        //    var _isUsable = isUsable ? 1 : 0;
        //    var _getAll = getAll ? 1 : 0;
        //    WoWScript.ExecuteNoResults(
        //        "QueryAuctionItems(" + name + ", " + minLvl + ", + " + maxLvl + ", " + classIdx + ", " + subclassIdx + ", " + _isUsable + ", " + page + ", " + qualityIdx + ", " + _getAll + ")"
        //    );
        //}

        //private int ScanPage = -1;
        //public bool FullSearch(string name)
        //{
        //    if (ScanPage == -1)
        //    {
        //        ScanPage = 0;
        //        WoWScript.ExecuteNoResults("QueryAuctionItems(" + name + ", 0, 0, 0, 0, 0, 0)");
        //    }
        //    else
        //    {
        //        WoWScript.ExecuteNoResults("QueryAuctionItems(" + name + ", 0, 0, 0, 0, 0, " + ScanPage + ")");
        //        var totalAuctions = WoWScript.Execute<int>("GetNumAuctionItems(\"list\")");
        //        if (ScanPage >= (int)Math.Ceiling((double)totalAuctions) / 50)
        //        { // Scan complete
        //            ScanPage = -1;
        //            return true;
        //        }
        //        ScanPage++;
        //    }
        //    return false;
        //}

        //public List<WoWAuction> ListAuctions
        //{
        //    get { return Enumerable.Range(0, WoWAuction.ListCount).Select(i => new WoWAuction(AuctionListType.List, i)).Where(a => a.IsValid).ToList(); }
        //}

        //public List<WoWAuction> BidderAuctions
        //{
        //    get { return Enumerable.Range(0, WoWAuction.BidderCount).Select(i => new WoWAuction(AuctionListType.Bidder, i)).Where(a => a.IsValid).ToList(); }
        //}

        //public List<WoWAuction> OwnerAuctions
        //{
        //    get { return Enumerable.Range(0, WoWAuction.OwnerCount).Select(i => new WoWAuction(AuctionListType.Owner, i)).Where(a => a.IsValid).ToList(); }
        //}
    }
}
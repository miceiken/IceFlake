namespace IceFlake.Client.API
{
    public static class API
    {
        // Frame stuff
        public static Gossip Gossip = new Gossip();
        public static Merchant Merchant = new Merchant();
        public static Trainer Trainer = new Trainer();
        public static QuestGossip Quest = new QuestGossip();
        public static Taxi Taxi = new Taxi();
        public static Talent Talent = new Talent();
        public static Auction Auction = new Auction();
        public static Companion Companion = new Companion();
        public static TradeSkill TradeSkill = new TradeSkill();

        // API stuff
        public static Login Login = new Login();
        public static Profession Profession = new Profession();
    }
}
using System.Collections.Generic;
using System.Linq;

namespace IceFlake.Client.API
{
    public class Taxi : Frame
    {
        public Taxi()
            : base("TaxiFrame")
        {
        }

        public int NumNodes
        {
            get { return WoWScript.Execute<int>("NumTaxiNodes()"); }
        }

        public List<TaxiNode> Nodes
        {
            get { return Enumerable.Range(1, NumNodes + 1).Select(i => new TaxiNode(i)).ToList(); }
        }

        public List<TaxiNode> ReachableNodes
        {
            get { return Nodes.Where(x => x.Type.ToLower() == "reachable").ToList(); }
        }

        public TaxiNode CurrentNode
        {
            get { return Nodes.FirstOrDefault(x => x.Type.ToLower() == "current"); }
        }

        public bool OnTaxi
        {
            get { return WoWScript.Execute<bool>("UnitOnTaxi(\"player\")"); }
        }
    }

    public class TaxiNode
    {
        public TaxiNode(int slot)
        {
            Slot = slot;
        }

        public int Slot { get; private set; }

        public int Cost
        {
            get { return WoWScript.Execute<int>("TaxiNodeCost(" + Slot + ")"); }
        }

        public string Name
        {
            get { return WoWScript.Execute<string>("TaxiNodeName(" + Slot + ")"); }
        }

        // Returns CURRENT, REACHABLE or NONE
        public string Type
        {
            get { return WoWScript.Execute<string>("TaxiNodeGetType(" + Slot + ")"); }
        }

        public int X
        {
            get { return WoWScript.Execute<int>("TaxiNodePosition(" + Slot + ")", 0); }
        }

        public int Y
        {
            get { return WoWScript.Execute<int>("TaxiNodePosition(" + Slot + ")", 1); }
        }
    }
}
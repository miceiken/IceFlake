using System;
using System.Collections.Generic;
using IceFlake.Client.Patchables;

namespace IceFlake.Client.API
{
    public class Gossip : Frame
    {
        public Gossip()
            : base("GossipFrame")
        {
        }

        public int NumOptions
        {
            get { return WoWScript.Execute<int>("GetNumGossipOptions()"); }
        }

        public IEnumerable<GossipOption> Options
        {
            get
            {
                List<string> ret = WoWScript.Execute("GetGossipOptions()");
                if (ret.Count > 0)
                {
                    for (int i = 0; i < ret.Count / 2; i++)
                    {
                        int idx = i * 2;
                        yield return new GossipOption
                                      {
                                          Index = i + 1,
                                          Title = ret[idx],
                                          Gossip = (GossipType)Enum.Parse(typeof(GossipType), ret[idx + 1], true)
                                      };
                    }
                }
            }
        }

        public int NumAvailableQuests
        {
            get { return WoWScript.Execute<int>("GetNumGossipAvailableQuests()"); }
        }

        public IEnumerable<GossipAvailableQuest> AvailableQuests
        {
            get
            {
                List<string> ret = WoWScript.Execute("GetGossipAvailableQuests()");
                if (ret.Count > 0)
                {
                    for (int i = 0; i < ret.Count / 5; i++)
                    {
                        int idx = i * 5;
                        yield return new GossipAvailableQuest
                                      {
                                          Index = i + 1,
                                          Title = ret[idx],
                                          Level = int.Parse(ret[idx + 1]),
                                          IsLowLevel = !(ret[idx + 2] == "0" || ret[idx + 2] == "nil"),
                                          IsDaily = !(ret[idx + 3] == "0" || ret[idx + 3] == "nil"),
                                          IsRepeatable = !(ret[idx + 4] == "0" || ret[idx + 4] == "nil"),
                                      };
                    }
                }
            }
        }

        public int NumActiveQuests
        {
            get { return WoWScript.Execute<int>("GetNumGossipActiveQuests()"); }
        }

        public IEnumerable<GossipActiveQuest> ActiveQuests
        {
            get
            {
                List<string> ret = WoWScript.Execute("GetGossipActiveQuests()");
                if (ret.Count > 0)
                {
                    for (int i = 0; i < ret.Count / 4; i++)
                    {
                        int idx = i * 4;
                        yield return new GossipActiveQuest
                                      {
                                          Index = i + 1,
                                          Title = ret[idx],
                                          Level = int.Parse(ret[idx + 1]),
                                          IsLowLevel = !(ret[idx + 2] == "0" || ret[idx + 2] == "nil"),
                                          IsComplete = !(ret[idx + 3] == "0" || ret[idx + 3] == "nil"),
                                      };
                    }
                }
            }
        }

        public void Close()
        {
            WoWScript.ExecuteNoResults("CloseGossip()");
        }
    }

    public class GossipOption
    {
        public GossipType Gossip;
        public int Index;

        public string Title;

        public void Select()
        {
            WoWScript.ExecuteNoResults("SelectGossipOption(" + Index + ")");
        }
    }

    public class GossipAvailableQuest
    {
        public int Index;

        public bool IsDaily;
        public bool IsLowLevel;
        public bool IsRepeatable;
        public int Level;
        public string Title;

        public void Select()
        {
            WoWScript.ExecuteNoResults("SelectGossipAvailableQuest(" + Index + ")");
        }
    }

    public class GossipActiveQuest
    {
        public int Index;

        public bool IsComplete;
        public bool IsLowLevel;
        public int Level;
        public string Title;

        public void Select()
        {
            WoWScript.ExecuteNoResults("SelectGossipActiveQuest(" + Index + ")");
        }
    }
}
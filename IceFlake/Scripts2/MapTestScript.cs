using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cleanCore;
using cleanLayer.Library.Scripts;

namespace cleanLayer.Scripts
{
    public class MapTestScript : Script
    {
        public MapTestScript()
            : base("Map Test", "Uncatalogued")
        { }

        public override void OnStart()
        {
            var mid = WoWWorld.CurrentMapId;
            var tbl = WoWDB.GetTable(ClientDB.Map);
            //Print("MinIdx: {0} MaxIdx: {1}", tbl.MinIndex, tbl.MaxIndex);            
            //for (var i = tbl.MinIndex; i < tbl.MaxIndex; i++)
            //{
            //    var map = tbl.GetRow(i).GetStruct<MapRec>();
            //    Print(map.InternalName);
            //}
            var row = tbl.GetRow(mid);
            var map = row.GetStruct<MapRec>();
            Print("#{0}: {1} ({2})", map.ID, map.Name, map.InternalName);
            Stop();
        }
    }
}

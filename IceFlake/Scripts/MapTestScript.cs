using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Scripts;
using IceFlake.Client.Patchables;


namespace IceFlake.Scripts
{
    public class MapTestScript : Script
    {
        public MapTestScript()
            : base("Map Test", "Uncatalogued")
        { }

        public override void OnStart()
        {
            var mid = World.CurrentMapId;
            var tbl = Manager.DBC[ClientDB.Map];
            //Print("MinIdx: {0} MaxIdx: {1}", tbl.MinIndex, tbl.MaxIndex);            
            //for (var i = tbl.MinIndex; i < tbl.MaxIndex; i++)
            //{
            //    var map = tbl.GetRow(i).GetStruct<MapRec>();
            //    Print(map.InternalName);
            //}
            var row = tbl.GetRow(mid);
            var map = row.GetStruct<MapRec>();
            Print("#{0}: {1})", map.m_ID, map._m_MapName_lang);
            Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client.Scripts;
using IceFlake.Client;
using IceFlake.Client.API;

namespace IceFlake.Scripts
{
    public class CompanionScript : Script
    {
        public CompanionScript()
            : base("Companions", "Uncatalogued")
        { }

        private CompanionForm ScriptForm;

        private bool ShouldRefresh = false;
        private bool ShouldSummon = false;
        private int SummonSpellId = -1;

        public override void OnStart()
        {
            ScriptForm = new CompanionForm(this);
            Program.CreateForm(ScriptForm);
            ShouldRefresh = false;
        }

        public override void OnTerminate()
        {
            ScriptForm.Close();
        }

        public override void OnTick()
        {
            if (ShouldRefresh)
            {
                if (ScriptForm == null || ScriptForm.IsDisposed)
                    return;
                Print("Fetching mounts and updating form...");
                ScriptForm.UpdateList(API.Companion.Mounts);
                ShouldRefresh = false;
            }
            if (ShouldSummon)
            {
                var mt = API.Companion.Mounts.FirstOrDefault(x => x.SpellId == SummonSpellId);
                mt.Summon();
                Print("Summoning {0}", mt.Name);
                SummonSpellId = -1;
                ShouldSummon = false;
            }

            Sleep(100);
        }

        public void HandleCommands(string cmd, List<string> args)
        {
            switch (cmd)
            {
                case "fetch":
                    ShouldRefresh = true;
                    break;
                case "summon":
                    ShouldSummon = true;
                    SummonSpellId = int.Parse(args[0]);
                    break;
            }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cleanCore.API;

namespace IceFlake.Scripts
{
    public partial class CompanionForm : Form
    {
        public CompanionForm(CompanionScript script)
        {
            Script = script;
            InitializeComponent();
            dgCompanions.DataSource = Items;
        }

        private BindingList<WoWCompanion> Items = new BindingList<WoWCompanion>();
        private CompanionScript Script;

        private void btnFetch_Click(object sender, EventArgs e)
        {
            Script.HandleCommands("fetch", new List<string>());
        }

        private void btnSummon_Click(object sender, EventArgs e)
        {
            var obj = dgCompanions.SelectedRows[0].DataBoundItem as WoWCompanion;
            if (obj == null)
                return;
            Script.HandleCommands("summon", new List<string>() { obj.SpellId.ToString() });
        }

        public void UpdateList(List<WoWCompanion> comps)
        {
            Items.Clear();
            foreach (var mt in comps)
                Items.Add(mt);
        }
    }
}

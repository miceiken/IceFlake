using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;

namespace IceFlake
{
    public partial class IceForm : Form, ILog
    {
        public IceForm()
        {
            InitializeComponent();
        }

        private Location ToPathTo;

        #region ILog Members

        public void WriteLine(LogEntry entry)
        {
            Color logColor;
            switch (entry.Type)
            {
                case LogType.Error:
                    logColor = Color.Red;
                    break;
                case LogType.Warning:
                    logColor = Color.OrangeRed;
                    break;
                case LogType.Information:
                    logColor = Color.SteelBlue;
                    break;
                case LogType.Good:
                    logColor = Color.ForestGreen;
                    break;
                case LogType.Normal:
                default:
                    logColor = Color.Black;
                    break;
            }

            rbLogBox.Invoke((Action)(() =>
            {
                rbLogBox.SelectionColor = logColor;
                rbLogBox.AppendText(entry.FormattedMessage + Environment.NewLine);
                rbLogBox.ScrollToCaret();
            }));
        }

        #endregion

        private void IceForm_Load(object sender, EventArgs e)
        {
            Log.AddReader(this);
            Log.WriteLine("CoreForm loaded");
        }

        private void btnDump_Click(object sender, EventArgs e)
        {
            try
            {
                var plrname = WoWScript.Execute<string>("UnitName(\"player\")");
                Log.WriteLine(plrname);

                Log.WriteLine("SpellBook:");
                foreach (var spell in Manager.Spellbook)
                    Log.WriteLine("#{0}: {1}", spell.Id, spell.Name);

                Log.WriteLine("All Items:");
                foreach (var item in Manager.LocalPlayer.Items.Where(i => i != null))
                    Log.WriteLine("\t{0}", item.Name);

                Log.WriteLine("Inventory Items");
                foreach (var item in Manager.LocalPlayer.InventoryItems.Where(i => i != null))
                {
                    int x, y;
                    if (item.GetSlotIndexes(out x, out y))
                        Log.WriteLine("\t({0},{1}) {2}", x, y, item.Name);
                    else
                        Log.WriteLine("\t{0}", item.Name);
                }

                Log.WriteLine("Equipped Items:");
                foreach (var item in Manager.LocalPlayer.EquippedItems.Where(i => i != null))
                    Log.WriteLine("\t{0}", item.Name);

                Log.WriteLine("Equipped Items:");
                for (var i = (int)EquipSlot.Head; i < (int)EquipSlot.Tabard + 1; i++)
                {
                    var item = Manager.LocalPlayer.GetEquippedItem(i);
                    if (item == null || !item.IsValid) continue;
                    Log.WriteLine("{0}: {1}", (EquipSlot)i, item.Name);
                }
            }
            catch { }
        }

        private void btnGeneratePath_Click(object sender, EventArgs e)
        {
            if (ToPathTo == default(Location))
            {
                ToPathTo = Manager.LocalPlayer.Location;
            }
            else
            {
                Manager.Movement.PathTo(ToPathTo);
            }
        }

        private void IceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.RemoveReader(this);
            DirectX.Direct3D.Shutdown();
        }
    }
}

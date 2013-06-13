namespace IceFlake
{
    partial class IceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rbLogBox = new System.Windows.Forms.RichTextBox();
            this.btnGeneratePath = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStatus = new System.Windows.Forms.TabPage();
            this.gbPlayer = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPowerText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblHealth = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblZone = new System.Windows.Forms.Label();
            this.tabScripts = new System.Windows.Forms.TabPage();
            this.lstScripts = new System.Windows.Forms.CheckedListBox();
            this.btnScriptCompile = new System.Windows.Forms.Button();
            this.btnScriptStop = new System.Windows.Forms.Button();
            this.btnScriptStart = new System.Windows.Forms.Button();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tbLUA = new System.Windows.Forms.TextBox();
            this.btnDump = new System.Windows.Forms.Button();
            this.GUITimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.gbPlayer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabScripts.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbLogBox
            // 
            this.rbLogBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbLogBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbLogBox.Location = new System.Drawing.Point(0, 213);
            this.rbLogBox.Name = "rbLogBox";
            this.rbLogBox.ReadOnly = true;
            this.rbLogBox.Size = new System.Drawing.Size(361, 298);
            this.rbLogBox.TabIndex = 0;
            this.rbLogBox.Text = "";
            // 
            // btnGeneratePath
            // 
            this.btnGeneratePath.Location = new System.Drawing.Point(0, 0);
            this.btnGeneratePath.Name = "btnGeneratePath";
            this.btnGeneratePath.Size = new System.Drawing.Size(75, 23);
            this.btnGeneratePath.TabIndex = 2;
            this.btnGeneratePath.Text = "Path";
            this.btnGeneratePath.UseVisualStyleBackColor = true;
            this.btnGeneratePath.Click += new System.EventHandler(this.btnGeneratePath_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStatus);
            this.tabControl1.Controls.Add(this.tabScripts);
            this.tabControl1.Controls.Add(this.tabDebug);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 213);
            this.tabControl1.TabIndex = 3;
            // 
            // tabStatus
            // 
            this.tabStatus.Controls.Add(this.gbPlayer);
            this.tabStatus.Location = new System.Drawing.Point(4, 22);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatus.Size = new System.Drawing.Size(353, 187);
            this.tabStatus.TabIndex = 2;
            this.tabStatus.Text = "Status";
            this.tabStatus.UseVisualStyleBackColor = true;
            // 
            // gbPlayer
            // 
            this.gbPlayer.Controls.Add(this.tableLayoutPanel1);
            this.gbPlayer.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbPlayer.Location = new System.Drawing.Point(3, 3);
            this.gbPlayer.Name = "gbPlayer";
            this.gbPlayer.Size = new System.Drawing.Size(184, 181);
            this.gbPlayer.TabIndex = 0;
            this.gbPlayer.TabStop = false;
            this.gbPlayer.Text = "Player";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPowerText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblHealth, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPower, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLevel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblZone, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(178, 162);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Health:";
            // 
            // lblPowerText
            // 
            this.lblPowerText.AutoSize = true;
            this.lblPowerText.Location = new System.Drawing.Point(3, 13);
            this.lblPowerText.Name = "lblPowerText";
            this.lblPowerText.Size = new System.Drawing.Size(40, 13);
            this.lblPowerText.TabIndex = 1;
            this.lblPowerText.Text = "Power:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Level:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Zone:";
            // 
            // lblHealth
            // 
            this.lblHealth.AutoSize = true;
            this.lblHealth.Location = new System.Drawing.Point(50, 0);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(63, 13);
            this.lblHealth.TabIndex = 4;
            this.lblHealth.Text = "<unknown>";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(50, 13);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(63, 13);
            this.lblPower.TabIndex = 5;
            this.lblPower.Text = "<unknown>";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(50, 26);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(63, 13);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "<unknown>";
            // 
            // lblZone
            // 
            this.lblZone.AutoSize = true;
            this.lblZone.Location = new System.Drawing.Point(50, 39);
            this.lblZone.Name = "lblZone";
            this.lblZone.Size = new System.Drawing.Size(63, 13);
            this.lblZone.TabIndex = 7;
            this.lblZone.Text = "<unknown>";
            // 
            // tabScripts
            // 
            this.tabScripts.Controls.Add(this.lstScripts);
            this.tabScripts.Controls.Add(this.btnScriptCompile);
            this.tabScripts.Controls.Add(this.btnScriptStop);
            this.tabScripts.Controls.Add(this.btnScriptStart);
            this.tabScripts.Location = new System.Drawing.Point(4, 22);
            this.tabScripts.Name = "tabScripts";
            this.tabScripts.Padding = new System.Windows.Forms.Padding(3);
            this.tabScripts.Size = new System.Drawing.Size(353, 187);
            this.tabScripts.TabIndex = 0;
            this.tabScripts.Text = "Scripts";
            this.tabScripts.UseVisualStyleBackColor = true;
            // 
            // lstScripts
            // 
            this.lstScripts.FormattingEnabled = true;
            this.lstScripts.Location = new System.Drawing.Point(6, 7);
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.Size = new System.Drawing.Size(339, 139);
            this.lstScripts.TabIndex = 4;
            this.lstScripts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstScripts_ItemCheck);
            this.lstScripts.SelectedIndexChanged += new System.EventHandler(this.lstScripts_SelectedIndexChanged);
            // 
            // btnScriptCompile
            // 
            this.btnScriptCompile.Image = global::IceFlake.Properties.Resources.brick_go;
            this.btnScriptCompile.Location = new System.Drawing.Point(168, 156);
            this.btnScriptCompile.Name = "btnScriptCompile";
            this.btnScriptCompile.Size = new System.Drawing.Size(75, 23);
            this.btnScriptCompile.TabIndex = 3;
            this.btnScriptCompile.Text = "Compile";
            this.btnScriptCompile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptCompile.UseVisualStyleBackColor = true;
            this.btnScriptCompile.Click += new System.EventHandler(this.btnScriptCompile_Click);
            // 
            // btnScriptStop
            // 
            this.btnScriptStop.Image = global::IceFlake.Properties.Resources.control_stop_blue;
            this.btnScriptStop.Location = new System.Drawing.Point(87, 156);
            this.btnScriptStop.Name = "btnScriptStop";
            this.btnScriptStop.Size = new System.Drawing.Size(75, 23);
            this.btnScriptStop.TabIndex = 2;
            this.btnScriptStop.Text = "Stop";
            this.btnScriptStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptStop.UseVisualStyleBackColor = true;
            this.btnScriptStop.Click += new System.EventHandler(this.btnScriptStop_Click);
            // 
            // btnScriptStart
            // 
            this.btnScriptStart.Image = global::IceFlake.Properties.Resources.control_play_blue;
            this.btnScriptStart.Location = new System.Drawing.Point(6, 156);
            this.btnScriptStart.Name = "btnScriptStart";
            this.btnScriptStart.Size = new System.Drawing.Size(75, 23);
            this.btnScriptStart.TabIndex = 1;
            this.btnScriptStart.Text = "Start";
            this.btnScriptStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScriptStart.UseVisualStyleBackColor = true;
            this.btnScriptStart.Click += new System.EventHandler(this.btnScriptStart_Click);
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.btnExecute);
            this.tabDebug.Controls.Add(this.tbLUA);
            this.tabDebug.Controls.Add(this.btnDump);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(353, 187);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(181, 160);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tbLUA
            // 
            this.tbLUA.Location = new System.Drawing.Point(8, 162);
            this.tbLUA.Name = "tbLUA";
            this.tbLUA.Size = new System.Drawing.Size(167, 20);
            this.tbLUA.TabIndex = 1;
            // 
            // btnDump
            // 
            this.btnDump.Location = new System.Drawing.Point(8, 7);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(75, 23);
            this.btnDump.TabIndex = 0;
            this.btnDump.Text = "Dump";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.btnDump_Click);
            // 
            // GUITimer
            // 
            this.GUITimer.Enabled = true;
            this.GUITimer.Interval = 500;
            this.GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // IceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 511);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnGeneratePath);
            this.Controls.Add(this.rbLogBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IceForm";
            this.ShowIcon = false;
            this.Text = "IceFlake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IceForm_FormClosing);
            this.Load += new System.EventHandler(this.IceForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.gbPlayer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabScripts.ResumeLayout(false);
            this.tabDebug.ResumeLayout(false);
            this.tabDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rbLogBox;
        private System.Windows.Forms.Button btnGeneratePath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabScripts;
        private System.Windows.Forms.Button btnScriptCompile;
        private System.Windows.Forms.Button btnScriptStop;
        private System.Windows.Forms.Button btnScriptStart;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.CheckedListBox lstScripts;
        private System.Windows.Forms.Timer GUITimer;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox tbLUA;
        private System.Windows.Forms.TabPage tabStatus;
        private System.Windows.Forms.GroupBox gbPlayer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPowerText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHealth;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblZone;
    }
}


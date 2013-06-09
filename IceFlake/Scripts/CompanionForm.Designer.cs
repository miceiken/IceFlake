namespace IceFlake.Scripts
{
    partial class CompanionForm
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
            this.dgCompanions = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSummon = new System.Windows.Forms.Button();
            this.btnFetch = new System.Windows.Forms.Button();
            this.woWCompanionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.creatureIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spellIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.flagsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMountDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isGroundDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isFlyingDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgCompanions)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.woWCompanionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgCompanions
            // 
            this.dgCompanions.AllowUserToAddRows = false;
            this.dgCompanions.AllowUserToDeleteRows = false;
            this.dgCompanions.AutoGenerateColumns = false;
            this.dgCompanions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgCompanions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCompanions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.creatureIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.spellIdDataGridViewTextBoxColumn,
            this.activeDataGridViewCheckBoxColumn,
            this.flagsDataGridViewTextBoxColumn,
            this.isMountDataGridViewCheckBoxColumn,
            this.isGroundDataGridViewCheckBoxColumn,
            this.isFlyingDataGridViewCheckBoxColumn});
            this.dgCompanions.DataSource = this.woWCompanionBindingSource;
            this.dgCompanions.Location = new System.Drawing.Point(12, 12);
            this.dgCompanions.Name = "dgCompanions";
            this.dgCompanions.ReadOnly = true;
            this.dgCompanions.Size = new System.Drawing.Size(743, 375);
            this.dgCompanions.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSummon);
            this.groupBox1.Controls.Add(this.btnFetch);
            this.groupBox1.Location = new System.Drawing.Point(761, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // btnSummon
            // 
            this.btnSummon.Location = new System.Drawing.Point(6, 48);
            this.btnSummon.Name = "btnSummon";
            this.btnSummon.Size = new System.Drawing.Size(75, 23);
            this.btnSummon.TabIndex = 1;
            this.btnSummon.Text = "Summon";
            this.btnSummon.UseVisualStyleBackColor = true;
            this.btnSummon.Click += new System.EventHandler(this.btnSummon_Click);
            // 
            // btnFetch
            // 
            this.btnFetch.Location = new System.Drawing.Point(6, 19);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(75, 23);
            this.btnFetch.TabIndex = 0;
            this.btnFetch.Text = "Fetch";
            this.btnFetch.UseVisualStyleBackColor = true;
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // woWCompanionBindingSource
            // 
            this.woWCompanionBindingSource.DataSource = typeof(cleanCore.API.WoWCompanion);
            // 
            // creatureIdDataGridViewTextBoxColumn
            // 
            this.creatureIdDataGridViewTextBoxColumn.DataPropertyName = "CreatureId";
            this.creatureIdDataGridViewTextBoxColumn.HeaderText = "CreatureId";
            this.creatureIdDataGridViewTextBoxColumn.Name = "creatureIdDataGridViewTextBoxColumn";
            this.creatureIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // spellIdDataGridViewTextBoxColumn
            // 
            this.spellIdDataGridViewTextBoxColumn.DataPropertyName = "SpellId";
            this.spellIdDataGridViewTextBoxColumn.HeaderText = "SpellId";
            this.spellIdDataGridViewTextBoxColumn.Name = "spellIdDataGridViewTextBoxColumn";
            this.spellIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activeDataGridViewCheckBoxColumn
            // 
            this.activeDataGridViewCheckBoxColumn.DataPropertyName = "Active";
            this.activeDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.activeDataGridViewCheckBoxColumn.Name = "activeDataGridViewCheckBoxColumn";
            this.activeDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // flagsDataGridViewTextBoxColumn
            // 
            this.flagsDataGridViewTextBoxColumn.DataPropertyName = "Flags";
            this.flagsDataGridViewTextBoxColumn.HeaderText = "Flags";
            this.flagsDataGridViewTextBoxColumn.Name = "flagsDataGridViewTextBoxColumn";
            this.flagsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isMountDataGridViewCheckBoxColumn
            // 
            this.isMountDataGridViewCheckBoxColumn.DataPropertyName = "IsMount";
            this.isMountDataGridViewCheckBoxColumn.HeaderText = "IsMount";
            this.isMountDataGridViewCheckBoxColumn.Name = "isMountDataGridViewCheckBoxColumn";
            this.isMountDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isGroundDataGridViewCheckBoxColumn
            // 
            this.isGroundDataGridViewCheckBoxColumn.DataPropertyName = "IsGround";
            this.isGroundDataGridViewCheckBoxColumn.HeaderText = "IsGround";
            this.isGroundDataGridViewCheckBoxColumn.Name = "isGroundDataGridViewCheckBoxColumn";
            this.isGroundDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isFlyingDataGridViewCheckBoxColumn
            // 
            this.isFlyingDataGridViewCheckBoxColumn.DataPropertyName = "IsFlying";
            this.isFlyingDataGridViewCheckBoxColumn.HeaderText = "IsFlying";
            this.isFlyingDataGridViewCheckBoxColumn.Name = "isFlyingDataGridViewCheckBoxColumn";
            this.isFlyingDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // CompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 399);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgCompanions);
            this.Name = "CompanionForm";
            this.Text = "CompanionForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgCompanions)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.woWCompanionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgCompanions;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSummon;
        private System.Windows.Forms.Button btnFetch;
        private System.Windows.Forms.DataGridViewTextBoxColumn creatureIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn spellIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn flagsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMountDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isGroundDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isFlyingDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource woWCompanionBindingSource;
    }
}
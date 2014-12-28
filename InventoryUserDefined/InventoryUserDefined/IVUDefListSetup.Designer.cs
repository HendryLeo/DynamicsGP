namespace InventoryUserDefined
{
    partial class IVUDefListSetup
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdClear = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.txtUDef = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lstListItem = new System.Windows.Forms.ListBox();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdCancel.BackColor = System.Drawing.Color.Transparent;
            this.dexButtonProvider.SetButtonType(this.cmdCancel, Microsoft.Dexterity.Shell.DexButtonType.ToolbarWithSeparator);
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatAppearance.BorderSize = 0;
            this.cmdCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.Image = global::InventoryUserDefined.Properties.Resources.Toolbar_Cancel;
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdCancel.Location = new System.Drawing.Point(168, 0);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(72, 24);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCancel.UseVisualStyleBackColor = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdClear
            // 
            this.cmdClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdClear.BackColor = System.Drawing.Color.Transparent;
            this.dexButtonProvider.SetButtonType(this.cmdClear, Microsoft.Dexterity.Shell.DexButtonType.ToolbarWithSeparator);
            this.cmdClear.FlatAppearance.BorderSize = 0;
            this.cmdClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdClear.Image = global::InventoryUserDefined.Properties.Resources.Toolbar_Clear;
            this.cmdClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdClear.Location = new System.Drawing.Point(90, 0);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(72, 24);
            this.cmdClear.TabIndex = 7;
            this.cmdClear.Text = "Clear";
            this.cmdClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdClear.UseVisualStyleBackColor = false;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdSave.BackColor = System.Drawing.Color.Transparent;
            this.dexButtonProvider.SetButtonType(this.cmdSave, Microsoft.Dexterity.Shell.DexButtonType.ToolbarWithSeparator);
            this.cmdSave.FlatAppearance.BorderSize = 0;
            this.cmdSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSave.Image = global::InventoryUserDefined.Properties.Resources.Toolbar_Save;
            this.cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdSave.Location = new System.Drawing.Point(8, 0);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(72, 24);
            this.cmdSave.TabIndex = 6;
            this.cmdSave.Text = "Save";
            this.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSave.UseVisualStyleBackColor = false;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // txtUDef
            // 
            this.txtUDef.BackColor = System.Drawing.SystemColors.Window;
            this.txtUDef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUDef.Location = new System.Drawing.Point(90, 39);
            this.txtUDef.Name = "txtUDef";
            this.txtUDef.Size = new System.Drawing.Size(178, 13);
            this.txtUDef.TabIndex = 118;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 117;
            this.label5.Text = "User Defined";
            // 
            // lstListItem
            // 
            this.lstListItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstListItem.FormattingEnabled = true;
            this.lstListItem.Location = new System.Drawing.Point(150, 72);
            this.lstListItem.Name = "lstListItem";
            this.lstListItem.Size = new System.Drawing.Size(120, 208);
            this.lstListItem.TabIndex = 119;
            // 
            // txtItem
            // 
            this.txtItem.BackColor = System.Drawing.SystemColors.Window;
            this.txtItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtItem.Location = new System.Drawing.Point(8, 75);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(136, 13);
            this.txtItem.TabIndex = 120;
            // 
            // cmdAdd
            // 
            this.cmdAdd.BackColor = System.Drawing.SystemColors.Control;
            this.cmdAdd.Location = new System.Drawing.Point(4, 95);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdAdd.TabIndex = 121;
            this.cmdAdd.Text = "Add >>";
            this.cmdAdd.UseVisualStyleBackColor = false;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // IVUDefListSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 305);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.txtItem);
            this.Controls.Add(this.lstListItem);
            this.Controls.Add(this.txtUDef);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.cmdSave);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "IVUDefListSetup";
            this.StatusArea = false;
            this.Text = "IV_UDef_ListSetup";
            this.Activated += new System.EventHandler(this.IVUDefListSetup_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IVUDefListSetup_FormClosing);
            this.Load += new System.EventHandler(this.IVUDefListSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.TextBox txtUDef;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox lstListItem;
        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.Button cmdAdd;
    }
}


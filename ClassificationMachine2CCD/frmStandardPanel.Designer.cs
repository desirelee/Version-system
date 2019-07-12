namespace ClassificationMachine
{
    partial class frmStandardPanel
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
            this.txtWidthY = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtLengthX = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtIntervalY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIntervalX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtWidthY
            // 
            this.txtWidthY.Location = new System.Drawing.Point(135, 47);
            this.txtWidthY.Name = "txtWidthY";
            this.txtWidthY.Size = new System.Drawing.Size(110, 21);
            this.txtWidthY.TabIndex = 11;
            this.txtWidthY.Text = "100";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(39, 50);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(71, 12);
            this.label29.TabIndex = 10;
            this.label29.Text = "标定板长度Y";
            // 
            // txtLengthX
            // 
            this.txtLengthX.Location = new System.Drawing.Point(135, 12);
            this.txtLengthX.Name = "txtLengthX";
            this.txtLengthX.Size = new System.Drawing.Size(110, 21);
            this.txtLengthX.TabIndex = 9;
            this.txtLengthX.Text = "100";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(39, 15);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(71, 12);
            this.label28.TabIndex = 8;
            this.label28.Text = "标定板长度X";
            // 
            // txtIntervalY
            // 
            this.txtIntervalY.Enabled = false;
            this.txtIntervalY.Location = new System.Drawing.Point(135, 119);
            this.txtIntervalY.Name = "txtIntervalY";
            this.txtIntervalY.Size = new System.Drawing.Size(110, 21);
            this.txtIntervalY.TabIndex = 15;
            this.txtIntervalY.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "标定板间距Y";
            // 
            // txtIntervalX
            // 
            this.txtIntervalX.Enabled = false;
            this.txtIntervalX.Location = new System.Drawing.Point(135, 84);
            this.txtIntervalX.Name = "txtIntervalX";
            this.txtIntervalX.Size = new System.Drawing.Size(110, 21);
            this.txtIntervalX.TabIndex = 13;
            this.txtIntervalX.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "标定板间距X";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(164, 194);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 56);
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(12, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 56);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "终止";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmStandardPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 262);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtIntervalY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIntervalX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWidthY);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.txtLengthX);
            this.Controls.Add(this.label28);
            this.Name = "frmStandardPanel";
            this.Text = "标定板设定";
            this.Load += new System.EventHandler(this.frmStandardPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWidthY;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtLengthX;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtIntervalY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIntervalX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
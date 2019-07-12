namespace ClassificationMachine
{
    partial class FormAskNum
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
            this.button1 = new System.Windows.Forms.Button();
            this.rb_9 = new System.Windows.Forms.RadioButton();
            this.rb_4WithReference = new System.Windows.Forms.RadioButton();
            this.rb_4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(63, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 25);
            this.button1.TabIndex = 7;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rb_9
            // 
            this.rb_9.AutoSize = true;
            this.rb_9.Location = new System.Drawing.Point(55, 114);
            this.rb_9.Name = "rb_9";
            this.rb_9.Size = new System.Drawing.Size(83, 16);
            this.rb_9.TabIndex = 6;
            this.rb_9.TabStop = true;
            this.rb_9.Text = "九宫格测量";
            this.rb_9.UseVisualStyleBackColor = true;
            // 
            // rb_4WithReference
            // 
            this.rb_4WithReference.AutoSize = true;
            this.rb_4WithReference.Location = new System.Drawing.Point(55, 68);
            this.rb_4WithReference.Name = "rb_4WithReference";
            this.rb_4WithReference.Size = new System.Drawing.Size(179, 16);
            this.rb_4WithReference.TabIndex = 5;
            this.rb_4WithReference.TabStop = true;
            this.rb_4WithReference.Text = "四点测量(参考线)或六点测量";
            this.rb_4WithReference.UseVisualStyleBackColor = true;
            this.rb_4WithReference.CheckedChanged += new System.EventHandler(this.rb_4WithReference_CheckedChanged);
            // 
            // rb_4
            // 
            this.rb_4.AutoSize = true;
            this.rb_4.Location = new System.Drawing.Point(55, 22);
            this.rb_4.Name = "rb_4";
            this.rb_4.Size = new System.Drawing.Size(71, 16);
            this.rb_4.TabIndex = 4;
            this.rb_4.TabStop = true;
            this.rb_4.Text = "四点测量";
            this.rb_4.UseVisualStyleBackColor = true;
            // 
            // FormAskNum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 203);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rb_9);
            this.Controls.Add(this.rb_4WithReference);
            this.Controls.Add(this.rb_4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAskNum";
            this.Text = "测量程式";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAskNum_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rb_9;
        private System.Windows.Forms.RadioButton rb_4WithReference;
        private System.Windows.Forms.RadioButton rb_4;
    }
}
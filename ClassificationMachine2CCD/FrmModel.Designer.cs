namespace ClassificationMachine
{
    partial class FrmModel
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
            this.C = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.btnSaveModel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnMatch = new System.Windows.Forms.Button();
            this.imageViewerModel = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.SuspendLayout();
            // 
            // C
            // 
            this.C.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.C.Location = new System.Drawing.Point(10, 4);
            this.C.Margin = new System.Windows.Forms.Padding(11, 8, 11, 8);
            this.C.Name = "C";
            this.C.ShowImageInfo = true;
            this.C.ShowScrollbars = true;
            this.C.ShowToolbar = true;
            this.C.Size = new System.Drawing.Size(562, 506);
            this.C.TabIndex = 11;
            this.C.ToolsShown = NationalInstruments.Vision.WindowsForms.ViewerTools.Rectangle;
            this.C.ZoomToFit = true;
            this.C.RoiChanged += new System.EventHandler<NationalInstruments.Vision.WindowsForms.ContoursChangedEventArgs>(this.C_RoiChanged);
            // 
            // btnSaveModel
            // 
            this.btnSaveModel.Location = new System.Drawing.Point(660, 391);
            this.btnSaveModel.Name = "btnSaveModel";
            this.btnSaveModel.Size = new System.Drawing.Size(156, 45);
            this.btnSaveModel.TabIndex = 1;
            this.btnSaveModel.Text = "模板保存";
            this.btnSaveModel.UseVisualStyleBackColor = true;
            this.btnSaveModel.Click += new System.EventHandler(this.btnSaveModel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(660, 314);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(156, 45);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(660, 465);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(156, 45);
            this.btnMatch.TabIndex = 22;
            this.btnMatch.Text = "搜寻";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // imageViewerModel
            // 
            this.imageViewerModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewerModel.Location = new System.Drawing.Point(579, 4);
            this.imageViewerModel.Margin = new System.Windows.Forms.Padding(11, 8, 11, 8);
            this.imageViewerModel.Name = "imageViewerModel";
            this.imageViewerModel.Size = new System.Drawing.Size(330, 277);
            this.imageViewerModel.TabIndex = 12;
            this.imageViewerModel.ToolsShown = NationalInstruments.Vision.WindowsForms.ViewerTools.None;
            this.imageViewerModel.ZoomToFit = true;
            // 
            // FrmModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 527);
            this.Controls.Add(this.C);
            this.Controls.Add(this.imageViewerModel);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSaveModel);
            this.Name = "FrmModel";
            this.Text = "模板设定";
            this.Load += new System.EventHandler(this.FrmModel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSaveModel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnOK;
        public NationalInstruments.Vision.WindowsForms.ImageViewer C;
        private System.Windows.Forms.Button btnMatch;
        public NationalInstruments.Vision.WindowsForms.ImageViewer imageViewerModel;


    }
}
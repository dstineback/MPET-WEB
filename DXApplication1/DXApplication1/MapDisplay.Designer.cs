namespace DXApplication1
{
    partial class MapDisplay
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
            this.mapControl1 = new DevExpress.XtraMap.MapControl();
            this.mapControl2 = new DevExpress.XtraMap.MapControl();
            this.imageLayer1 = new DevExpress.XtraMap.ImageLayer();
            this.bingMapDataProvider1 = new DevExpress.XtraMap.BingMapDataProvider();
            ((System.ComponentModel.ISupportInitialize)(this.mapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // mapControl1
            // 
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(1253, 762);
            this.mapControl1.TabIndex = 0;
            // 
            // mapControl2
            // 
            this.mapControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl2.Layers.Add(this.imageLayer1);
            this.mapControl2.Location = new System.Drawing.Point(0, 0);
            this.mapControl2.Name = "mapControl2";
            this.mapControl2.Size = new System.Drawing.Size(888, 359);
            this.mapControl2.TabIndex = 0;
            // 
            // imageLayer1
            // 
            this.imageLayer1.DataProvider = this.bingMapDataProvider1;
            // 
            // bingMapDataProvider1
            // 
            this.bingMapDataProvider1.BingKey = "ZjhJ67W9I5S4DRLTSr3X~78OEInuF4CzjVqlDRnVLcg~ArUL4UQBAGeTRRpiGRnn04XH-s0YhdPs6ESDt" +
    "iArfH-8xv0fmq4DXY5Mv7KHAoM1";
            this.bingMapDataProvider1.Kind = DevExpress.XtraMap.BingMapKind.Road;
            // 
            // MapDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 359);
            this.Name = "MapDisplay";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MapDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraMap.MapControl mapControl1;
        private DevExpress.XtraMap.MapControl mapControl2;
        private DevExpress.XtraMap.ImageLayer imageLayer1;
        private DevExpress.XtraMap.BingMapDataProvider bingMapDataProvider1;
    }
}


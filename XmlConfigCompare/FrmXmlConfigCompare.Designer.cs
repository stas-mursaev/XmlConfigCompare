namespace XmlConfigCompare
{
    partial class FrmXmlConfigCompare
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
            this.tbProd = new System.Windows.Forms.TextBox();
            this.tbUAT = new System.Windows.Forms.TextBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.btnSelectProd = new System.Windows.Forms.Button();
            this.btnSelectUAT = new System.Windows.Forms.Button();
            this.btnSelectResult = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbProd
            // 
            this.tbProd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProd.Location = new System.Drawing.Point(79, 12);
            this.tbProd.Name = "tbProd";
            this.tbProd.Size = new System.Drawing.Size(260, 20);
            this.tbProd.TabIndex = 0;
            // 
            // tbUAT
            // 
            this.tbUAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUAT.Location = new System.Drawing.Point(79, 43);
            this.tbUAT.Name = "tbUAT";
            this.tbUAT.Size = new System.Drawing.Size(260, 20);
            this.tbUAT.TabIndex = 1;
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(79, 72);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(260, 20);
            this.tbResult.TabIndex = 2;
            // 
            // btnSelectProd
            // 
            this.btnSelectProd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectProd.Location = new System.Drawing.Point(345, 12);
            this.btnSelectProd.Name = "btnSelectProd";
            this.btnSelectProd.Size = new System.Drawing.Size(26, 23);
            this.btnSelectProd.TabIndex = 3;
            this.btnSelectProd.Text = "...";
            this.btnSelectProd.UseVisualStyleBackColor = true;
            this.btnSelectProd.Click += new System.EventHandler(this.btnSelectProd_Click);
            // 
            // btnSelectUAT
            // 
            this.btnSelectUAT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectUAT.Location = new System.Drawing.Point(345, 41);
            this.btnSelectUAT.Name = "btnSelectUAT";
            this.btnSelectUAT.Size = new System.Drawing.Size(26, 23);
            this.btnSelectUAT.TabIndex = 4;
            this.btnSelectUAT.Text = "...";
            this.btnSelectUAT.UseVisualStyleBackColor = true;
            this.btnSelectUAT.Click += new System.EventHandler(this.btnSelectUAT_Click);
            // 
            // btnSelectResult
            // 
            this.btnSelectResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectResult.Location = new System.Drawing.Point(345, 70);
            this.btnSelectResult.Name = "btnSelectResult";
            this.btnSelectResult.Size = new System.Drawing.Size(26, 23);
            this.btnSelectResult.TabIndex = 5;
            this.btnSelectResult.Text = "...";
            this.btnSelectResult.UseVisualStyleBackColor = true;
            this.btnSelectResult.Click += new System.EventHandler(this.btnSelectResult_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(79, 98);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 6;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 126);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(116, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Waiting for your input...";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Prod config";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "UAT config";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Diff";
            // 
            // FrmXmlConfigCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 148);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSelectResult);
            this.Controls.Add(this.btnSelectUAT);
            this.Controls.Add(this.btnSelectProd);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tbUAT);
            this.Controls.Add(this.tbProd);
            this.Name = "FrmXmlConfigCompare";
            this.Text = "Xml Config Compare";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbProd;
        private System.Windows.Forms.TextBox tbUAT;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button btnSelectProd;
        private System.Windows.Forms.Button btnSelectUAT;
        private System.Windows.Forms.Button btnSelectResult;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}


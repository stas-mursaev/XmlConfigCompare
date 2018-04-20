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
            this.tbConfig1 = new System.Windows.Forms.TextBox();
            this.tbConfig2 = new System.Windows.Forms.TextBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.btnSelectConfig1 = new System.Windows.Forms.Button();
            this.btnSelectConfig2 = new System.Windows.Forms.Button();
            this.btnSelectResult = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectConfig3 = new System.Windows.Forms.Button();
            this.tbConfig3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbConfig1
            // 
            this.tbConfig1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConfig1.Location = new System.Drawing.Point(79, 12);
            this.tbConfig1.Name = "tbConfig1";
            this.tbConfig1.Size = new System.Drawing.Size(260, 20);
            this.tbConfig1.TabIndex = 0;
            // 
            // tbConfig2
            // 
            this.tbConfig2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConfig2.Location = new System.Drawing.Point(79, 43);
            this.tbConfig2.Name = "tbConfig2";
            this.tbConfig2.Size = new System.Drawing.Size(260, 20);
            this.tbConfig2.TabIndex = 1;
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(79, 101);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(260, 20);
            this.tbResult.TabIndex = 2;
            // 
            // btnSelectConfig1
            // 
            this.btnSelectConfig1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectConfig1.Location = new System.Drawing.Point(345, 12);
            this.btnSelectConfig1.Name = "btnSelectConfig1";
            this.btnSelectConfig1.Size = new System.Drawing.Size(26, 23);
            this.btnSelectConfig1.TabIndex = 3;
            this.btnSelectConfig1.Text = "...";
            this.btnSelectConfig1.UseVisualStyleBackColor = true;
            this.btnSelectConfig1.Click += new System.EventHandler(this.btnSelectConfig1_Click);
            // 
            // btnSelectConfig2
            // 
            this.btnSelectConfig2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectConfig2.Location = new System.Drawing.Point(345, 41);
            this.btnSelectConfig2.Name = "btnSelectConfig2";
            this.btnSelectConfig2.Size = new System.Drawing.Size(26, 23);
            this.btnSelectConfig2.TabIndex = 4;
            this.btnSelectConfig2.Text = "...";
            this.btnSelectConfig2.UseVisualStyleBackColor = true;
            this.btnSelectConfig2.Click += new System.EventHandler(this.btnSelectConfig2_Click);
            // 
            // btnSelectResult
            // 
            this.btnSelectResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectResult.Location = new System.Drawing.Point(345, 99);
            this.btnSelectResult.Name = "btnSelectResult";
            this.btnSelectResult.Size = new System.Drawing.Size(26, 23);
            this.btnSelectResult.TabIndex = 5;
            this.btnSelectResult.Text = "...";
            this.btnSelectResult.UseVisualStyleBackColor = true;
            this.btnSelectResult.Click += new System.EventHandler(this.btnSelectResult_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(79, 127);
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
            this.lblStatus.Location = new System.Drawing.Point(12, 153);
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
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Config 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Config 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Diff";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Config 3";
            this.label4.Visible = false;
            // 
            // btnSelectConfig3
            // 
            this.btnSelectConfig3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectConfig3.Location = new System.Drawing.Point(345, 70);
            this.btnSelectConfig3.Name = "btnSelectConfig3";
            this.btnSelectConfig3.Size = new System.Drawing.Size(26, 23);
            this.btnSelectConfig3.TabIndex = 12;
            this.btnSelectConfig3.Text = "...";
            this.btnSelectConfig3.UseVisualStyleBackColor = true;
            this.btnSelectConfig3.Visible = false;
            this.btnSelectConfig3.Click += new System.EventHandler(this.btnSelectConfig3_Click);
            // 
            // tbConfig3
            // 
            this.tbConfig3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConfig3.Location = new System.Drawing.Point(79, 72);
            this.tbConfig3.Name = "tbConfig3";
            this.tbConfig3.Size = new System.Drawing.Size(260, 20);
            this.tbConfig3.TabIndex = 11;
            this.tbConfig3.Visible = false;
            // 
            // FrmXmlConfigCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 178);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSelectConfig3);
            this.Controls.Add(this.tbConfig3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSelectResult);
            this.Controls.Add(this.btnSelectConfig2);
            this.Controls.Add(this.btnSelectConfig1);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tbConfig2);
            this.Controls.Add(this.tbConfig1);
            this.Name = "FrmXmlConfigCompare";
            this.Text = "Xml Config Compare";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbConfig1;
        private System.Windows.Forms.TextBox tbConfig2;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button btnSelectConfig1;
        private System.Windows.Forms.Button btnSelectConfig2;
        private System.Windows.Forms.Button btnSelectResult;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectConfig3;
        private System.Windows.Forms.TextBox tbConfig3;
    }
}


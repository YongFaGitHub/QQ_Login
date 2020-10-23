namespace QQ_Login
{
    partial class Form2
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
            this.txtverfiycode = new System.Windows.Forms.TextBox();
            this.vefycodpicbox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vefycodpicbox)).BeginInit();
            this.SuspendLayout();
            // 
            // txtverfiycode
            // 
            this.txtverfiycode.Location = new System.Drawing.Point(26, 69);
            this.txtverfiycode.Name = "txtverfiycode";
            this.txtverfiycode.Size = new System.Drawing.Size(151, 20);
            this.txtverfiycode.TabIndex = 3;
            // 
            // vefycodpicbox
            // 
            this.vefycodpicbox.Location = new System.Drawing.Point(37, 12);
            this.vefycodpicbox.Name = "vefycodpicbox";
            this.vefycodpicbox.Size = new System.Drawing.Size(135, 48);
            this.vefycodpicbox.TabIndex = 2;
            this.vefycodpicbox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(66, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 138);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtverfiycode);
            this.Controls.Add(this.vefycodpicbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请输入验证码";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vefycodpicbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox txtverfiycode;
        public System.Windows.Forms.PictureBox vefycodpicbox;
    }
}
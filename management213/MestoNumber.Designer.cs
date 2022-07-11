namespace management213
{
    partial class MestoNumber
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
            this.lbl_number = new System.Windows.Forms.Label();
            this.lbl_RM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_number
            // 
            this.lbl_number.AutoSize = true;
            this.lbl_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 200F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_number.Location = new System.Drawing.Point(413, 310);
            this.lbl_number.Name = "lbl_number";
            this.lbl_number.Size = new System.Drawing.Size(541, 302);
            this.lbl_number.TabIndex = 5;
            this.lbl_number.Text = "№?";
            // 
            // lbl_RM
            // 
            this.lbl_RM.AutoSize = true;
            this.lbl_RM.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_RM.Location = new System.Drawing.Point(177, 157);
            this.lbl_RM.Name = "lbl_RM";
            this.lbl_RM.Size = new System.Drawing.Size(1012, 153);
            this.lbl_RM.TabIndex = 4;
            this.lbl_RM.Text = "Рабочее место";
            // 
            // MestoNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.lbl_number);
            this.Controls.Add(this.lbl_RM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MestoNumber";
            this.Text = "MestoNumber";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MestoNumber_FormClosing);
            this.Load += new System.EventHandler(this.MestoNumber_Load);
            this.Shown += new System.EventHandler(this.MestoNumber_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_number;
        private System.Windows.Forms.Label lbl_RM;
    }
}
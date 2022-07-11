namespace management213
{
    partial class GotoOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotoOut));
            this.btn_NextOrBack = new System.Windows.Forms.Button();
            this.big_place = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.big_passport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.big_born = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.big_FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_outListBig = new System.Windows.Forms.DataGridView();
            this.idDBBig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_VC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_PlaceTo = new System.Windows.Forms.TextBox();
            this.tb_Number = new System.Windows.Forms.TextBox();
            this.btn_Generation = new System.Windows.Forms.Button();
            this.idDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.placeBorn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.born = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FIO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_prizyvniki = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_passportDate = new System.Windows.Forms.TextBox();
            this.cb_SV = new System.Windows.Forms.ComboBox();
            this.cb_FL = new System.Windows.Forms.ComboBox();
            this.cb_gvardia = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_flot = new System.Windows.Forms.CheckBox();
            this.lbl_search = new System.Windows.Forms.Label();
            this.tb_search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_outListBig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_prizyvniki)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_NextOrBack
            // 
            this.btn_NextOrBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.btn_NextOrBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_NextOrBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_NextOrBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_NextOrBack.Location = new System.Drawing.Point(695, 444);
            this.btn_NextOrBack.Name = "btn_NextOrBack";
            this.btn_NextOrBack.Size = new System.Drawing.Size(93, 44);
            this.btn_NextOrBack.TabIndex = 25;
            this.btn_NextOrBack.Text = "Далее";
            this.btn_NextOrBack.UseVisualStyleBackColor = false;
            this.btn_NextOrBack.Click += new System.EventHandler(this.btn_NextOrBack_Click);
            // 
            // big_place
            // 
            this.big_place.HeaderText = "Место рождения";
            this.big_place.Name = "big_place";
            this.big_place.Width = 230;
            // 
            // big_passport
            // 
            this.big_passport.HeaderText = "Паспорт";
            this.big_passport.Name = "big_passport";
            // 
            // big_born
            // 
            this.big_born.HeaderText = "Дата рождения";
            this.big_born.Name = "big_born";
            // 
            // phone
            // 
            this.phone.HeaderText = "Номер телефона";
            this.phone.Name = "phone";
            this.phone.Width = 75;
            // 
            // big_FIO
            // 
            this.big_FIO.HeaderText = "ФИО";
            this.big_FIO.Name = "big_FIO";
            this.big_FIO.Width = 200;
            // 
            // dgv_outListBig
            // 
            this.dgv_outListBig.BackgroundColor = System.Drawing.Color.Azure;
            this.dgv_outListBig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_outListBig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.big_FIO,
            this.phone,
            this.big_born,
            this.big_passport,
            this.big_place,
            this.idDBBig});
            this.dgv_outListBig.Location = new System.Drawing.Point(12, 12);
            this.dgv_outListBig.Name = "dgv_outListBig";
            this.dgv_outListBig.Size = new System.Drawing.Size(776, 426);
            this.dgv_outListBig.TabIndex = 24;
            // 
            // idDBBig
            // 
            this.idDBBig.HeaderText = "id";
            this.idDBBig.Name = "idDBBig";
            this.idDBBig.ReadOnly = true;
            this.idDBBig.Width = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(399, 448);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "В/Ч";
            // 
            // tb_VC
            // 
            this.tb_VC.Location = new System.Drawing.Point(358, 464);
            this.tb_VC.Multiline = true;
            this.tb_VC.Name = "tb_VC";
            this.tb_VC.Size = new System.Drawing.Size(108, 20);
            this.tb_VC.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 448);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Пункт назначения";
            // 
            // tb_PlaceTo
            // 
            this.tb_PlaceTo.Location = new System.Drawing.Point(223, 464);
            this.tb_PlaceTo.Name = "tb_PlaceTo";
            this.tb_PlaceTo.Size = new System.Drawing.Size(108, 20);
            this.tb_PlaceTo.TabIndex = 19;
            this.tb_PlaceTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_PlaceTo_KeyDown);
            // 
            // tb_Number
            // 
            this.tb_Number.Location = new System.Drawing.Point(88, 464);
            this.tb_Number.Name = "tb_Number";
            this.tb_Number.Size = new System.Drawing.Size(108, 20);
            this.tb_Number.TabIndex = 18;
            // 
            // btn_Generation
            // 
            this.btn_Generation.BackColor = System.Drawing.Color.SpringGreen;
            this.btn_Generation.Enabled = false;
            this.btn_Generation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btn_Generation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen;
            this.btn_Generation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Generation.Location = new System.Drawing.Point(695, 502);
            this.btn_Generation.Name = "btn_Generation";
            this.btn_Generation.Size = new System.Drawing.Size(93, 44);
            this.btn_Generation.TabIndex = 17;
            this.btn_Generation.Text = "Сформировать";
            this.btn_Generation.UseVisualStyleBackColor = false;
            this.btn_Generation.Click += new System.EventHandler(this.btn_Generation_Click);
            // 
            // idDB
            // 
            this.idDB.HeaderText = "id";
            this.idDB.Name = "idDB";
            this.idDB.ReadOnly = true;
            this.idDB.Width = 50;
            // 
            // placeBorn
            // 
            this.placeBorn.HeaderText = "Место рождения";
            this.placeBorn.Name = "placeBorn";
            this.placeBorn.Width = 230;
            // 
            // passport
            // 
            this.passport.HeaderText = "Паспорт";
            this.passport.Name = "passport";
            // 
            // born
            // 
            this.born.HeaderText = "Дата рождения";
            this.born.Name = "born";
            this.born.ReadOnly = true;
            // 
            // FIO
            // 
            this.FIO.HeaderText = "ФИО";
            this.FIO.Name = "FIO";
            this.FIO.ReadOnly = true;
            this.FIO.Width = 250;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 448);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Номер команды";
            // 
            // dgv_prizyvniki
            // 
            this.dgv_prizyvniki.AllowUserToAddRows = false;
            this.dgv_prizyvniki.AllowUserToDeleteRows = false;
            this.dgv_prizyvniki.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.dgv_prizyvniki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_prizyvniki.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FIO,
            this.born,
            this.passport,
            this.placeBorn,
            this.idDB});
            this.dgv_prizyvniki.Location = new System.Drawing.Point(12, 12);
            this.dgv_prizyvniki.Name = "dgv_prizyvniki";
            this.dgv_prizyvniki.Size = new System.Drawing.Size(776, 426);
            this.dgv_prizyvniki.TabIndex = 16;
            this.dgv_prizyvniki.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_prizyvniki_CellEndEdit);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(502, 448);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Паспорт выдан";
            // 
            // tb_passportDate
            // 
            this.tb_passportDate.Location = new System.Drawing.Point(490, 464);
            this.tb_passportDate.Multiline = true;
            this.tb_passportDate.Name = "tb_passportDate";
            this.tb_passportDate.Size = new System.Drawing.Size(108, 20);
            this.tb_passportDate.TabIndex = 26;
            // 
            // cb_SV
            // 
            this.cb_SV.FormattingEnabled = true;
            this.cb_SV.Items.AddRange(new object[] {
            "рядовой",
            "ефрейтор",
            "младший сержант",
            "сержант",
            "старший сержант",
            "старшина",
            "прапорщик",
            "старший праворщик",
            "младший лейтенант",
            "лейтенант",
            "старший лейтенант",
            "капитан",
            "майор",
            "подполковник",
            "полковник"});
            this.cb_SV.Location = new System.Drawing.Point(79, 19);
            this.cb_SV.Name = "cb_SV";
            this.cb_SV.Size = new System.Drawing.Size(121, 21);
            this.cb_SV.TabIndex = 28;
            // 
            // cb_FL
            // 
            this.cb_FL.FormattingEnabled = true;
            this.cb_FL.Items.AddRange(new object[] {
            "матрос",
            "старший матрос",
            "старшина 2 статьи",
            "старшина 1 статьи",
            "главный старшина",
            "главный корабельный старшина",
            "мичман",
            "старший мичман",
            "младший лейтенант",
            "лейтенант",
            "старший тейтенант",
            "капитан-лейтенант",
            "капитан 3 ранга",
            "капитан 2 ранга",
            "капитан 1 ранга"});
            this.cb_FL.Location = new System.Drawing.Point(263, 19);
            this.cb_FL.Name = "cb_FL";
            this.cb_FL.Size = new System.Drawing.Size(121, 21);
            this.cb_FL.TabIndex = 29;
            // 
            // cb_gvardia
            // 
            this.cb_gvardia.AutoSize = true;
            this.cb_gvardia.Location = new System.Drawing.Point(6, 21);
            this.cb_gvardia.Name = "cb_gvardia";
            this.cb_gvardia.Size = new System.Drawing.Size(67, 17);
            this.cb_gvardia.TabIndex = 30;
            this.cb_gvardia.Text = "гвардия";
            this.cb_gvardia.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_flot);
            this.groupBox1.Controls.Add(this.cb_gvardia);
            this.groupBox1.Controls.Add(this.cb_FL);
            this.groupBox1.Controls.Add(this.cb_SV);
            this.groupBox1.Location = new System.Drawing.Point(81, 490);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 56);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Звание старшего";
            // 
            // cb_flot
            // 
            this.cb_flot.AutoSize = true;
            this.cb_flot.Location = new System.Drawing.Point(206, 21);
            this.cb_flot.Name = "cb_flot";
            this.cb_flot.Size = new System.Drawing.Size(51, 17);
            this.cb_flot.TabIndex = 31;
            this.cb_flot.Text = "флот";
            this.cb_flot.UseVisualStyleBackColor = true;
            // 
            // lbl_search
            // 
            this.lbl_search.AutoSize = true;
            this.lbl_search.Location = new System.Drawing.Point(525, 494);
            this.lbl_search.Name = "lbl_search";
            this.lbl_search.Size = new System.Drawing.Size(39, 13);
            this.lbl_search.TabIndex = 33;
            this.lbl_search.Text = "Поиск";
            this.lbl_search.Visible = false;
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(490, 510);
            this.tb_search.Multiline = true;
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(108, 20);
            this.tb_search.TabIndex = 32;
            this.tb_search.Visible = false;
            this.tb_search.TextChanged += new System.EventHandler(this.tb_search_TextChanged);
            this.tb_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_search_KeyDown);
            // 
            // GotoOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 558);
            this.Controls.Add(this.lbl_search);
            this.Controls.Add(this.tb_search);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_passportDate);
            this.Controls.Add(this.btn_NextOrBack);
            this.Controls.Add(this.dgv_outListBig);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_VC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_PlaceTo);
            this.Controls.Add(this.tb_Number);
            this.Controls.Add(this.btn_Generation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_prizyvniki);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(816, 597);
            this.MinimumSize = new System.Drawing.Size(816, 597);
            this.Name = "GotoOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подготовка списка";
            this.Load += new System.EventHandler(this.GotoOut_Load);
            this.Shown += new System.EventHandler(this.GotoOut_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_outListBig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_prizyvniki)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_NextOrBack;
        private System.Windows.Forms.DataGridViewTextBoxColumn big_place;
        private System.Windows.Forms.DataGridViewTextBoxColumn big_passport;
        private System.Windows.Forms.DataGridViewTextBoxColumn big_born;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn big_FIO;
        private System.Windows.Forms.DataGridView dgv_outListBig;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDBBig;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_VC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_PlaceTo;
        private System.Windows.Forms.TextBox tb_Number;
        private System.Windows.Forms.Button btn_Generation;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn placeBorn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passport;
        private System.Windows.Forms.DataGridViewTextBoxColumn born;
        private System.Windows.Forms.DataGridViewTextBoxColumn FIO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_prizyvniki;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_passportDate;
        private System.Windows.Forms.ComboBox cb_SV;
        private System.Windows.Forms.ComboBox cb_FL;
        private System.Windows.Forms.CheckBox cb_gvardia;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_flot;
        private System.Windows.Forms.Label lbl_search;
        private System.Windows.Forms.TextBox tb_search;
    }
}
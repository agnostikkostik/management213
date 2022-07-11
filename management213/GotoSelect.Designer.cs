namespace management213
{
    partial class GotoSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotoSelect));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.familiya = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.born = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.komandNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gotoOut = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btn_selectComand = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btn_showDate = new System.Windows.Forms.Button();
            this.btn_create = new System.Windows.Forms.Button();
            this.lbl_search = new System.Windows.Forms.Label();
            this.tb_search = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.familiya,
            this.nameP,
            this.otec,
            this.born,
            this.idDB,
            this.komandNumber,
            this.gotoOut});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 426);
            this.dataGridView1.TabIndex = 11;
            // 
            // familiya
            // 
            this.familiya.HeaderText = "Фамилия";
            this.familiya.Name = "familiya";
            // 
            // nameP
            // 
            this.nameP.HeaderText = "Имя";
            this.nameP.Name = "nameP";
            // 
            // otec
            // 
            this.otec.HeaderText = "Отчество";
            this.otec.Name = "otec";
            // 
            // born
            // 
            this.born.HeaderText = "Дата рождения";
            this.born.Name = "born";
            // 
            // idDB
            // 
            this.idDB.HeaderText = "id";
            this.idDB.Name = "idDB";
            this.idDB.ReadOnly = true;
            this.idDB.Visible = false;
            // 
            // komandNumber
            // 
            this.komandNumber.HeaderText = "Номер команды";
            this.komandNumber.Name = "komandNumber";
            this.komandNumber.ReadOnly = true;
            // 
            // gotoOut
            // 
            this.gotoOut.HeaderText = "Убывает";
            this.gotoOut.Name = "gotoOut";
            // 
            // btn_selectComand
            // 
            this.btn_selectComand.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_selectComand.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_selectComand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_selectComand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_selectComand.Location = new System.Drawing.Point(393, 444);
            this.btn_selectComand.Name = "btn_selectComand";
            this.btn_selectComand.Size = new System.Drawing.Size(93, 44);
            this.btn_selectComand.TabIndex = 16;
            this.btn_selectComand.Text = "Выбрать эту команду";
            this.btn_selectComand.UseVisualStyleBackColor = false;
            this.btn_selectComand.Click += new System.EventHandler(this.btn_selectComand_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(287, 456);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 15;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 456);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(139, 20);
            this.dateTimePicker1.TabIndex = 14;
            this.dateTimePicker1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePicker1_KeyDown);
            // 
            // btn_showDate
            // 
            this.btn_showDate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_showDate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_showDate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_showDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_showDate.Location = new System.Drawing.Point(157, 444);
            this.btn_showDate.Name = "btn_showDate";
            this.btn_showDate.Size = new System.Drawing.Size(93, 44);
            this.btn_showDate.TabIndex = 13;
            this.btn_showDate.Text = "Отобразить за эту дату";
            this.btn_showDate.UseVisualStyleBackColor = false;
            this.btn_showDate.Click += new System.EventHandler(this.btn_showDate_Click);
            // 
            // btn_create
            // 
            this.btn_create.BackColor = System.Drawing.Color.LightCyan;
            this.btn_create.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MediumTurquoise;
            this.btn_create.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumTurquoise;
            this.btn_create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_create.Location = new System.Drawing.Point(695, 444);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(93, 44);
            this.btn_create.TabIndex = 12;
            this.btn_create.Text = "Подготовить для списка";
            this.btn_create.UseVisualStyleBackColor = false;
            this.btn_create.Click += new System.EventHandler(this.btn_create_Click);
            // 
            // lbl_search
            // 
            this.lbl_search.AutoSize = true;
            this.lbl_search.Location = new System.Drawing.Point(578, 443);
            this.lbl_search.Name = "lbl_search";
            this.lbl_search.Size = new System.Drawing.Size(39, 13);
            this.lbl_search.TabIndex = 35;
            this.lbl_search.Text = "Поиск";
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(543, 459);
            this.tb_search.Multiline = true;
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(108, 20);
            this.tb_search.TabIndex = 34;
            this.tb_search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_search_KeyDown);
            // 
            // GotoSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.lbl_search);
            this.Controls.Add(this.tb_search);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_selectComand);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.btn_showDate);
            this.Controls.Add(this.btn_create);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(816, 539);
            this.MinimumSize = new System.Drawing.Size(816, 539);
            this.Name = "GotoSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор людей для команды";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn familiya;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameP;
        private System.Windows.Forms.DataGridViewTextBoxColumn otec;
        private System.Windows.Forms.DataGridViewTextBoxColumn born;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn komandNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn gotoOut;
        private System.Windows.Forms.Button btn_selectComand;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btn_showDate;
        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.Label lbl_search;
        private System.Windows.Forms.TextBox tb_search;
    }
}
namespace management213
{
    partial class ProfotborDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfotborDB));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.familiya = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.otec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.born = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rayon = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.txt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.button6 = new System.Windows.Forms.Button();
            this.btn_testBad = new System.Windows.Forms.Button();
            this.btn_testEnd = new System.Windows.Forms.Button();
            this.btn_testStart = new System.Windows.Forms.Button();
            this.btn_insert = new System.Windows.Forms.Button();
            this.btn_toTXT = new System.Windows.Forms.Button();
            this.btn_default = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.GhostWhite;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.familiya,
            this.nameP,
            this.otec,
            this.born,
            this.rayon,
            this.txt});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 426);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
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
            // rayon
            // 
            this.rayon.HeaderText = "Район";
            this.rayon.Name = "rayon";
            this.rayon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.rayon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // txt
            // 
            this.txt.HeaderText = "Перенесён";
            this.txt.Name = "txt";
            this.txt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.txt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.MediumPurple;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue;
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SlateBlue;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(695, 444);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(93, 44);
            this.button6.TabIndex = 13;
            this.button6.Text = "Управление профотбором";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btn_testBad
            // 
            this.btn_testBad.BackColor = System.Drawing.Color.LightCoral;
            this.btn_testBad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.IndianRed;
            this.btn_testBad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.IndianRed;
            this.btn_testBad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_testBad.Location = new System.Drawing.Point(408, 444);
            this.btn_testBad.Name = "btn_testBad";
            this.btn_testBad.Size = new System.Drawing.Size(93, 44);
            this.btn_testBad.TabIndex = 12;
            this.btn_testBad.Text = "&Двоешник";
            this.btn_testBad.UseVisualStyleBackColor = false;
            this.btn_testBad.Click += new System.EventHandler(this.btn_testBad_Click);
            // 
            // btn_testEnd
            // 
            this.btn_testEnd.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.btn_testEnd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.btn_testEnd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen;
            this.btn_testEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_testEnd.Location = new System.Drawing.Point(309, 444);
            this.btn_testEnd.Name = "btn_testEnd";
            this.btn_testEnd.Size = new System.Drawing.Size(93, 44);
            this.btn_testEnd.TabIndex = 11;
            this.btn_testEnd.Text = "Про&шёл тест";
            this.btn_testEnd.UseVisualStyleBackColor = false;
            this.btn_testEnd.Click += new System.EventHandler(this.btn_testEnd_Click);
            // 
            // btn_testStart
            // 
            this.btn_testStart.BackColor = System.Drawing.Color.Goldenrod;
            this.btn_testStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Peru;
            this.btn_testStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Peru;
            this.btn_testStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_testStart.Location = new System.Drawing.Point(210, 444);
            this.btn_testStart.Name = "btn_testStart";
            this.btn_testStart.Size = new System.Drawing.Size(93, 44);
            this.btn_testStart.TabIndex = 10;
            this.btn_testStart.Text = "Про&ходит тест";
            this.btn_testStart.UseVisualStyleBackColor = false;
            this.btn_testStart.Click += new System.EventHandler(this.btn_testStart_Click);
            // 
            // btn_insert
            // 
            this.btn_insert.BackColor = System.Drawing.Color.Cornsilk;
            this.btn_insert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.NavajoWhite;
            this.btn_insert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.NavajoWhite;
            this.btn_insert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_insert.Location = new System.Drawing.Point(111, 444);
            this.btn_insert.Name = "btn_insert";
            this.btn_insert.Size = new System.Drawing.Size(93, 44);
            this.btn_insert.TabIndex = 9;
            this.btn_insert.Text = "&Вставить из документа";
            this.btn_insert.UseVisualStyleBackColor = false;
            this.btn_insert.Click += new System.EventHandler(this.btn_insert_Click);
            // 
            // btn_toTXT
            // 
            this.btn_toTXT.BackColor = System.Drawing.Color.Cyan;
            this.btn_toTXT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.btn_toTXT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightCyan;
            this.btn_toTXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_toTXT.Location = new System.Drawing.Point(12, 444);
            this.btn_toTXT.Name = "btn_toTXT";
            this.btn_toTXT.Size = new System.Drawing.Size(93, 44);
            this.btn_toTXT.TabIndex = 8;
            this.btn_toTXT.Text = "Перенести в &файл";
            this.btn_toTXT.UseVisualStyleBackColor = false;
            this.btn_toTXT.Click += new System.EventHandler(this.btn_toTXT_Click);
            // 
            // btn_default
            // 
            this.btn_default.BackColor = System.Drawing.Color.Cyan;
            this.btn_default.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.btn_default.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightCyan;
            this.btn_default.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_default.Location = new System.Drawing.Point(507, 444);
            this.btn_default.Name = "btn_default";
            this.btn_default.Size = new System.Drawing.Size(93, 44);
            this.btn_default.TabIndex = 14;
            this.btn_default.Text = "&Исходное состояние";
            this.btn_default.UseVisualStyleBackColor = false;
            this.btn_default.Click += new System.EventHandler(this.btn_default_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(794, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 169);
            this.label1.TabIndex = 15;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // ProfotborDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_default);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btn_testBad);
            this.Controls.Add(this.btn_testEnd);
            this.Controls.Add(this.btn_testStart);
            this.Controls.Add(this.btn_insert);
            this.Controls.Add(this.btn_toTXT);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(979, 539);
            this.Name = "ProfotborDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "База призывников";
            this.Load += new System.EventHandler(this.ProfotborDB_Load);
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
        private System.Windows.Forms.DataGridViewComboBoxColumn rayon;
        private System.Windows.Forms.DataGridViewCheckBoxColumn txt;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btn_testBad;
        private System.Windows.Forms.Button btn_testEnd;
        private System.Windows.Forms.Button btn_testStart;
        private System.Windows.Forms.Button btn_insert;
        private System.Windows.Forms.Button btn_toTXT;
        private System.Windows.Forms.Button btn_default;
        private System.Windows.Forms.Label label1;
    }
}
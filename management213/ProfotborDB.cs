using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace management213
{
    public partial class ProfotborDB : Form
    {
        public ProfotborDB()
        {
            InitializeComponent();
        }

        Main owner;
        static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.168.168\docs\others\@Прочее\management 4\db.mdb";

        private void btn_toTXT_Click(object sender, EventArgs e)
        {
            string dataText;
            List<int> numRows = new List<int>();
            bool error = false;

            StreamWriter streamWriter = new StreamWriter(@"\\192.168.168.168\docs\others\exportToOtbor.txt", false, Encoding.Default);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (((dataGridView1.Rows[i].Cells["txt"].Value == null) || (Convert.ToBoolean(dataGridView1.Rows[i].Cells["txt"].Value) == false)) &&
                    (dataGridView1.Rows[i].Cells["familiya"].Value != null))
                {
                    try
                    {
                        numRows.Add(i);
                        dataText = "";
                        dataText += dataGridView1.Rows[i].Cells["familiya"].Value.ToString() + ";";
                        dataText += dataGridView1.Rows[i].Cells["nameP"].Value.ToString() + ";";
                        dataText += dataGridView1.Rows[i].Cells["otec"].Value.ToString() + ";";
                        dataText += dataGridView1.Rows[i].Cells["born"].Value.ToString() + ";";
                        dataText += dataGridView1.Rows[i].Cells["rayon"].Value.ToString() + ";";

                        streamWriter.WriteLine(dataText);

                        dataGridView1.Rows[i].Cells["txt"].Value = true;
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Cyan;
                    }
                    catch
                    {
                        error = true;
                        MessageBox.Show("В строке " + (i + 1).ToString() + " одна из ячеек не заполнена");
                        for (int j = 0; j < numRows.Count; j++)
                        {
                            dataGridView1.Rows[numRows[j]].Cells["txt"].Value = false;
                            dataGridView1.Rows[numRows[j]].DefaultCellStyle.BackColor = Color.Empty;
                        }
                    }
                }
            }
            if (error == false)
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    for (int i = 0; i < numRows.Count; i++)
                    {
                        OleDbCommand command = new OleDbCommand("INSERT INTO prizyvniki" +
                        "(familiyaPrizyvnik, namePrizyvnik, otecPrizyvnik, bornPrizyvnik, rayonPrizyvnik, pribylPrizyvnik) VALUES(" +
                        "'" + dataGridView1.Rows[numRows[i]].Cells["familiya"].Value.ToString() + "', " +
                        "'" + dataGridView1.Rows[numRows[i]].Cells["nameP"].Value.ToString() + "', " +
                        "'" + dataGridView1.Rows[numRows[i]].Cells["otec"].Value.ToString() + "', " +
                        "'" + dataGridView1.Rows[numRows[i]].Cells["born"].Value.ToString() + "', " +
                        "'" + dataGridView1.Rows[numRows[i]].Cells["rayon"].Value.ToString() + "', " +
                        "'" + DateTime.Today.ToString() + "')", conn);

                        command.ExecuteReader();
                    }
                }
            }
            numRows.Clear();

            streamWriter.Close();
        }

        private void ProfotborDB_Load(object sender, EventArgs e)
        {
            owner = (Main)this.Owner;
            DataGridViewComboBoxColumn dgvCmd = (DataGridViewComboBoxColumn)dataGridView1.Columns["rayon"];
            dgvCmd.HeaderText = "Район";
            dgvCmd.Items.Add("Адмиралтейский и Кировский");
            dgvCmd.Items.Add("Василеостровский");
            dgvCmd.Items.Add("Выборгский");
            dgvCmd.Items.Add("Калининский");
            dgvCmd.Items.Add("Колпинский");
            dgvCmd.Items.Add("Пушкинский");
            dgvCmd.Items.Add("Красногвардейский");
            dgvCmd.Items.Add("Красносельский");
            dgvCmd.Items.Add("Кронштадтский и Курортный");
            dgvCmd.Items.Add("Московский");
            dgvCmd.Items.Add("Невский");
            dgvCmd.Items.Add("Петроградский");
            dgvCmd.Items.Add("Петродворцовый");
            dgvCmd.Items.Add("Приморский");
            dgvCmd.Items.Add("Фрунзенский");
            dgvCmd.Items.Add("Центральный");
            dgvCmd.Width = 170;
        }

        private void btn_default_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.Cyan;
            }
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();
            string[] lines = s.Replace("\n", "").Split('\r');

            string[] fields;
            int row = dataGridView1.Rows.Count - 1;
            int col = 0;

            foreach (string line in lines)
            {
                if (line != "")
                {
                    dataGridView1.Rows.Add(1);
                    fields = line.Split('\t');
                    foreach (string field in fields)
                    {
                        dataGridView1[col, row].Value = field;
                        col++;
                    }
                    row++;
                    col = 0;
                }
            }
        }

        private void btn_testStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.Goldenrod;
            }
        }

        private void btn_testEnd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.MediumSpringGreen;
            }
        }

        private void btn_testBad_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.LightCoral;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProfotborRMI profotborRMI = new ProfotborRMI();
            profotborRMI.Owner = this;
            profotborRMI.Show();
        }

        string tempString = "";
        ComboBox cb;
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            cb = e.Control as ComboBox;
            tempString = "";

            if (cb != null)
            {
                cb.KeyPress -= new KeyPressEventHandler(cb_KeyPress);
                cb.KeyPress += new KeyPressEventHandler(cb_KeyPress);
            }
        }

        void cb_KeyPress(object sender, KeyPressEventArgs e)
        {
            tempString += e.KeyChar.ToString();
            if (tempString.IndexOf("7") != -1)
                cb.SelectedIndex = cb.FindString("Василеостровский");
            if (tempString.IndexOf("8") != -1)
                cb.SelectedIndex = cb.FindString("Выборгский");


            if (tempString.IndexOf("9") != -1)
                cb.SelectedIndex = cb.FindString("Калининский");
            if (tempString.IndexOf("0") != -1)
                cb.SelectedIndex = cb.FindString("Колпинский");
            if (tempString.IndexOf("1") != -1)
                cb.SelectedIndex = cb.FindString("Красногвардейский");
            if (tempString.IndexOf("2") != -1)
                cb.SelectedIndex = cb.FindString("Красносельский");
            if (tempString.IndexOf("3") != -1)
                cb.SelectedIndex = cb.FindString("Кронштадтский и Курортный");

            if (tempString.IndexOf("4") != -1)
                cb.SelectedIndex = cb.FindString("Петроградский");
            if (tempString.IndexOf("5") != -1)
                cb.SelectedIndex = cb.FindString("Петродворцовый");
            if (tempString.IndexOf("6") != -1)
                cb.SelectedIndex = cb.FindString("Приморский");
            if (tempString.IndexOf("=") != -1)
                cb.SelectedIndex = cb.FindString("Пушкинский");

        }
    }
}

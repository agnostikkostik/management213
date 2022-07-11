using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace management213
{
    public partial class GotoSelect : Form
    {
        public GotoSelect()
        {
            InitializeComponent();
        }

        static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.168.168\docs\others\@Прочее\management 4\db.mdb";

        private void btn_showDate_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT familiyaPrizyvnik, namePrizyvnik, otecPrizyvnik, bornPrizyvnik, id, komandToNumber FROM prizyvniki WHERE pribylPrizyvnik=#" +
                    dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + dateTimePicker1.Value.Day.ToString() + "#", conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                }
                reader.Close();
            }

            DataTable dataTable = new DataTable();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dataTable.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = dataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dataTable.Rows.Add(dRow);
            }
            
            DataView dataView = new DataView(dataTable);
            dataView.Sort = "familiya ASC, nameP ASC, otec ASC, born ASC";
            dataGridView1.Rows.Clear();
            for (int i = 0; i < dataView.Count; i++)
            {
                dataGridView1.Rows.Add(dataView[i][0], dataView[i][1], dataView[i][2], dataView[i][3], dataView[i][4], dataView[i][5]);
            }
        }

        private void btn_selectComand_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["komandNumber"].Value != null)
                {
                    if (dataGridView1.Rows[i].Cells["komandNumber"].Value.ToString() == textBox1.Text)
                    {
                        dataGridView1.Rows[i].Cells["gotoOut"].Value = true;
                    }
                }
            }
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            bool error = false;

            GotoOut gotoOutForm = new GotoOut();
            gotoOutForm.Owner = this;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((dataGridView1.Rows[i].Cells["gotoOut"].Value != null) &&
                    ((bool)dataGridView1.Rows[i].Cells["gotoOut"].Value != false) &&
                    (dataGridView1.Rows[i].Cells["familiya"].Value != null))
                {
                    try
                    {
                        gotoOutForm.idGo += dataGridView1.Rows[i].Cells["idDB"].Value.ToString() + ";";
                    }
                    catch
                    {
                        MessageBox.Show("В строке " + (i + 1).ToString() + " одна из ячеек не заполнена");
                        error = true;
                        gotoOutForm.Close();
                    }
                }
            }
            if (error == false)
            {
                gotoOutForm.ShowDialog();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_selectComand.PerformClick();
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_showDate.PerformClick();
            }
        }

        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
            }

            if (tb_search.Text != "")
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1["familiya", i].Value.ToString().ToUpper().IndexOf(tb_search.Text.ToUpper()) == 0)
                    {
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    }
                }
            }
        }
    }
}

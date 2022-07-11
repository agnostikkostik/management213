using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace management213
{
    public partial class GotoOut : Form
    {
        public GotoOut()
        {
            InitializeComponent();
        }

        bool grand = true;

        public string idGo = "";
        static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\192.168.168.168\docs\others\@Прочее\management 4\db.mdb";

        Word._Application application;
        Word._Document document;
        Object missingObj = Missing.Value;
        Object trueObj = true;
        Object falseObj = false;

        private void btn_NextOrBack_Click(object sender, EventArgs e)
        {
            if (grand)
            {
                grand = false;
                dgv_prizyvniki.Visible = true;
                dgv_outListBig.Visible = false;
                lbl_search.Visible = true;
                tb_search.Visible = true;
                btn_NextOrBack.Text = "Назад";
                btn_Generation.Enabled = true;
            }
            else
            {
                grand = true;
                dgv_prizyvniki.Visible = false;
                dgv_outListBig.Visible = true;
                lbl_search.Visible = false;
                tb_search.Visible = false;
                btn_NextOrBack.Text = "Далее";
                btn_Generation.Enabled = false;
            }
        }

        private void btn_Generation_Click(object sender, EventArgs e)
        {
            if ((tb_Number.Text == "") || (tb_passportDate.Text == "") || (tb_PlaceTo.Text == "") || (tb_VC.Text == ""))
            {
                MessageBox.Show("Заполните все необходимые поля");
                return;
            }

            if (MessageBox.Show("Проставить всем пустым город?", "Запрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < dgv_prizyvniki.Rows.Count; i++)
            {
                if ((dgv_prizyvniki[3, i].Value == null) || (dgv_prizyvniki[3, i].Value.ToString() == ""))
                {
                    dgv_prizyvniki[3, i].Value = "г. Санкт-Петербург";
                }
            }

            if (MessageBox.Show("Подтвердите правильность места рождения.", "Запрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            application = new Word.Application();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                for (int i = 0; i < dgv_prizyvniki.RowCount; i++)
                {
                    OleDbCommand cmd = new OleDbCommand("UPDATE prizyvniki SET komandToPlace='" + tb_PlaceTo.Text + "' WHERE id=" + dgv_prizyvniki.Rows[i].Cells["idDB"].Value.ToString(), conn);
                    OleDbDataReader reader = cmd.ExecuteReader();

                    cmd = new OleDbCommand("UPDATE prizyvniki SET komandToNumber='" + tb_Number.Text + "' WHERE id=" + dgv_prizyvniki.Rows[i].Cells["idDB"].Value.ToString(), conn);
                    reader = cmd.ExecuteReader();
                }
            }

            for (int i = 0; i < dgv_outListBig.RowCount - 1; i++)
            {
                if (dgv_outListBig["idDBBig", i].Value == null)
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();

                        string phoneDB = "";
                        if (dgv_outListBig["phone", i].Value != null)
                        {
                            phoneDB = dgv_outListBig["phone", i].Value.ToString();
                        }

                        OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM bigOut WHERE " +
                        "FIO = '" + dgv_outListBig["big_FIO", i].Value.ToString() + "' AND " +
                        "bornDate = '" + dgv_outListBig["big_born", i].Value.ToString() + "' AND " +
                        "passport = '" + dgv_outListBig["big_passport", i].Value.ToString() + "' AND " +
                        "bornPlace = '" + dgv_outListBig["big_place", i].Value.ToString() + "' AND " +
                        "phone = '" + phoneDB + "' AND " +
                        "komand = '" + tb_Number.Text.ToString() + "' AND " +
                        "place = '" + tb_PlaceTo.Text.ToString() + "'", conn);

                        bool insertInTable = true;
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader[0].ToString() != "0")
                            {
                                insertInTable = false;
                            }
                        }
                        reader.Close();

                        if (insertInTable)
                        {
                            command = new OleDbCommand("INSERT INTO bigOut" +
                                "(FIO, bornDate, passport, bornPlace, phone, komand, place) VALUES(" +
                                "'" + dgv_outListBig["big_FIO", i].Value.ToString() + "', " +
                                "'" + dgv_outListBig["big_born", i].Value.ToString() + "', " +
                                "'" + dgv_outListBig["big_passport", i].Value.ToString() + "', " +
                                "'" + dgv_outListBig["big_place", i].Value.ToString() + "', " +
                                "'" + phoneDB + "', " +
                                "'" + tb_Number.Text.ToString() + "', " +
                                "'" + tb_PlaceTo.Text.ToString() + "')", conn);

                            command.ExecuteReader();
                        }
                    }
                }
            }

            Directory.CreateDirectory(@"\\192.168.168.168\docs\Копач Анатолий Семенович\2022 весна\" + tb_PlaceTo.Text + " " + tb_Number.Text);
            try
            {
                document = application.Documents.Add(ref missingObj, ref missingObj, ref missingObj, ref missingObj);
            }
            catch (Exception ex)
            {
                document.Close(ref falseObj, ref missingObj, ref missingObj);
                application.Quit(ref missingObj, ref missingObj, ref missingObj);
                document = null;
                application = null;
            }

            application.Visible = true;

            document.Application.Selection.PageSetup.LeftMargin = document.Application.CentimetersToPoints(1.25f);
            document.Application.Selection.PageSetup.RightMargin = document.Application.CentimetersToPoints(1.27f);
            document.Application.Selection.PageSetup.TopMargin = document.Application.CentimetersToPoints(1.25f);
            document.Application.Selection.PageSetup.BottomMargin = document.Application.CentimetersToPoints(0.25f);

            #region шапка
            Word.Paragraph paragraph;
            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "СПИСОК";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 16;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "организованной группы пассажиров,";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 16;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "следующих от ст. Санкт-Петербург до ст. " + tb_PlaceTo.Text;
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 16;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 16;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();
            #endregion

            #region таблица
            object endFlag = "\\endofdoc";
            Word.Range range = document.Bookmarks[ref endFlag].Range;
            Word.Table table;
            table = document.Tables.Add(range, 1, 7, Missing.Value, Missing.Value);
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            table.LeftPadding = document.Application.CentimetersToPoints(0.1f);
            table.RightPadding = document.Application.CentimetersToPoints(0.1f);
            table.Cell(1, 1).Range.Text = "№ п/п";
            table.Cell(1, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 2).Range.Text = "Ф.И.О. (мобильный телефон старшего группы)";
            table.Cell(1, 2).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 3).Range.Text = "Паспорт\r\n(воен.билет)";
            table.Cell(1, 3).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 4).Range.Text = "Дата рождения";
            table.Cell(1, 4).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 5).Range.Text = "Место рождения";
            table.Cell(1, 5).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 6).Range.Text = "Пол";
            table.Cell(1, 6).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Cell(1, 7).Range.Text = "Гражданство";
            table.Cell(1, 7).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Rows[1].Range.Font.Size = 12;
            table.Rows[1].Range.Font.Kerning = 12;
            table.Columns[1].SetWidth(document.Application.CentimetersToPoints(0.85f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[2].SetWidth(document.Application.CentimetersToPoints(4.46f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[3].SetWidth(document.Application.CentimetersToPoints(3.24f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[4].SetWidth(document.Application.CentimetersToPoints(2.46f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[5].SetWidth(document.Application.CentimetersToPoints(3.93f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[6].SetWidth(document.Application.CentimetersToPoints(1.15f), Word.WdRulerStyle.wdAdjustNone);
            table.Columns[7].SetWidth(document.Application.CentimetersToPoints(2.72f), Word.WdRulerStyle.wdAdjustNone);

            int rowCount = 1;

            for (int i = 0; i < dgv_outListBig.Rows.Count; i++)
            {
                if (dgv_outListBig[0, i].Value != null)
                {
                    table.Rows.Add(Missing.Value);
                    rowCount++;
                    table.Cell(rowCount, 1).Range.Text = (rowCount - 1).ToString();
                    table.Cell(rowCount, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    string namePhone = "";
                    if (dgv_outListBig[0, i].Value != null)
                    {
                        namePhone = dgv_outListBig[0, i].Value.ToString();
                        if (dgv_outListBig[1, i].Value != null)
                        {
                            if (dgv_outListBig[1, i].Value.ToString() != "")
                            {
                                namePhone += "\n";
                                namePhone += dgv_outListBig[1, i].Value != null ? dgv_outListBig[1, i].Value.ToString() : "";
                            }
                        }
                    }

                    table.Cell(rowCount, 2).Range.Text = namePhone;
                    table.Cell(rowCount, 2).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    table.Cell(rowCount, 3).Range.Text = dgv_outListBig[3, i].Value != null ? dgv_outListBig[3, i].Value.ToString() : "";
                    table.Cell(rowCount, 3).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    table.Cell(rowCount, 4).Range.Text = dgv_outListBig[2, i].Value != null ? dgv_outListBig[2, i].Value.ToString() + " г." : "";
                    table.Cell(rowCount, 4).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    table.Cell(rowCount, 5).Range.Text = dgv_outListBig[4, i].Value != null ? dgv_outListBig[4, i].Value.ToString() : "";
                    table.Cell(rowCount, 5).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    table.Cell(rowCount, 6).Range.Text = "муж.";
                    table.Cell(rowCount, 6).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    table.Cell(rowCount, 7).Range.Text = "российское";
                    table.Cell(rowCount, 7).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                }
            }

            for (int i = 0; i < dgv_prizyvniki.Rows.Count; i++)
            {
                if (dgv_prizyvniki[0, i].Value != null)
                {
                    table.Rows.Add(Missing.Value);
                    rowCount++;
                    table.Cell(rowCount, 1).Range.Text = (rowCount - 1).ToString();
                    table.Cell(rowCount, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 2).Range.Text = dgv_prizyvniki[0, i].Value.ToString();
                    table.Cell(rowCount, 2).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 3).Range.Text = dgv_prizyvniki[2, i].Value.ToString();
                    table.Cell(rowCount, 3).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 4).Range.Text = dgv_prizyvniki[1, i].Value.ToString() + " г.";
                    table.Cell(rowCount, 4).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 5).Range.Text = dgv_prizyvniki[3, i].Value.ToString();
                    table.Cell(rowCount, 5).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 6).Range.Text = "муж.";
                    table.Cell(rowCount, 6).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(rowCount, 7).Range.Text = "российское";
                    table.Cell(rowCount, 7).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                }
            }

            #endregion

            #region подпись
            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "ВОЕННЫЙ КОМИССАР ГОРОДА САНКТ-ПЕТЕРБУРГА";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "полковник";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "С.КАЧКОВСКИЙ";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "НАЧАЛЬНИК ФИНАНСОВО-ЭКОНОМИЧЕСКОГО ОТДЕЛЕНИЯ";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            paragraph.Range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            paragraph.Range.Text = "Д.Л.ЛАДЗИН";
            paragraph.LineUnitAfter = (float)0;
            paragraph.SpaceAfter = (float)0;
            paragraph.LineUnitBefore = (float)0;
            paragraph.SpaceBefore = (float)0;
            paragraph.LineSpacingRule = (float)0;
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 12;
            paragraph.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
            paragraph.Range.InsertParagraphAfter();
            #endregion

            document.SaveAs(@"\\192.168.168.168\docs\Копач Анатолий Семенович\2022 весна\" + tb_PlaceTo.Text + " " + tb_Number.Text + @"\СПИСОК.docx");

            #region заявка

            document = application.Documents.Add(@"\\192.168.168.168\docs\Копач Анатолий Семенович\2022 весна\Шаблоны\ЖД шаблон\Заявка.docx", ref missingObj, ref missingObj, ref missingObj);

            string[] months = { "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа",
                "сентября", "октября", "ноября", "декабря"};


            string[] zvanieSV = { "ряд.", "ефр.", "мл. с-т", "с-т", "ст. с-т", "ст-на", "пр-к", "ст. пр-к", "мл. л-.", "л-т", "ст. л-т", "к-н", "м-р",
                "п/п-к", "п/к"};

            string[] zvanieFL = { "мат.", "ст. мат.", "ст-на 2 ст.", "ст-на 1 ст.", "гл. ст-на", "гл. кор. ст-на", "м-н", "ст. м-н", "мл. л-.", "л-т",
                "ст. л-т", "кап. л-т", "кап. 3 р.", "кап. 2 р.", "кап. 1 р."};

            string zvanie = "";
            if (cb_gvardia.Checked)
                zvanie += "гв. ";
            if (cb_flot.Checked == false)
                zvanie += zvanieSV[cb_SV.SelectedIndex];
            else
                zvanie += zvanieFL[cb_FL.SelectedIndex];

            Word.Range bookmarkRange = document.Bookmarks["date_day"].Range;
            bookmarkRange.Text = DateTime.Now.Day.ToString();

            bookmarkRange = document.Bookmarks["date_month"].Range;
            bookmarkRange.Text = months[DateTime.Now.Month - 1];

            bookmarkRange = document.Bookmarks["date_year"].Range;
            bookmarkRange.Text = DateTime.Now.Year.ToString();

            bookmarkRange = document.Bookmarks["fioPassBig"].Range;
            bookmarkRange.Text = dgv_outListBig["big_FIO", 0].Value.ToString() + " " + dgv_outListBig["big_passport", 0].Value.ToString();

            bookmarkRange = document.Bookmarks["fioPassBig2"].Range;
            bookmarkRange.Text = dgv_outListBig["big_FIO", 0].Value.ToString() + " " + dgv_outListBig["big_passport", 0].Value.ToString();

            bookmarkRange = document.Bookmarks["numberTo"].Range;
            bookmarkRange.Text = tb_Number.Text;

            bookmarkRange = document.Bookmarks["phoneNumber"].Range;
            bookmarkRange.Text = dgv_outListBig["phone", 0].Value.ToString();

            bookmarkRange = document.Bookmarks["placeTo"].Range;
            bookmarkRange.Text = tb_PlaceTo.Text;

            bookmarkRange = document.Bookmarks["placeTo2"].Range;
            bookmarkRange.Text = tb_PlaceTo.Text;

            bookmarkRange = document.Bookmarks["vch"].Range;
            bookmarkRange.Text = tb_VC.Text;

            bookmarkRange = document.Bookmarks["countP1"].Range;
            bookmarkRange.Text = (rowCount - 1).ToString();

            bookmarkRange = document.Bookmarks["countP2"].Range;
            bookmarkRange.Text = (rowCount - 1).ToString();

            bookmarkRange = document.Bookmarks["countPr"].Range;
            bookmarkRange.Text = dgv_prizyvniki.Rows.Count.ToString();

            bookmarkRange = document.Bookmarks["countOf"].Range;
            bookmarkRange.Text = (dgv_outListBig.Rows.Count - 1).ToString();

            bookmarkRange = document.Bookmarks["passDate"].Range;
            bookmarkRange.Text = tb_passportDate.Text;

            bookmarkRange = document.Bookmarks["passDate2"].Range;
            bookmarkRange.Text = tb_passportDate.Text;

            bookmarkRange = document.Bookmarks["zvanie"].Range;
            bookmarkRange.Text = zvanie;

            bookmarkRange = document.Bookmarks["zvanie2"].Range;
            bookmarkRange.Text = zvanie;
            //

            document.SaveAs(@"\\192.168.168.168\docs\Копач Анатолий Семенович\2022 весна\" + tb_PlaceTo.Text + " " + tb_Number.Text + @"\ЗАЯВКА.docx");
            #endregion
        }

        private void GotoOut_Load(object sender, EventArgs e)
        {

        }

        private void GotoOut_Shown(object sender, EventArgs e)
        {
            string[] idArray = idGo.Split(';');
            foreach (string id in idArray)
            {
                if (id != "")
                {
                    using (OleDbConnection conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand("SELECT familiyaPrizyvnik, namePrizyvnik, otecPrizyvnik, bornPrizyvnik, passportPrizyvnik, placeBornPrizyvnik " +
                            "FROM prizyvniki WHERE id=" + id.ToString(), conn);
                        OleDbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            dgv_prizyvniki.Rows.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), id.ToString());
                        }
                        reader.Close();
                    }
                }
            }
        }

        private void tb_PlaceTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand("SELECT FIO, phone, bornDate, passport, bornPlace, id " +
                        "FROM bigOut WHERE place='" + tb_PlaceTo.Text.ToString() + "'", conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dgv_outListBig.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
                    }
                    reader.Close();
                }
            }
        }

        private void dgv_prizyvniki_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string tempID = dgv_prizyvniki.Rows[e.RowIndex].Cells["idDB"].Value.ToString();

                if (e.ColumnIndex == 2)
                {
                    string passport = dgv_prizyvniki.Rows[e.RowIndex].Cells["passport"].Value == null ? "" : dgv_prizyvniki.Rows[e.RowIndex].Cells["passport"].Value.ToString();
                    OleDbCommand cmd = new OleDbCommand("UPDATE prizyvniki SET passportPrizyvnik='" + passport + "' WHERE id=" + tempID, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                }
                if (e.ColumnIndex == 3)
                {
                    string place = dgv_prizyvniki.Rows[e.RowIndex].Cells["placeBorn"].Value == null ? "" : dgv_prizyvniki.Rows[e.RowIndex].Cells["placeBorn"].Value.ToString();
                    OleDbCommand cmd = new OleDbCommand("UPDATE prizyvniki SET placeBornPrizyvnik='" + place + "' WHERE id=" + tempID, conn);
                    OleDbDataReader reader = cmd.ExecuteReader();
                }
            }
        }

        private void tb_search_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < dgv_prizyvniki.RowCount; i++)
            {
                dgv_prizyvniki.Rows[i].Selected = false;
            }

            if (tb_search.Text != "")
            {
                for (int i = 0; i < dgv_prizyvniki.RowCount; i++)
                {
                    if (dgv_prizyvniki["FIO", i].Value.ToString().ToUpper().IndexOf(tb_search.Text.ToUpper()) == 0)
                    {
                        dgv_prizyvniki.Rows[i].Selected = true;
                        dgv_prizyvniki.CurrentCell = dgv_prizyvniki.Rows[i].Cells[0];
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Zadanie3
{
    public partial class Form3 : Form
    {
        private bool newRowAdding = false;
        private DataSet dataSet = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlConnection sqlConnection = null;
        public Form3()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Позиции", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Позиции");

                dataGridView1.DataSource = dataSet.Tables["Позиции"];


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[8, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Позиции"].Clear();

                sqlDataAdapter.Fill(dataSet, "Позиции");

                dataGridView1.DataSource = dataSet.Tables["Позиции"];


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[8, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DB1"].ConnectionString);
            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            sqlDataAdapter.Update(dataSet, "Позиции");
                        }
                    }
                    else if (task == "Insert")
                    {
                        if (MessageBox.Show("Ввести данные?", "Ввод", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = dataGridView1.Rows.Count - 2;
                            DataRow row = dataSet.Tables["Позиции"].NewRow();

                            row["Id заказа"] = dataGridView1.Rows[rowIndex].Cells["Id заказа"].Value;
                            row["Целевые характеристики материала"] = dataGridView1.Rows[rowIndex].Cells["Целевые характеристики материала"].Value;
                            row["Марка стали"] = dataGridView1.Rows[rowIndex].Cells["Марка стали"].Value;
                            row["Диаметр"] = dataGridView1.Rows[rowIndex].Cells["Диаметр"].Value;
                            row["Стенка"] = dataGridView1.Rows[rowIndex].Cells["Стенка"].Value;
                            row["Объём позиции заказа"] = dataGridView1.Rows[rowIndex].Cells["Объём позиции заказа"].Value;
                            row["Еденица измерения"] = dataGridView1.Rows[rowIndex].Cells["Еденица измерения"].Value;
                            row["Статус"] = dataGridView1.Rows[rowIndex].Cells["Статус"].Value;

                            dataSet.Tables["Позиции"].Rows.Add(row);

                            dataSet.Tables["Позиции"].Rows.RemoveAt(dataSet.Tables["Позиции"].Rows.Count - 1);

                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                            dataGridView1.Rows[e.RowIndex].Cells[8].Value = "Delete";

                            sqlDataAdapter.Update(dataSet, "Позиции");

                            newRowAdding = false;
                        }

                    }
                    else if (task == "Update")
                    {
                        if (MessageBox.Show("Обновить данные?", "Обновление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int r = e.RowIndex;

                            dataSet.Tables["Позиции"].Rows[r]["Id заказа"] = dataGridView1.Rows[r].Cells["Id заказа"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Целевые характеристики материала"] = dataGridView1.Rows[r].Cells["Целевые характеристики материала"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Марка стали"] = dataGridView1.Rows[r].Cells["Марка стали"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Диаметр"] = dataGridView1.Rows[r].Cells["Диаметр"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Стенка"] = dataGridView1.Rows[r].Cells["Стенка"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Объём позиции заказа"] = dataGridView1.Rows[r].Cells["Объём позиции заказа"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Еденица измерения"] = dataGridView1.Rows[r].Cells["Еденица измерения"].Value;
                            dataSet.Tables["Позиции"].Rows[r]["Статус"] = dataGridView1.Rows[r].Cells["Статус"].Value;

                            sqlDataAdapter.Update(dataSet, "Позиции");

                            dataGridView1.Rows[e.RowIndex].Cells[8].Value = "Delete";
                        }

                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"[Целевые характеристики материала] LIKE '%" + textBox1.Text + "%'";
        }
    }
}

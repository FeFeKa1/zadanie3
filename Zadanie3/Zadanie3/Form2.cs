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
    public partial class Form2 : Form
    {
        private bool newRowAdding = false;
        private DataSet dataSet = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlConnection sqlConnection = null;

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Delete] FROM Заголовок", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Заголовок");

                dataGridView1.DataSource = dataSet.Tables["Заголовок"];


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkCell;
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
                dataSet.Tables["Заголовок"].Clear();

                sqlDataAdapter.Fill(dataSet, "Заголовок");

                dataGridView1.DataSource = dataSet.Tables["Заголовок"];


                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[5, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
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
                if (e.ColumnIndex == 5)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            sqlDataAdapter.Update(dataSet, "Заголовок");
                        }
                    }
                    else if (task == "Insert")
                    {
                        if (MessageBox.Show("Ввести данные?", "Ввод", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = dataGridView1.Rows.Count - 2;
                            DataRow row = dataSet.Tables["Заголовок"].NewRow();

                            row["Id заказа"] = dataGridView1.Rows[rowIndex].Cells["Id заказа"].Value;
                            row["Цех производитель"] = dataGridView1.Rows[rowIndex].Cells["Цех производитель"].Value;
                            row["Дата начала"] = dataGridView1.Rows[rowIndex].Cells["Дата начала"].Value;
                            row["Дата окончания"] = dataGridView1.Rows[rowIndex].Cells["Дата окончания"].Value;
                            row["Статус"] = dataGridView1.Rows[rowIndex].Cells["Статус"].Value;

                            dataSet.Tables["Заголовок"].Rows.Add(row);

                            dataSet.Tables["Заголовок"].Rows.RemoveAt(dataSet.Tables["Заголовок"].Rows.Count - 1);

                            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                            dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";

                            sqlDataAdapter.Update(dataSet, "Заголовок");

                            newRowAdding = false;
                        }
                            
                    }
                    else if (task == "Update")
                    {
                        if (MessageBox.Show("Обновить данные?", "Обновление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int r = e.RowIndex;

                            dataSet.Tables["Заголовок"].Rows[r]["Id заказа"] = dataGridView1.Rows[r].Cells["Id заказа"].Value;
                            dataSet.Tables["Заголовок"].Rows[r]["Цех производитель"] = dataGridView1.Rows[r].Cells["Цех производитель"].Value;
                            dataSet.Tables["Заголовок"].Rows[r]["Дата начала"] = dataGridView1.Rows[r].Cells["Дата начала"].Value;
                            dataSet.Tables["Заголовок"].Rows[r]["Дата окончания"] = dataGridView1.Rows[r].Cells["Дата окончания"].Value;
                            dataSet.Tables["Заголовок"].Rows[r]["Статус"] = dataGridView1.Rows[r].Cells["Статус"].Value;

                            sqlDataAdapter.Update(dataSet, "Заголовок");

                            dataGridView1.Rows[e.RowIndex].Cells[5].Value = "Delete";
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

                    dataGridView1[5, lastRow] = linkCell;

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

                    dataGridView1[5, rowIndex] = linkCell;

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
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"[Цех производитель] LIKE '%" + textBox1.Text + "%'";
        }

    }
}

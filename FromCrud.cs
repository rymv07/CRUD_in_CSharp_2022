using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUD_in_CSharp_2022
{
    public partial class FromCrud : Form
    {

        static SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dtread;


        public FromCrud()
        {
            InitializeComponent();
        }

        void DbConnection()
        {
            conn = new SqlConnection(@"Data Source=RYMV\SQLEXPRESS;Initial Catalog=CRUDExperimental;Persist Security Info=True;User ID=sa;Password=msadmin");

            conn.Open();
        }


        private void FromCrud_Load(object sender, EventArgs e)
        {
            DbConnection();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            if (txtProdId.Text != string.Empty)
            {
                cmd = new SqlCommand("select * from productinfo where ProductID = '" + int.Parse(txtProdId.Text) + "' ", conn);
                dtread = cmd.ExecuteReader();

                if (dtread.Read())
                {
                    dtread.Close();
                    MessageBox.Show("Product ID already exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dtread.Close();

                    DbConnection();

                    cmd = new SqlCommand("insert into productinfo values(@ProductID, @ItemName, @Model, @Color, getdate(), NULL)", conn);
                    cmd.Parameters.AddWithValue("ProductID", int.Parse(txtProdId.Text));
                    cmd.Parameters.AddWithValue("ItemName", txtItem.Text);
                    cmd.Parameters.AddWithValue("Model", txtModel.Text);
                    cmd.Parameters.AddWithValue("Color", cmbColor.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data is Inserted.");
                    conn.Close();

                    BindData();

                    txtProdId.Text = string.Empty;
                    txtItem.Text = string.Empty;
                    txtModel.Text = string.Empty;
                    cmbColor.Text = string.Empty;
                    dataGridView.Refresh();

                }
             
            }
            else
            {
                MessageBox.Show("Please enter valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // next to fix errors on this portion
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtProdId.Text != string.Empty)
            {
                if (dataGridView != null || dataGridView.Rows.Count != 0)
                {
                    DbConnection();

                    cmd = new SqlCommand("update productinfo set ItemName = '" + txtItem.Text + "', Model = '" + txtModel.Text + "', Color = '" + cmbColor.Text + "', UpdateDate = getDate() where ProductID = '" + int.Parse(txtProdId.Text) + "'", conn);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data is Updated.");
                }
                else
                {
                    MessageBox.Show("Product ID did not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }           
            }
            else
            {
                MessageBox.Show("Please enter valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                if (dataGridView != null || dataGridView.Rows.Count != 0)
                {
                    DbConnection();

                    cmd = new SqlCommand("select * from productinfo where ProductID = '" + int.Parse(txtSearch.Text) + "' ", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Product ID did not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  
            }
            else
            {
                MessageBox.Show("Please enter valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView.Refresh();
            }
                  
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {

                cmd = new SqlCommand("select * from productinfo where ProductID = '" + int.Parse(txtSearch.Text) + "' ", conn);

                if (dataGridView != null || dataGridView.Rows.Count != 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Once deleted, it cannot be retrieved again.", "OK to Continue", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK)
                    {

                        DbConnection();

                        cmd = new SqlCommand("delete from productinfo where ProductID = '" + int.Parse(txtSearch.Text) + "'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Data is Deleted.");
                        dataGridView.Refresh();
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        DbConnection();

                        cmd = new SqlCommand("select * from productinfo where ProductID = '" + int.Parse(txtSearch.Text) + "'", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView.DataSource = dt;

                    }

                }
                else
                {
                    dtread.Close();
                    MessageBox.Show("Data not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please enter valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        void BindData()
        {
            cmd = new SqlCommand("select * from productinfo", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.DataSource = dt;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProdId.Text = string.Empty;
            txtItem.Text = string.Empty;
            txtModel.Text = string.Empty;
            cmbColor.Text = string.Empty;
            txtSearch.Text = string.Empty;
            this.dataGridView.DataSource = null;
            this.dataGridView.Rows.Clear();
            dataGridView.Refresh();
        }
    }
}

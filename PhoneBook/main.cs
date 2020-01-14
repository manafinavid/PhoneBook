using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace PhoneBook
{
    public partial class main : Form
    {
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";
        public main()
        {
            InitializeComponent();
            dataGridView1.Rows.Add("Some Data");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlConnection sql = new SqlConnection(ConnectionString);
            sql.Open();
            SqlCommand command = new SqlCommand("select L,N,id from inf", sql);
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader[0].ToString() + "," + reader[1].ToString(), reader[2].ToString());
            }
        }
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Add add = new Add(0);
            add.ShowIcon = false;
            add.ShowInTaskbar = false;
            add.ShowDialog();
            Form1_Load(this, EventArgs.Empty);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                Add add = new Add(1, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
                add.ShowIcon = false;
                add.ShowInTaskbar = false;
                add.ShowDialog();
                Form1_Load(this, EventArgs.Empty);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection sql = new SqlConnection(ConnectionString);
                sql.Open();
                SqlCommand command = new SqlCommand("delete from inf where id=" + dataGridView1.CurrentRow.Cells[1].Value.ToString(), sql);
                command.ExecuteNonQuery();
                Form1_Load(this, EventArgs.Empty);
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Add add = new Add(2, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
            add.ShowIcon = false;
            add.ShowInTaskbar = false;
            add.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Search about = new Search();
            about.ShowDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Groups about = new Groups();
            about.ShowDialog();
        }
    }
}

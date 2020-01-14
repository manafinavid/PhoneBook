using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Result : Form
    {
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";

        public Result(List<string> vs)
        {
            InitializeComponent();
            for(int i = 0; i < vs.Count; i++)
            {
                SqlConnection sql = new SqlConnection(ConnectionString);
                SqlCommand command = new SqlCommand("select * from inf where id=" + vs[i],sql);
                sql.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                reader.Read();
                dataGridView1.Rows.Add(reader[2].ToString() + "," + reader[1].ToString(),reader[0].ToString());
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Add add = new Add(1, dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
            add.ShowIcon = false;
            add.ShowInTaskbar = false;
            add.ShowDialog();
        }

        private void Result_Load(object sender, EventArgs e)
        {

        }
    }
}

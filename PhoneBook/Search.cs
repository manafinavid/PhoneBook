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
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";

        private void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand("select * from inf", sql);
            sql.Open();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            List<string> vs = new List<string>();
            while (reader.Read())
            {
                for(int i = 1; i < 10; i++)
                {
                    if (reader[i].ToString().IndexOf(textBox1.Text) != -1)
                    {
                        vs.Add(reader[0].ToString());
                        break;
                    }
                }
            }
            if (vs.Count == 0)
            {
                MessageBox.Show("No results found");
            }
            else
            {
                MessageBox.Show(vs.Count+ " result found");
                this.Hide();
                Result result = new Result(vs);
                result.ShowIcon = false;
                result.ShowInTaskbar = false;
                result.ShowDialog();
                Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

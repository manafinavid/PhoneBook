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
    public partial class Group : Form
    {
        TextBox B;
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";
        public Group(ref TextBox box)
        {
            InitializeComponent();
            B = box;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string temp = "";
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "True")
                {
                    temp += dataGridView1.Rows[i].Cells[1].Value.ToString()+";";
                }
            }
            B.Text = temp;
            Close();
        }

        private void Group_Load(object sender, EventArgs e)
        {
            string temp = B.Text,T="";
            ShowIcon = false;
            SqlConnection sql = new SqlConnection(ConnectionString);
            sql.Open();
            SqlCommand command = new SqlCommand("select Group_name from Groups", sql);
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                dataGridView1.Rows.Add(false, reader[0].ToString());
            }
            reader.Close();
            while (temp!="")
            {
                T = temp.Substring(0, temp.IndexOf(';'));
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if(dataGridView1.Rows[i].Cells[1].Value.ToString() == T)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = true;
                    }
                }
                temp = temp.Substring(temp.IndexOf(';') + 1);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

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
    public partial class Groups : Form
    {
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";

        public Groups()
        {
            InitializeComponent();
        }

        private void Groups_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            SqlConnection sql = new SqlConnection(ConnectionString);
            sql.Open();
            SqlCommand command = new SqlCommand("select Group_name from Groups", sql);
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader[0].ToString());
            }
            reader.Close();
        }
        List<string> backup= new List<string>();
        private void Button4_Click(object sender, EventArgs e)
        {
            if (button4.Text != "&Save")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    backup.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                dataGridView1.ReadOnly = !dataGridView1.ReadOnly;
                button4.Text = "&Save";
                button1.Enabled = button2.Enabled = button3.Enabled = false;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() != backup[i])
                    {
                        {
                            SqlConnection sql1 = new SqlConnection(ConnectionString);
                            sql1.Open();
                            SqlCommand command1 = new SqlCommand("update Groups set Group_name=@G where Group_name=@R", sql1);
                            command1.Parameters.Add("@G", SqlDbType.NVarChar).Value = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            command1.Parameters.Add("@R", SqlDbType.NVarChar).Value = backup[i];
                            command1.ExecuteNonQuery();
                        }
                        {
                            SqlConnection sql = new SqlConnection(ConnectionString);
                            sql.Open();
                            SqlCommand command = new SqlCommand("select id,Group_ from inf", sql);
                            command.ExecuteNonQuery();
                            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                            string LOL;
                            while (reader.Read())
                            {
                                LOL = reader[1].ToString();
                                if (( LOL.IndexOf(backup[i])) != -1)
                                {
                                    SqlConnection sql1 = new SqlConnection(ConnectionString);
                                    sql1.Open();
                                    SqlCommand command1 = new SqlCommand("update inf set Group_=@G where id=" + reader[0].ToString(), sql1);
                                    LOL = LOL.Replace(backup[i], dataGridView1.CurrentRow.Cells[0].Value.ToString());
                                    command1.Parameters.Add("@G", SqlDbType.Text).Value = LOL;
                                    command1.ExecuteNonQuery();
                                }
                            }
                            reader.Close();
                            sql.Close();
                        }
                    }
                }
                dataGridView1.ReadOnly = !dataGridView1.ReadOnly;
                button1.Enabled = button2.Enabled = button3.Enabled = true;
                button4.Text = "&Turn edit on";
            }

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Select a group first");
                return;
            }
            if( MessageBox.Show("Are you Sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                {
                    ShowIcon = false;
                    SqlConnection sql = new SqlConnection(ConnectionString);
                    sql.Open();
                    SqlCommand command = new SqlCommand("select id,Group_ from inf", sql);
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    int pos = 0;
                    string LOL;
                    while (reader.Read())
                    {
                        LOL = reader[1].ToString();
                        if ((pos = LOL.IndexOf(dataGridView1.CurrentRow.Cells[0].Value.ToString() + ";")) != -1)
                        {
                            SqlConnection sql1 = new SqlConnection(ConnectionString);
                            sql1.Open();
                            SqlCommand command1 = new SqlCommand("update inf set Group_=@G where id=" + reader[0].ToString(), sql1);
                            LOL = LOL.Remove(pos, dataGridView1.CurrentRow.Cells[0].Value.ToString().Length + 1);
                            command1.Parameters.Add("@G", SqlDbType.Text).Value = LOL;
                            command1.ExecuteNonQuery();
                        }
                    }
                    reader.Close();
                    sql.Close();
                }
                SqlConnection sql2 = new SqlConnection(ConnectionString);
                sql2.Open();
                SqlCommand command2 = new SqlCommand("delete from groups where Group_name=@G", sql2);
                command2.Parameters.Add("@G", SqlDbType.NVarChar).Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                command2.ExecuteNonQuery();
                backup.Remove(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
            }
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }



        private void Button2_Click(object sender, EventArgs e)
        {
Start:            string temp="";
            if(InputBox("","Enter new group name:",ref temp)== DialogResult.OK)
            {
                if (temp == "")
                {
                    MessageBox.Show("Please enter a name");
                    goto Start;
                }
                if (temp.IndexOf(';') != -1)
                {
                    MessageBox.Show("Symbol error Do not use \";\"") ;
                    return;
                }
                for(int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == temp)
                    {
                        MessageBox.Show("This name is already available");
                        return;
                    }
                }
                SqlConnection sql = new SqlConnection(ConnectionString);
                SqlCommand command = new SqlCommand("insert into groups VALUES (@text)", sql);
                command.Parameters.AddWithValue("@text", temp);
                sql.Open();
                command.ExecuteNonQuery();
                dataGridView1.Rows.Add(temp);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            List<string> vs = new List<string>();
            SqlConnection sql = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand("select id,group_ from inf", sql);
            sql.Open();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                if (reader[1].ToString().IndexOf(dataGridView1.CurrentRow.Cells[0].Value.ToString())!= -1)
                {
                    vs.Add(reader[0].ToString());
                }
            }
            Result RL = new Result(vs);
            RL.ShowIcon = false;
            RL.ShowInTaskbar = false;
            RL.ShowDialog();
        }
    }
}

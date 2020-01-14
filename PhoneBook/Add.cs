using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Add : Form
    {
        readonly string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"|DataDirectory|\\PhoneData.mdf\";Integrated Security=True";
        string Command = "";
        public Add(int Mode, string id = "-1")
        {
            InitializeComponent();
            if (Mode == 0)
            {
                Text = "Add";
                Command = "insert into inf  VALUES (@n,@ln,@nn,@e,@g,@a,@w,@Note,@num,@i);";
            }
            else if(Mode==1)
            {
                Text = "Edit";
                load(id);
            }
            else
            {
                this.Size = new System.Drawing.Size(270, 460);
                load(id);
                Name_.ReadOnly = true;
                LName.ReadOnly = true;
                NName.ReadOnly = true;
                Email.ReadOnly = true;
                WebSite.ReadOnly = true;
                Adderss.ReadOnly = true;
                Groups.ReadOnly = true;
                Note.ReadOnly = true;
                dataGridView1.ReadOnly = true;
                button3.Visible = false;
                Groups.Size = Name_.Size;
            }
        }
        void load(string id)
        {
            Command = "update inf set N=@n , L=@ln , Nickname=@nn , Email=@e , Group_=@g , Address=@a , Web=@w , Note=@Note , number=@num , image=@i  where id=" + id + ";";
            SqlConnection sql = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand("select * from inf where id = " + id + ";", sql);
            sql.Open();
            SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                string temp = reader[9].ToString();
                int pos = 0;
                Name_.Text = reader[1].ToString();
                LName.Text = reader[2].ToString();
                NName.Text = reader[3].ToString();
                Email.Text = reader[4].ToString();
                Groups.Text = reader[5].ToString();
                Adderss.Text = reader[6].ToString();
                WebSite.Text = reader[7].ToString();
                Note.Text = reader[8].ToString();
                for (int i = 0; temp.Length != 0; i++)
                {
                    pos = temp.IndexOf(";");
                    dataGridView1.Rows.Add(temp.Substring(0, pos));
                    temp = temp.Remove(0, pos + 1);
                }
                try
                {
                    ImageConverter T = new ImageConverter();
                    pictureBox1.Image = (Image)T.ConvertFrom(reader[10]);
                }
                catch (Exception ex)
                {
                }

            }
            reader.Close();
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!LName.ReadOnly)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Image files | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFile.Multiselect = false;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.BackgroundImage = null;
                    pictureBox1.Load(openFile.FileName);
                }
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_.Text == "")
            {
                MessageBox.Show("Pls Fill Name Field!!!");
                return;
            }
            string numbers = "";
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                numbers += (dataGridView1.Rows[i].Cells[0].Value.ToString() + ";");
            }
            SqlConnection sql = new SqlConnection(ConnectionString);
            sql.Open();
            SqlCommand command = new SqlCommand(Command, sql);
            command.Parameters.Add("@n", SqlDbType.NVarChar).Value = Name_.Text;
            command.Parameters.Add("@nn", SqlDbType.NVarChar).Value = NName.Text;
            command.Parameters.Add("@ln", SqlDbType.NVarChar).Value = LName.Text;
            command.Parameters.Add("@e", SqlDbType.NVarChar).Value = Email.Text;
            command.Parameters.Add("@g", SqlDbType.NVarChar).Value = Groups.Text;
            command.Parameters.Add("@a", SqlDbType.NVarChar).Value = Adderss.Text;
            command.Parameters.Add("@w", SqlDbType.NVarChar).Value = WebSite.Text;
            command.Parameters.Add("@Note", SqlDbType.NVarChar).Value = Note.Text;
            if (pictureBox1.Image != null)
            {
                try
                {
                    ImageConverter converter = new ImageConverter();
                    command.Parameters.Add("@i", SqlDbType.Image).Value = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                command.CommandText = command.CommandText.Replace(",@i", "");
            }
            command.Parameters.Add("@num", SqlDbType.Text).Value = numbers;
            command.ExecuteNonQuery();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
}

private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Group G = new Group(ref Groups);
            G.ShowIcon = false;
            G.ShowInTaskbar = false;
            G.ShowDialog();
        }
    }
}

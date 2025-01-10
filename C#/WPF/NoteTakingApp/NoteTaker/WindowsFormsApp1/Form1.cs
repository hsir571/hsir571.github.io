using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTakingApp
{
    public partial class NoteTakingApp : Form
    {
        DataTable notes = new DataTable();
        bool editing = false;
        public NoteTakingApp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notes.Columns.Add("Title");
            notes.Columns.Add("Note");

            savedNotes.DataSource = notes;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void messageLabel_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {

                notes.Rows[savedNotes.CurrentCell.RowIndex].Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Have not selected a Note");
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (editing)
            {
                notes.Rows[savedNotes.CurrentCell.RowIndex]["title"] = titleBox.Text;
                notes.Rows[savedNotes.CurrentCell.RowIndex]["message"] = messageBox.Text;
            }
            else
            {
                notes.Rows.Add(titleBox.Text, messageBox.Text);
            }
            titleBox.Text = "";
            messageBox.Text = "";
            editing = false;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                titleBox.Text = notes.Rows[savedNotes.CurrentCell.RowIndex].ItemArray[0].ToString();
                messageBox.Text = notes.Rows[savedNotes.CurrentCell.RowIndex].ItemArray[1].ToString();
                editing = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            titleBox.Text = "";
            messageBox.Text = "";
        }

        private void savedNotes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                titleBox.Text = notes.Rows[savedNotes.CurrentCell.RowIndex].ItemArray[0].ToString();
                messageBox.Text = notes.Rows[savedNotes.CurrentCell.RowIndex].ItemArray[1].ToString();
                editing = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}

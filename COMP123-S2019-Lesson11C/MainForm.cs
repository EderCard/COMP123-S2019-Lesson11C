using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace COMP123_S2019_Lesson11C
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// This is the event handler for the MainForm Closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// This is the event handler for the ExitToolStripMenuItem click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// This is the event handler for the AboutToolStripItem click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.aboutForm.ShowDialog();
        }

        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            Program.aboutForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sectionCDatabaseDataSet.StudentTable' table. You can move, or remove it, as needed.
            this.studentTableTableAdapter.Fill(this.sectionCDatabaseDataSet.StudentTable);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            Program.studentInfoForm.Show();
            this.Hide();
        }

        
        private void StudentDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //local variables that are used as aliases
            var currentCell = StudentDataGridView.CurrentCell;
            var rowIndex = StudentDataGridView.CurrentCell.RowIndex;
            var currentRow = StudentDataGridView.Rows[rowIndex];
            var columnCount = StudentDataGridView.ColumnCount;
            var cells = currentRow.Cells;

            currentRow.Selected = true;

            string outputString = ""; //or string.Empty

            for (int index = 0; index < columnCount; index++)
            {
                outputString += cells[index].Value + " ";
            }

            SelectionLabel.Text = outputString;

            Program.student.id = int.Parse(cells[(int)studentField.ID].Value.ToString());
            Program.student.StudentID = cells[(int)studentField.STUDENT_ID].Value.ToString();
            Program.student.FirstName = cells[(int)studentField.FIRST_NAME].Value.ToString();
            Program.student.LastName = cells[(int)studentField.LAST_NAME].Value.ToString();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //configure the file dialog
            StudentSaveFileDialog.FileName = "Student.txt";
            StudentSaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentSaveFileDialog.Filter = "Text files (*.txt)|*.txt| All files (*.*)|*.*";

            //open the file dialog
            var result = StudentSaveFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                // open stream to write
                using (StreamWriter outputStream = new StreamWriter(
                    File.Open(StudentSaveFileDialog.FileName, FileMode.Create)))
                {
                    // write stuff to the file
                    outputStream.WriteLine(Program.student.id.ToString());
                    outputStream.WriteLine(Program.student.StudentID);
                    outputStream.WriteLine(Program.student.FirstName);
                    outputStream.WriteLine(Program.student.LastName);

                    //cleanup
                    outputStream.Close();
                    outputStream.Dispose();
                }
                //give feedback to the user that the file has been saved
                //this is a "modal" form
                MessageBox.Show("File saved successfuly!", "Saving...",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            //configure the file dialog
            StudentOpenFileDialog.FileName = "Student.txt";
            StudentOpenFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            StudentOpenFileDialog.Filter = "Text files (*.txt)|*.txt| All files (*.*)|*.*";

            //open the file dialog
            var result = StudentOpenFileDialog.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                try
                {
                    //open stream for reading
                    using (StreamReader inputStream = new StreamReader(
                        File.Open(StudentOpenFileDialog.FileName, FileMode.Open)))
                    {
                        //Read stuff from the file into the Student object
                        Program.student.id = int.Parse(inputStream.ReadLine());
                        Program.student.StudentID = inputStream.ReadLine();
                        Program.student.FirstName = inputStream.ReadLine();
                        Program.student.LastName = inputStream.ReadLine();

                        //Cleanup
                        inputStream.Close();
                        inputStream.Dispose();
                    }
                    NextButton_Click(sender, e);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);

                    MessageBox.Show("Error " + exception.Message, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

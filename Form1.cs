using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cpts321;

namespace Cpts321
{
    public partial class Form1 : Form
    {
        //from the spreadsheet class in Class1.cs
        public Spreadsheet s;

        /// <summary>
        /// Initializes the spreadsheet
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            s = new Spreadsheet(50, 26);
        }

        /// <summary>
        /// This will be called when edit begins
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            dataGridView1.Rows[row].Cells[col].Value = (s.GetCell(row, col)).Text;
        }

        /// <summary>
        /// This will be called when edit ends or basically when it is done 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            string txt = "";
            Cell cell = s.GetCell(row, col);

            try
            {
                txt = dataGridView1.Rows[row].Cells[col].Value.ToString();
            }
            catch(NullReferenceException)
            {
                txt = "";
            }
            cell.Text = txt;
        }

        /// <summary>
        /// wiill inform and change the property of the cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellPropChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell cell = sender as Cell;
            if(cell != null && e.PropertyName == "Value")
            {
                dataGridView1.Rows[cell.RowIndex].Cells[cell.ColIndex].Value = cell.Value;
            }
        }

        /// <summary>
        /// loads the rows and columns and also will update each cells if there is a text value inputted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            s.CellPropertyChanged += OnCellPropChanged; // this is where the property changes are 

            // this is to update with the editing
            dataGridView1.CellBeginEdit += CellBeginEdit;
            dataGridView1.CellEndEdit += CellEndEdit;

            // clears both columns and rows
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            // adds columns to the application
            for(char i = 'A'; i <= 'Z'; i++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

                col.Name = i.ToString();

                dataGridView1.Columns.Add(col);
            }

            // adds rows to the application
            for(int j = 1; j <= 50; j++)
            {
                DataGridViewRow row = new DataGridViewRow();

                row.HeaderCell.Value = (j).ToString();

                dataGridView1.Rows.Add(row);
            }
        }

        ///// <summary>
        ///// when we click to perform demo it will go to the spreadsheet class and goes through the demo
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void button_Click(object sender, EventArgs e)
        //{
        //    s.Demo();
        //}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Cpts321
{
    //class for the spreadsheet
    public class Spreadsheet
    {
        public event PropertyChangedEventHandler CellPropertyChanged; // to subscribe to a single event
        private int rowCount;
        private int colCount;
        private Cell[,] mCell;
        public Dictionary<Cell, List<Cell>> mDependecies; // a dictionary of dependencies

        /// <summary>
        /// this is where we can put strings in the spreadsheet
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Spreadsheet(int row, int col)
        {
            this.rowCount = row;
            this.colCount = col;
            mCell = new Cell[row, col];

            //2d array cell
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mCell[i, j] = new CellInit(i, j);
                    mCell[i, j].PropertyChanged += OnPropChanged;
                }
            }
        }

        public int RowCount
        {
            get { return this.rowCount; }
        }

        public int ColCount
        {
            get { return this.colCount; }
        }

        /// <summary>
        /// returns the cell of a specific location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Cell GetCell(int row, int col)
        {
            if (row > rowCount || col > ColCount)
            {
                return null;
            }

            return mCell[row, col];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell c = sender as Cell; // makes the sender into a cell and setting a temp value for it
            if (e.PropertyName == "Text")
            {
                UpdateCell(c);
            }
            CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value")); // this is to check if there is an inputted value and will check if there is a property changed
        }

        /// <summary>
        /// this is where we place 
        /// </summary>
        /// <param name="c"></param>
        private void UpdateCell(Cell c)
        {
            double cVal = 0.0;
            string cName = "";
            ExpTree tree;

            // converting the cell c into a string to put in the exptree
            cName += Convert.ToChar('A' + c.ColIndex);
            cName += (c.RowIndex + 1).ToString();

            if(!c.Text.StartsWith("="))
            {
                if (double.TryParse(c.Text, out cVal))
                {
                    tree = new ExpTree(c.Text);
                    cVal = tree.Eval();
                    tree.SetVar(cName, cVal);
                    c.Value = cVal.ToString();
                }
                else
                {
                    c.Value = c.Text;
                }
            }
            // will have to make a new tree with expression
            else
            {
                remDependencies(c);
                tree = new ExpTree(c.Text.Substring(1));
                setTreeVar(ref tree);
                addDependencies(c, tree.GetVar());
            }
        }

        /// <summary>
        /// this will set the var values in the exptree dictionary
        /// </summary>
        /// <param name="tree"></param>
        private void setTreeVar(ref ExpTree tree)
        {
            foreach(string name in tree.GetVar())
            {
                int clCol = name[0] - 'A';
                int clRow = Convert.ToInt32(name.Substring(1)) - 1;
                double val = 0.0;
                if(double.TryParse(mCell[clRow, clCol].Value, out val))
                {
                    tree.SetVar(name, val);
                }
            }
        }

        /// <summary>
        /// removes dependicies in the cell
        /// </summary>
        /// <param name="c"></param>
        private void remDependencies(Cell c)
        {
            foreach (List<Cell> dCell in mDependecies.Values)
            {
                if(dCell.Contains(c))
                {
                    dCell.Remove(c);
                }
            }
        }

        /// <summary>
        /// adds the dependencies in a dictionary
        /// </summary>
        /// <param name="c"></param>
        /// <param name="ind"></param>
        private void addDependencies(Cell c, string[] ind)
        {
            foreach(string cl in ind)
            {
                // this will get the independent cell
                int clCol = cl[0] - 'A';
                int clRow = Convert.ToInt32(cl.Substring(1)) - 1;
                Cell inCl = GetCell(clRow, clCol);

                this.mDependecies[inCl].Add(c);
            }
        }


        ///// <summary>
        ///// this is what the perform demo button does when pressed
        ///// </summary>
        //public void Demo()
        //{
        //    // randomize which row and column it chooses to put the "I love C#" text 
        //    int randomRow = 0, randomCol = 0;
        //    Random random = new Random();

        //    for (int i = 0; i < 50; i++)
        //    {
        //        randomCol = random.Next(0, 25);
        //        randomRow = random.Next(0, 49);

        //        Cell fill = GetCell(randomRow, randomCol);
        //        fill.Text = "I love C#";
        //        mCell[randomRow, randomCol] = fill;
        //    }

        //    // goes to column B and places the string
        //    for (int i = 0; i < 50; i++)
        //    {
        //        this.mCell[i, 1].Text = "This is Cell B" + (i + 1).ToString();
        //    }

        //    // copies because the text starts with '='
        //    for (int i = 0; i < 50; i++)
        //    {
        //        this.mCell[i, 0].Text = "=B" + (i + 1).ToString();
        //    }
        //}
    }
}

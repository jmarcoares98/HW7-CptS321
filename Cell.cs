using System;
using System.ComponentModel;

namespace Cpts321
{
    public abstract class Cell : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected int mRowIndex;
        protected int mColIndex;
        protected string mText;
        protected string mValue;

        // Constructor
        public Cell(int row, int column)
        {
            mRowIndex = row;
            mColIndex = column;
            mText = "";
            mValue = "";
        }

        // notify if there is a property change on the cell
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // getter and setter for row
        public int RowIndex
        {
            get { return mRowIndex; }
            protected set { this.mRowIndex = value; }
        }

        // getter and setter for column
        public int ColIndex
        {
            get { return mColIndex; }
            protected set { this.mColIndex = value; }
        }

        // getter and setter for text
        public string Text
        {
            get { return this.mText; }

            protected internal set
            {
                if (value != this.mText)
                {
                    this.mText = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
                else
                {
                    return;
                }
            }
        }

        // getter and setter for value
        public string Value
        {
            get { return this.mValue; }

            protected internal set
            {
                if (value != this.mValue)
                {
                    this.mValue = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
                else
                {
                    return;
                }
            }
        }

    }
}

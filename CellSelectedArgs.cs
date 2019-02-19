namespace Codefarts.VectorGrid
{
    using System;

    public class CellSelectedArgs : EventArgs
    {
        public CellSelectedArgs(int row, int column)
            : this()
        {
            this.Row = row;
            this.Column = column;
        }

        public CellSelectedArgs()
        {
        }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
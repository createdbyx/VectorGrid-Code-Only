namespace Codefarts.VectorGrid
{
    using System;

    /// <summary>
    /// Provides event arguments for cell selection.
    /// </summary>
    public class CellSelectedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellSelectedArgs"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        public CellSelectedArgs(int row, int column)
            : this()
        {
            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CellSelectedArgs"/> class.
        /// </summary>
        public CellSelectedArgs()
        {
        }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        public int Column { get; set; }
    }
}
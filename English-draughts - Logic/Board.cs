namespace Ex04.Damka.Logic
{
    public class Board
    {
        private readonly byte m_NumOfRows;
        private readonly byte m_NumOfCols;
        private Cell[,] m_PlayBoard;

        public Board(byte i_boardSize)
        {
            m_NumOfRows = m_NumOfCols = i_boardSize;
            setBoard();
        }

        public void ResetBoard()
        {
            setBoardInitialValues();
        }

        public byte BoardSize { get => m_NumOfCols; }

        public Cell[,] PlayBoard { get => m_PlayBoard; set => m_PlayBoard = value; }

        public Cell this[Cell i_CurrentCell]
        {
            get => m_PlayBoard[i_CurrentCell.CellRow, i_CurrentCell.CellCol];
            set => m_PlayBoard[i_CurrentCell.CellRow, i_CurrentCell.CellCol] = i_CurrentCell;
        }

        internal Cell this[byte i_CurrRow, byte i_CurrCol] => m_PlayBoard[i_CurrRow, i_CurrCol];

        private void setBoard()
        {
            m_PlayBoard = new Cell[m_NumOfRows, m_NumOfCols];
            setBoardInitialValues();
        }

        public void setBoardInitialValues()
        {
            int numOfRowsForPlayer = (BoardSize - 2) / 2;
            eSign signToSet;
            for (byte curRow = 0; curRow < m_NumOfRows; curRow++)
            {
                if (curRow < numOfRowsForPlayer)
                {
                    signToSet = eSign.O;
                }
                else if (curRow == numOfRowsForPlayer || curRow == numOfRowsForPlayer + 1)
                {
                    signToSet = eSign.Empty;
                }
                else
                {
                    signToSet = eSign.X;
                }

                for (byte curCol = 0; curCol < m_NumOfCols; curCol++)
                {
                    if ((curCol + curRow) % 2 == 0)
                    {
                        m_PlayBoard[curRow, curCol] = new Cell(curRow, curCol, eSign.Empty);
                    }
                    else
                    {
                        m_PlayBoard[curRow, curCol] = new Cell(curRow, curCol, signToSet);
                    }
                }
            }
        }
    }
}

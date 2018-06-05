using System.Text;

namespace Ex05.Damka.Logic
{
    public class Cell
    {
        private const int k_LegalNumberOfInput = 2;
        private byte m_CellRow;
        private byte m_CellCol;
        private eSign m_CellSign;
        
        public Cell(byte i_CellRow, byte i_CellCol, eSign i_CellSign)
        {
            m_CellRow = i_CellRow;
            m_CellCol = i_CellCol;
            m_CellSign = i_CellSign;
        }

        public eSign CellSign { get => m_CellSign; set => m_CellSign = value; }

        public byte CellRow { get => m_CellRow; set => m_CellRow = value; }

        public byte CellCol { get => m_CellCol; set => m_CellCol = value; }

        public static bool Parse(string i_Input, out Cell o_ParsedCell)
        {
            bool isValidInput = false;
            o_ParsedCell = null;
            int rowNum, colNum;
            if(i_Input.Length == k_LegalNumberOfInput)
            {
                if (int.TryParse(i_Input[0].ToString(), out rowNum) && int.TryParse(i_Input[1].ToString(), out colNum))
                {
                    o_ParsedCell = new Cell((byte)rowNum, (byte)colNum, eSign.Empty);
                    isValidInput = true;
                }
            }

            return isValidInput;
        }

        public string GetCellStr()
        {
            StringBuilder cellStr = new StringBuilder();
            cellStr.Append((char)(m_CellCol + 'A'));
            cellStr.Append((char)(m_CellRow + 'a'));

            return cellStr.ToString();
        }
}
}

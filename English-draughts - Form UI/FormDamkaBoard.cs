using System.Drawing;
using System.Windows.Forms;
using Ex04.Damka.Logic;

namespace Ex04.Damka.FormUI
{   
    public class FormDamkaBoard : Form
    {
        private const byte m_width = 40;
        private const byte m_height = 40;
        private GameLogic m_GameLogic;
        private Button[,] m_DamkaBoard;       

        public FormDamkaBoard(string playerOneName, string playerTwoName, bool isSecondPlayerComputer, byte boardSize)
        {
            BackColor = Color.LightGray;
            Size = new Size(boardSize * 50, boardSize * 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damka";
            runGameLogic(playerOneName, playerTwoName, isSecondPlayerComputer, boardSize);
            createFormBoard(boardSize);
        }

        private void runGameLogic(string i_playerOneName, string i_playerTwoName, bool i_isSecondPlayerComputer, byte i_boardSize)
        {
            Player playerOne = new Player(ePlayerType.Human, eSign.X, i_playerOneName);
            Player playerTwo = new Player(i_isSecondPlayerComputer ? ePlayerType.Computer : ePlayerType.Human, eSign.O, i_playerTwoName);
            Player[] Players = new Player[2];
            Players[0] = playerOne;
            Players[1] = playerTwo;

            m_GameLogic = new GameLogic(Players, i_boardSize, i_isSecondPlayerComputer ? eGameType.HumanVsComputer : eGameType.HumanVsHuman);
        }

        private void createFormBoard(byte boardSize)
        {
            byte numOfRows;
            byte numOfColum;
            numOfRows = numOfColum = boardSize;

            var grayCell = Color.DarkGray;
            var whiteCell = Color.White;
            m_DamkaBoard = new Button[numOfRows, numOfColum];

            for (byte row = 0;  row < numOfRows; row++)
            {
                for (byte col = 0; col < numOfColum; col++)
                {              
                    Button newButton = new Button
                    {
                        Size = new Size(m_width, m_height),
                        Location = new Point(m_width * row, m_height * col)                       
                    };

                    Controls.Add(newButton);
                    m_DamkaBoard[row, col] = newButton;

                    if (row % 2 == 0)
                    {
                        newButton.BackColor = col % 2 != 0 ? whiteCell : grayCell;
                    }
                    else
                    {
                        newButton.BackColor = col % 2 != 0 ? grayCell : whiteCell;
                    }
                }
            }
        }
    }
}

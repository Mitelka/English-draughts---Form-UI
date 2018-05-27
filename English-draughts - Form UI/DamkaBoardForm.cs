using System.Drawing;
using System.Windows.Forms;
using Ex04.Damka.Logic;

namespace Ex04.Damka.FormUI
{   
    public class DamkaBoardForm : Form
    {
        private const byte k_Width = 40;
        private const byte k_Height = 40;
        private GameLogic m_GameLogic;
        private Button[,] m_DamkaBoard;       

        public DamkaBoardForm(string playerOneName, string playerTwoName, bool isSecondPlayerComputer, byte boardSize)
        {
            BackColor = Color.LightGray;
            Size = new Size(boardSize * 50, boardSize * 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damka";
            runGameLogic(playerOneName, playerTwoName, isSecondPlayerComputer, boardSize);
            createFormBoard(boardSize);
        }

        private void runGameLogic(string i_FirstPlayerName, string i_SecPlayerName, bool i_IsPlayerComputer, byte i_BoardSize)
        {
            Player playerOne = new Player(ePlayerType.Human, eSign.X, i_FirstPlayerName);
            Player playerTwo = new Player(i_IsPlayerComputer ? ePlayerType.Computer : ePlayerType.Human, eSign.O, i_SecPlayerName);
            Player[] Players = new Player[2];
            Players[0] = playerOne;
            Players[1] = playerTwo;

            m_GameLogic = new GameLogic(Players, i_BoardSize, i_IsPlayerComputer ? eGameType.HumanVsComputer : eGameType.HumanVsHuman);
        }

        private void createFormBoard(byte i_BoardSize)
        {
            byte numOfRows;
            byte numOfColum;
            numOfRows = numOfColum = i_BoardSize;

            var grayCell = Color.DarkGray;
            var whiteCell = Color.White;
            m_DamkaBoard = new Button[numOfRows, numOfColum];

            for (byte row = 0;  row < numOfRows; row++)
            {
                for (byte col = 0; col < numOfColum; col++)
                {              
                    Button newButton = new Button
                    {
                        Size = new Size(k_Width, k_Height),
                        Location = new Point(k_Width * row, k_Height * col)                       
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

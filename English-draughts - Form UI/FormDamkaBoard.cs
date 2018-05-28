using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Ex04.Damka.Logic;

namespace Ex04.Damka.FormUI
{   
    public class FormDamkaBoard : Form
    {
        private const byte k_Width = 40;
        private const byte k_Height = 40;
        private const byte k_BoardLocationX = 40;
        private const byte k_BoardLocationY = 40;
        private readonly Label m_FirstPlayerLabel = new Label();
        private readonly Label m_SecPlayerLabel = new Label();
        private readonly Label m_FirstPlayerScoreLabel = new Label();
        private readonly Label m_SecPlayerScoreLabel = new Label();
        private GameLogic m_GameLogic;
        private Button[,] m_DamkaBoard;
        private FormGameSettings m_FormSettings = new FormGameSettings();
        private List<PlayerButtonMovelist> possibleButtons = new List<PlayerButtonMovelist>();

        public FormDamkaBoard(string i_FirstPlayerName, string i_SecPlayerName, bool i_IsPlayerComputer, byte i_BoardSize)
        {
            BackColor = Color.LightGray;
            Size = new Size(i_BoardSize * 50, i_BoardSize * 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damka";
            runGameLogic(i_FirstPlayerName, i_SecPlayerName, i_IsPlayerComputer, i_BoardSize);
            createFormBoard(i_BoardSize);
        }

        private void runGameLogic(string i_FirstPlayerName, string i_SecPlayerName, bool i_IsPlayerComputer, byte i_BoardSize)
        {
            Player playerOne = new Player(ePlayerType.Human, eSign.O, i_FirstPlayerName);
            Player playerTwo = new Player(i_IsPlayerComputer ? ePlayerType.Computer : ePlayerType.Human, eSign.X, i_SecPlayerName);
            Player[] players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;

            m_GameLogic = new GameLogic(players, i_BoardSize, i_IsPlayerComputer ? eGameType.HumanVsComputer : eGameType.HumanVsHuman);
        }

        private void createFormBoard(byte i_BoardSize)
        {
            byte numOfRows;
            byte numOfCol;
            numOfRows = numOfCol = i_BoardSize;

            var grayCell = Color.DarkGray;
            var whiteCell = Color.White;
            m_DamkaBoard = new Button[numOfRows, numOfCol];

            for (byte currRow = 0;  currRow < numOfRows; currRow++)
            {
                for (byte currCol = 0; currCol < numOfCol; currCol++)
                {
                    Button newButton = new Button
                    {
                        Size = new Size(k_Width, k_Height),
                        Location = new Point(k_BoardLocationX + (k_Width * currCol), k_BoardLocationY + (k_Height * currRow))
                    };
                    
                    newButton.Text = getSignToPrint(m_GameLogic.GameBoard[currRow, currCol].CellSign);
                    if (currRow % 2 == 0)
                    {
                        if(currCol % 2 != 0)
                        {
                            newButton.BackColor = whiteCell;
                            newButton.Click += new EventHandler(selectBoardButton_Click);
                        }
                        else
                        {
                            newButton.BackColor = grayCell;
                            newButton.Enabled = false;
                        }
                    }
                    else
                    {
                        if (currCol % 2 == 0)
                        {
                            newButton.BackColor = whiteCell;
                            newButton.Click += new EventHandler(selectBoardButton_Click);
                        }
                        else
                        {
                            newButton.BackColor = grayCell;
                            newButton.Enabled = false;
                        }
                    }

                    Controls.Add(newButton);
                    m_DamkaBoard[currRow, currCol] = newButton;
                }
            }

            initControls();
        }

        private void selectBoardButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            bool isAnotherButtonClicked = false;
            if(button.Text != " ")
            {
                button.BackColor = Color.LightBlue;
                button.Click -= selectBoardButton_Click;
                button.Click += new EventHandler(deselectBoardButton_Click);
            }
            else
            {
                foreach (Button cheackIfButtonSelect in m_DamkaBoard)
                {
                    if (cheackIfButtonSelect.BackColor == Color.LightBlue)
                    {                       
                        isAnotherButtonClicked = true;
                        
                        // check if legal move(setPossibleSquaresUI?), if legal move, update the board logic and the back color
                        break;
                    }                   
                }

                if (!isAnotherButtonClicked)
                {
                    MessageBox.Show("You must select a square to play with");
                }             
            }
        }

        private void deselectBoardButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.BackColor = Color.White;
            button.Click -= deselectBoardButton_Click;
            button.Click += new EventHandler(selectBoardButton_Click);
        }

        private void initControls()
        {
            m_FirstPlayerLabel.Text = "Player 1:";
            m_FirstPlayerLabel.Location = new Point(70, 20);
            m_FirstPlayerScoreLabel.Text = "0";
            m_FirstPlayerScoreLabel.Location = new Point(177, 20);

            m_SecPlayerLabel.Text = "Player 2:";
            m_SecPlayerLabel.Location = new Point(200, 20);
            m_SecPlayerScoreLabel.Text = "0";
            m_SecPlayerScoreLabel.Location = new Point(250, 20);

            Controls.AddRange(new Control[] { m_FirstPlayerLabel, m_FirstPlayerScoreLabel, m_SecPlayerLabel, m_SecPlayerScoreLabel });
        }

        private string getSignToPrint(eSign i_SignToPrint)
        {
            char signChar;
            switch (i_SignToPrint)
            {
                case eSign.Empty:
                    signChar = ' ';
                    break;
                case eSign.O:
                    signChar = 'O';
                    break;
                case eSign.X:
                    signChar = 'X';
                    break;
                case eSign.U:
                    signChar = 'U';
                    break;
                case eSign.K:
                    signChar = 'K';
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(i_SignToPrint), i_SignToPrint, null);
            }

            return signChar.ToString();
        }

        private void setPossibleButtonsUI(byte i_playerIndex)
        {
            bool o_DidEat = false;
            m_GameLogic.UpdateAllOptionalCellMove(i_playerIndex, m_GameLogic.Players[i_playerIndex].Sign, ref o_DidEat);
            List<Player.PlayerMovelist> PlayerMoveList = m_GameLogic.Players[i_playerIndex].PlayerPotentialMoveslist;
            
            foreach(Player.PlayerMovelist Movelist in PlayerMoveList)
            {
                Button original = m_DamkaBoard[Movelist.originalCell.CellRow, Movelist.originalCell.CellCol];
                Button desired = m_DamkaBoard[Movelist.desiredCell.CellRow, Movelist.desiredCell.CellCol];

                possibleButtons.Add(new PlayerButtonMovelist() { originalButton = original, desiredButton = desired });
            }
        }
    }
}

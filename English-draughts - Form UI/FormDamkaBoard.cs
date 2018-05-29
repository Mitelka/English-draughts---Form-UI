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
        private readonly GameLogic r_GameLogic;
        private Button[,] m_DamkaBoard;
        private FormGameSettings m_FormSettings = new FormGameSettings();
        //private List<PlayerButtonMovelist> possibleButtons = new List<PlayerButtonMovelist>();

        public FormDamkaBoard(byte i_BoardSize, GameLogic i_GameLogic)
        {
            BackColor = Color.LightGray;
            Size = new Size(i_BoardSize * 50, i_BoardSize * 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damka";
            r_GameLogic = i_GameLogic;
            r_GameLogic.CellChosen += cellButton_PrintingMove;
            r_GameLogic.CellChosen += cellButton_CheckingGameOver;
            createFormBoard(i_BoardSize);
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
                        Location = new Point(k_BoardLocationX + (k_Width * currCol), k_BoardLocationY + (k_Height * currRow)),
                        Name = $"{currRow}{currCol}",
                    };

                    newButton.Text = getSignToPrint(r_GameLogic.GameBoard[currRow, currCol].CellSign);
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

                    newButton.Click += CellButton_Click;
                    Controls.Add(newButton);
                    m_DamkaBoard[currRow, currCol] = newButton;
                }
            }

            initControls();
        }

        internal void CellButton_Click(object sender, EventArgs e)
        {
            Cell clickedCell;
            bool validCell = Cell.Parse(((Button)sender).Name, out clickedCell);

            Button button = (Button)sender;
            if (button != null && button.Enabled && validCell)
            {
                r_GameLogic.MakeMoveOnBoard(clickedCell, m_PlayerIndexTurn);
            }

            if (r_GameLogic.GameType == eGameType.PersonVsComputer && !this.m_IsGameFinished)
            {
                this.m_PlayerIndexTurn = GameLogic.GetOtherPlayerIndex(this.m_PlayerIndexTurn);
                this.HandleComputerMove();
            }
            else if (this.r_GameLogic.GameType == eGameType.PersonVsPerson)
            {
                this.m_PlayerIndexTurn = GameLogic.GetOtherPlayerIndex(this.m_PlayerIndexTurn);
            }
        }

        private void cellButton_CheckingGameOver(object i_Sender, CellChosenEventArgs i_E)
        {
            Cell.Parse(r_CellButtons[i_E.m_CellIndex].Name, out Cell clickedCell);

            m_IsGameFinished = this.r_GameLogic.CheckEndGame(clickedCell, this.m_CurrentNumOfMoves, this.m_PlayerIndexTurn);

            if (m_IsGameFinished)
            {
                byte winnerPlayerIndex = GameLogic.GetOtherPlayerIndex(this.m_PlayerIndexTurn);
                this.showResults(winnerPlayerIndex);
                this.resetRound();
            }
        }

        private void cellButton_PrintingMove(object i_Sender, CellChosenEventArgs i_E)
        {
            changeCellButtonAppearance(r_CellButtons[i_E.m_CellIndex], i_E.m_CellSign);
            BoldCurrentPlayerName(false);
        }

        private void changeCellButtonAppearance(Button i_CellButton, eSign i_PlayerSign)
        {
            i_CellButton.Enabled = false;
            i_CellButton.BackgroundImage = i_PlayerSign == eSign.O ? Resources.O : Resources.X;
            i_CellButton.BackgroundImageLayout = ImageLayout.Stretch;
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
                foreach (Button checkIfButtonSelect in m_DamkaBoard)
                {
                    if (checkIfButtonSelect.BackColor == Color.LightBlue)
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
                    throw new ArgumentOutOfRangeException(nameof(i_SignToPrint), i_SignToPrint, null);
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

                //possibleButtons.Add(new PlayerButtonMovelist() { originalButton = original, desiredButton = desired });
            }
        }
    }
}

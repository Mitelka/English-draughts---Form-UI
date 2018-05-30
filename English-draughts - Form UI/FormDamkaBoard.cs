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
        private Button m_OriginCell;
        private int m_CurrPlayerIndexTurn;
        private bool m_IsGameFinished = false;

        public FormDamkaBoard(byte i_BoardSize, GameLogic i_GameLogic)
        {
            BackColor = Color.LightGray; 
            //ClientSize = (i_BoardSize* 50) + k_BoardLocationX, (i_BoardSize * 50) + k_BoardLocationY);

            Size = new Size((i_BoardSize * 50) + k_BoardLocationX, (i_BoardSize * 50) + k_BoardLocationY);
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

                    //newButton.Click += CellButton_Click;
                    Controls.Add(newButton);
                    m_DamkaBoard[currRow, currCol] = newButton;
                }
            }

            initControls();
        }

        private void cellButtonClicked(Button i_ButtonClicked)
        {
            Cell clickedOriginCell, clickedDestCell;
            bool didEat = false;
            bool isKing;
            bool playerHasAnotherTurn = false;
            Button destButton = i_ButtonClicked;

            if (m_OriginCell != null)
            {
                bool validOriginCell = Cell.Parse(((Button)m_OriginCell).Name, out clickedOriginCell);
                bool validDestCell = Cell.Parse(destButton.Name, out clickedDestCell);


                r_GameLogic.UpdateAllOptionalCellMove(m_CurrPlayerIndexTurn, r_GameLogic.Players[m_CurrPlayerIndexTurn].Sign, ref didEat);
                if (destButton.Enabled && validOriginCell && validDestCell && r_GameLogic.AreCellsLegal(clickedOriginCell, clickedDestCell, r_GameLogic.Players[m_CurrPlayerIndexTurn].Sign, ref didEat))
                {
                    if (r_GameLogic.CheckIfCellsInThePossibleList(clickedOriginCell, clickedDestCell, m_CurrPlayerIndexTurn))
                    {
                        
                        r_GameLogic.MakeMoveOnBoard(clickedOriginCell, clickedDestCell, m_CurrPlayerIndexTurn, out isKing, out didEat);
                        if (didEat)
                        {
                            if (r_GameLogic.CheckDoubleEatingMove(clickedDestCell, m_CurrPlayerIndexTurn))
                            {
                                playerHasAnotherTurn = true;
                            }

                        }
                        r_GameLogic.UpdatePlayerTokens(m_CurrPlayerIndexTurn, didEat, isKing);
                    }
                    else
                    {
                        MessageBox.Show("Invalid move, please try again!");
                        playerHasAnotherTurn = true;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid move, please try again!");
                    playerHasAnotherTurn = true;
                }

                if (r_GameLogic.GameType == eGameType.HumanVsComputer && !m_IsGameFinished && !playerHasAnotherTurn)
                {
                    m_OriginCell = null;
                    m_CurrPlayerIndexTurn = r_GameLogic.GetOtherPlayerIndex(m_CurrPlayerIndexTurn);
                    handleComputerMove();
                }
                else if (r_GameLogic.GameType == eGameType.HumanVsHuman && !playerHasAnotherTurn)
                {
                    m_OriginCell = null;
                    m_CurrPlayerIndexTurn = r_GameLogic.GetOtherPlayerIndex(m_CurrPlayerIndexTurn);
                }
            }
        }

        private void handleComputerMove()
        {
            Cell o_LegalOriginCell, o_LegalDestCell;
            bool didEat = false;
            bool isKing;
            bool playerHasAnotherTurn = true;

            while (playerHasAnotherTurn)
            {
                r_GameLogic.SetComputerMove(m_CurrPlayerIndexTurn, out o_LegalOriginCell, out o_LegalDestCell, ref didEat);
                m_OriginCell = m_DamkaBoard[o_LegalOriginCell.CellRow, o_LegalOriginCell.CellCol];
                r_GameLogic.MakeMoveOnBoard(o_LegalOriginCell, o_LegalDestCell, m_CurrPlayerIndexTurn, out isKing, out didEat);
                if (didEat)
                {
                    if (!r_GameLogic.CheckDoubleEatingMove(o_LegalDestCell, m_CurrPlayerIndexTurn))
                    {
                        playerHasAnotherTurn = false;
                    }

                }
                else
                {
                    playerHasAnotherTurn = false;
                }
                r_GameLogic.UpdatePlayerTokens(m_CurrPlayerIndexTurn, didEat, isKing);
            }
            m_CurrPlayerIndexTurn = r_GameLogic.GetOtherPlayerIndex(m_CurrPlayerIndexTurn);
        }

        private void cellButton_CheckingGameOver(object i_Sender, CellsChosenEventArgs i_E)
        {
            m_IsGameFinished = r_GameLogic.CheckIfGameOver(i_E.m_DestCell, m_CurrPlayerIndexTurn);

            if (r_GameLogic.GameResult == eGameResult.WINNER)
            {
                r_GameLogic.UpdatePlayersScore(m_CurrPlayerIndexTurn);
                showWinnerResult(r_GameLogic.GetWinnerOfAllGamesIndex());
            }
            else if(r_GameLogic.GameResult == eGameResult.TIE)
            {
                showTieResults();
                
            }
        }

        private void showTieResults()
        {
            showResults("Tie!");
        }

        private void showWinnerResult(int i_WinnerIndex)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("Player ").Append(i_WinnerIndex).Append(" Won!");
            showResults(stringBuilder.ToString());
        }

        private void showResults(string i_Result)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.AppendLine(i_Result).Append("Another Round?");
            if(MessageBox.Show("Damka", stringBuilder.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                resetRound();
            }
            else
            {
                m_IsGameFinished = true;
                Close();
            }
        }

        private void resetRound()
        {
            r_GameLogic.GameBoard.ResetBoard();
            r_GameLogic.InitializeTokens();
            m_IsGameFinished = false;
          
        }

        private void cellButton_PrintingMove(object i_Sender, CellsChosenEventArgs i_E)
        {
            Button button = i_Sender as Button;
            changeCellButtonAppearance(i_E.m_DestCell.CellRow, i_E.m_DestCell.CellCol);
        }

        private void changeCellButtonAppearance(int i_DestCellRow, int i_DestCellCol)
        {
            m_OriginCell.BackColor = Color.White;
            m_DamkaBoard[i_DestCellRow, i_DestCellCol].Text = m_OriginCell.Text;
            m_OriginCell.Text = " ";

         }

        private void selectBoardButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text != " ")
            {
                button.BackColor = Color.LightBlue;
                button.Click -= selectBoardButton_Click;
                button.Click += new EventHandler(deselectBoardButton_Click);
                if (m_OriginCell != null)
                {
                    m_OriginCell.BackColor = Color.White;
                }

                m_OriginCell = button;
            }
            else
            {
                cellButtonClicked(button);
                
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
            m_FirstPlayerLabel.Location = new Point(m_DamkaBoard[0,0].Left + 12, 20);
            m_FirstPlayerLabel.AutoSize = true;
            m_FirstPlayerScoreLabel.Text = "0";
            m_FirstPlayerScoreLabel.Location = new Point(m_FirstPlayerLabel.Left + m_FirstPlayerLabel.Width, m_FirstPlayerLabel.Top);

            m_SecPlayerLabel.Text = "Player 2:";
            m_SecPlayerLabel.Location = new Point(m_FirstPlayerScoreLabel.Left + m_FirstPlayerScoreLabel.Width + 12, m_FirstPlayerLabel.Top);
            m_SecPlayerLabel.AutoSize = true;
            m_SecPlayerScoreLabel.Text = "0";
            m_SecPlayerScoreLabel.Location = new Point(m_SecPlayerLabel.Left + m_SecPlayerLabel.Width, m_FirstPlayerLabel.Top);

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

        //private void setPossibleButtonsUI(byte i_playerIndex)
        //{
        //    bool o_DidEat = false;
        //    m_GameLogic.UpdateAllOptionalCellMove(i_playerIndex, m_GameLogic.Players[i_playerIndex].Sign, ref o_DidEat);
        //    List<Player.PlayerMovelist> PlayerMoveList = m_GameLogic.Players[i_playerIndex].PlayerPotentialMoveslist;
            
        //    foreach(Player.PlayerMovelist Movelist in PlayerMoveList)
        //    {
        //        Button original = m_DamkaBoard[Movelist.originalCell.CellRow, Movelist.originalCell.CellCol];
        //        Button desired = m_DamkaBoard[Movelist.desiredCell.CellRow, Movelist.desiredCell.CellCol];

        //        //possibleButtons.Add(new PlayerButtonMovelist() { originalButton = original, desiredButton = desired });
        //    }
        //}
    }
}

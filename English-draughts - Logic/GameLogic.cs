using System;
using System.Collections.Generic;

namespace Ex04.Damka.Logic
{ 
    public delegate void CellChosenEventHandler(object sender, CellsChosenEventArgs e);

    public class GameLogic
    {
        private readonly Board m_GameBoard;
        private readonly Player[] m_Players;
        private byte m_BoardSize;
        private readonly eGameType m_GameType;
        private eGameResult m_GameResult;      

        public event CellChosenEventHandler CellChosen;

        public GameLogic(Player[] i_Players, byte i_BoardSize, eGameType i_GameType)
        {
            m_Players = i_Players;
            m_BoardSize = i_BoardSize;
            m_GameType = i_GameType;
            m_GameBoard = new Board(m_BoardSize);
            InitializeTokens();
        }

        public Board GameBoard { get => m_GameBoard; }

        public Player[] Players { get => m_Players; }

        public eGameResult GameResult { get => m_GameResult; }

        public eGameType GameType { get => m_GameType; }

        public void InitializeTokens()
        {
            int numberOfTokens = ((m_BoardSize - 2) / 2) * (m_BoardSize / 2);
            for (int playerIndex = 0; playerIndex < m_Players.Length; playerIndex++)
            {
                m_Players[playerIndex].NumOfTokens = numberOfTokens;
            }
        }

        public void UpdateAllOptionalCellMove(int i_PlayerIndex, eSign playerSign, ref bool o_DidEat)
        {
            m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Clear();
            foreach (Cell cell in m_GameBoard.PlayBoard)
            {
                if (cell.CellSign == playerSign || cell.CellSign == getKingsAlterSign(playerSign))
                {
                    foreach (Cell matchCell in m_GameBoard.PlayBoard)
                    {
                        if (matchCell.CellSign == eSign.Empty)
                        {
                            if (AreCellsLegal(cell, matchCell, playerSign, ref o_DidEat))
                            {
                                m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Add(new Player.PlayerMovelist() { originalCell = cell, desiredCell = matchCell });
                            }
                        }
                    }
                }
            }

            updateAllEatingOptionalCellMove(i_PlayerIndex);
        }

        private void updateAllEatingOptionalCellMove(int i_PlayerIndex)
        {
            List<Player.PlayerMovelist> playerPotentialEatinglist = new List<Player.PlayerMovelist>();
            foreach (Player.PlayerMovelist optionaEatingMove in m_Players[i_PlayerIndex].PlayerPotentialMoveslist)
            {
                if (Math.Abs(optionaEatingMove.originalCell.CellRow - optionaEatingMove.desiredCell.CellRow) == 2)
                {
                    playerPotentialEatinglist.Add(new Player.PlayerMovelist() { originalCell = optionaEatingMove.originalCell, desiredCell = optionaEatingMove.desiredCell });
                }
            }

            if (playerPotentialEatinglist.Count != 0)
            {
                m_Players[i_PlayerIndex].PlayerPotentialMoveslist = playerPotentialEatinglist;
            }
        }

        public bool AreCellsLegal(Cell i_OriginCell, Cell i_DestCell, eSign i_PlayerSign, ref bool o_DidEatFlag)
        {
            bool moveIsLegal = true;
            if (i_OriginCell.CellRow < 0 || i_OriginCell.CellRow > m_BoardSize || i_OriginCell.CellCol < 0 || i_OriginCell.CellCol > m_BoardSize)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (i_DestCell.CellRow < 0 || i_DestCell.CellRow > m_BoardSize || i_DestCell.CellCol < 0 || i_DestCell.CellCol > m_BoardSize)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if ((m_GameBoard[i_OriginCell].CellSign != i_PlayerSign && m_GameBoard[i_OriginCell].CellSign != getKingsAlterSign(i_PlayerSign)) || m_GameBoard[i_DestCell].CellSign != eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_DestCell].CellSign != eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_OriginCell].CellSign == eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_DestCell].CellRow > m_BoardSize || m_GameBoard[i_DestCell].CellCol > m_BoardSize || m_GameBoard[i_DestCell].CellRow < 0 || m_GameBoard[i_DestCell].CellCol < 0)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (Math.Abs(m_GameBoard[i_DestCell].CellRow - m_GameBoard[i_OriginCell].CellRow) != Math.Abs(m_GameBoard[i_DestCell].CellCol - m_GameBoard[i_OriginCell].CellCol))
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else
            {
                switch (m_GameBoard[i_OriginCell].CellSign)
                {
                    case eSign.O:
                        moveIsLegal = checkingOplayerCellsLegal(i_OriginCell, i_DestCell);
                        break;
                    case eSign.X:
                        moveIsLegal = checkingXplayerCellsLegal(i_OriginCell, i_DestCell);
                        break;
                    case eSign.K:
                        goto kingSignCheck;
                    case eSign.U:
                    kingSignCheck:
                    moveIsLegal = checkingKingPlayerCellsLegal(i_OriginCell, i_DestCell, m_GameBoard[i_OriginCell].CellSign);
                        break;
                }
            }

            return moveIsLegal;
        }

        private bool isPossibleToEat(Cell i_TheCellInTheMiddle, Cell i_OriginCell, bool i_IsKing)
        {
            bool isPossible = false;
            if (m_GameBoard[i_TheCellInTheMiddle].CellSign != eSign.Empty)
            {
                if (!i_IsKing && m_GameBoard[i_TheCellInTheMiddle].CellSign != m_GameBoard[i_OriginCell].CellSign && m_GameBoard[i_TheCellInTheMiddle].CellSign != getKingsAlterSign(m_GameBoard[i_OriginCell].CellSign))
                {
                    isPossible = true;
                }
                else if(i_IsKing && m_GameBoard[i_TheCellInTheMiddle].CellSign != m_GameBoard[i_OriginCell].CellSign &&
                        m_GameBoard[i_TheCellInTheMiddle].CellSign != getKingsAlterSign(m_GameBoard[i_OriginCell].CellSign))
                {
                    isPossible = true;
                }
            }

            return isPossible;
        }

        private eSign getKingsAlterSign(eSign i_KingSign)
        {
            eSign alterSign = eSign.Empty;
            switch(i_KingSign)
            {
                case eSign.U:
                    alterSign = eSign.O;
                    break;
                case eSign.K:
                    alterSign = eSign.X;
                    break;
                case eSign.O:
                    alterSign = eSign.U;
                    break;
                case eSign.X:
                    alterSign = eSign.K;
                    break;
            }

            return alterSign;
        }

        private bool checkingKingPlayerCellsLegal(Cell i_OriginCell, Cell i_DestCell, eSign i_CellSign)
        {
            bool isLegal = true;
            bool isKing = true;
            int middleRow = (i_OriginCell.CellRow + i_DestCell.CellRow) / 2;
            int middleCol = (i_OriginCell.CellCol + i_DestCell.CellCol) / 2;
            if (Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow) > 2 || (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) > 2))
            {
                isLegal = false;
            }
            else if(Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow) == 2 && (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) == 2))
            {
                if (!isPossibleToEat(m_GameBoard.PlayBoard[middleRow, middleCol], i_OriginCell, isKing)) 
                {
                    isLegal = false;
                }
            }

            return isLegal;
        }

        private bool checkingOplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLegal = true;
            bool isKing = false;
            if (i_OriginCell.CellRow >= i_DestCell.CellRow || i_OriginCell.CellCol == i_DestCell.CellCol)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow < i_DestCell.CellRow - 2 || (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) > 2))
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol - 2)
            {
                if (!isPossibleToEat(m_GameBoard.PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol + 1], i_OriginCell, isKing))
                {
                    isLegal = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol + 2)
            {
                if (!isPossibleToEat(m_GameBoard.PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol - 1], i_OriginCell, isKing))
                {
                    isLegal = false;
                }             
            }

            return isLegal;
        }

        private bool checkingXplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLegal = true;
            bool isKing = false;

            if (i_OriginCell.CellRow <= i_DestCell.CellRow || i_OriginCell.CellCol == i_DestCell.CellCol)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || (i_DestCell.CellCol - 2 > i_OriginCell.CellCol && i_OriginCell.CellCol > i_DestCell.CellCol + 2))
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow + 2 && (i_OriginCell.CellCol == i_DestCell.CellCol - 2))
            {
                if (!isPossibleToEat(m_GameBoard.PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol + 1], i_OriginCell, isKing))
                {
                    isLegal = false;
                }         
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow + 2 && (i_OriginCell.CellCol == i_DestCell.CellCol + 2))
            {
                if (!isPossibleToEat(m_GameBoard.PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol - 1], i_OriginCell, isKing))
                {
                    isLegal = false;
                }
            }
            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) > 2))
            {
                isLegal = false;
            }

            return isLegal;
        }

        public void MakeMoveOnBoard(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex, out bool o_IsKingFlag, out bool o_DidEat)
        {
            int middleRow;
            int middleCol;
            o_DidEat = false;
            eSign kingSign;
            o_IsKingFlag = false;
            if (checkIfBecomingKing(i_DestCell, i_PlayerIndex, out kingSign))
            {
                o_IsKingFlag = true;
                i_DestCell.CellSign = kingSign;
            }
            else
            {
                i_DestCell.CellSign = m_GameBoard[i_OriginCell].CellSign;
            }

            if (Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow) == 2)
            {
                middleRow = (i_OriginCell.CellRow + i_DestCell.CellRow) / 2;
                middleCol = (i_OriginCell.CellCol + i_DestCell.CellCol) / 2;
                if (m_Players[i_PlayerIndex].Sign == eSign.O || m_Players[i_PlayerIndex].Sign == eSign.X)
                {
                    m_GameBoard[(byte)middleRow, (byte)middleCol].CellSign = eSign.Empty;
                }

                o_DidEat = true;
            }

            m_GameBoard[i_OriginCell] = i_OriginCell;
            m_GameBoard[i_OriginCell].CellSign = eSign.Empty;
            m_GameBoard[i_DestCell] = i_DestCell;

            CellsChosenEventArgs e =
                new CellsChosenEventArgs
                {
                    m_DestCell = i_DestCell,
                    m_DestCellSign = i_DestCell.CellSign,
                };

            OnCellChosen(e);
        }

        protected virtual void OnCellChosen(CellsChosenEventArgs e)
        {
            this.CellChosen?.Invoke(this, e);
        }

        public bool CheckDoubleEatingMove(Cell newCellAfterFirstEating, int i_playerIndex)
        {
            List<Player.PlayerMovelist> DoubleEatingList = new List<Player.PlayerMovelist>();
            DoubleEatingList.Clear();

            bool o_DidEat = true;
            bool isDoubleEatingPossible = false;
            UpdateAllOptionalCellMove(i_playerIndex, m_Players[i_playerIndex].Sign, ref o_DidEat);

            foreach (Player.PlayerMovelist optionalCell in m_Players[i_playerIndex].PlayerPotentialMoveslist)
            {
                if (optionalCell.originalCell.CellRow == newCellAfterFirstEating.CellRow && optionalCell.originalCell.CellCol == newCellAfterFirstEating.CellCol)
                {
                    if (Math.Abs(optionalCell.desiredCell.CellRow - newCellAfterFirstEating.CellRow) == 2)
                    {
                        isDoubleEatingPossible = true;
                        DoubleEatingList.Add(new Player.PlayerMovelist() { originalCell = newCellAfterFirstEating, desiredCell = optionalCell.desiredCell });
                    }
                }
            }

            if (DoubleEatingList.Count != 0)
            {
                m_Players[i_playerIndex].PlayerPotentialMoveslist = DoubleEatingList;
            }

            return isDoubleEatingPossible;
        }

        public void UpdatePlayerTokens(int i_PlayerIndex, bool i_DidEat, bool i_IsKing)
        {
            if (i_DidEat)
            {
                m_Players[GetOtherPlayerIndex(i_PlayerIndex)].NumOfTokens -= 1;
            }

            if (i_IsKing)
            {
                m_Players[i_PlayerIndex].NumOfTokens += 3;
            }
        }

        public int GetOtherPlayerIndex(int i_CurrPlayerIndex)
        {
            return (i_CurrPlayerIndex + 1) % m_Players.Length;
        }

        public void UpdatePlayersScore(int i_WinnerIdx)
        {
            int otherPlayerIdx = GetOtherPlayerIndex(i_WinnerIdx);
            for (int playerIndex = 0; playerIndex < m_Players.Length; playerIndex++)
            {
                m_Players[playerIndex].NumOfTokens = 0;
                foreach(Cell cell in m_GameBoard.PlayBoard)
                {
                    if(cell.CellSign == m_Players[playerIndex].Sign)
                    {
                        m_Players[playerIndex].NumOfTokens += 1;
                    }
                    else if(cell.CellSign == getKingsAlterSign(m_Players[playerIndex].Sign))
                    {
                        m_Players[playerIndex].NumOfTokens += 4;
                    }
                }
            }

            m_Players[i_WinnerIdx].Score += m_Players[i_WinnerIdx].NumOfTokens - m_Players[otherPlayerIdx].NumOfTokens;
        }

        public int GetWinnerOfAllGamesIndex()
        {
            int winnerIndex = 0;
            int maxScore = 0;
            for (int playerIndex = 0; playerIndex < m_Players.Length; playerIndex++)
            {
                if (m_Players[playerIndex].Score > maxScore)
                {
                    winnerIndex = playerIndex;
                }
            }

            return winnerIndex;
        }

        private bool checkIfBecomingKing(Cell i_DestCell, int i_PlayerIndex, out eSign o_Sign)
        {
            bool isBecomingKingFlag = false;
            o_Sign = eSign.Empty;
            if ((m_Players[i_PlayerIndex].Sign == eSign.O) && (i_DestCell.CellRow == m_BoardSize - 1))
            {
                o_Sign = eSign.U;
                isBecomingKingFlag = true;
            }
            else if ((m_Players[i_PlayerIndex].Sign == eSign.X) && (i_DestCell.CellRow == 0))
            {
                o_Sign = eSign.K;
                isBecomingKingFlag = true;
            }

            return isBecomingKingFlag;
        }

        public bool CheckIfGameOver(Cell i_RequestedCell, int i_PlayerIndex)
        {
            bool didEat = false;
            bool gameOverFlag = false;
            int otherPlayerIndex = GetOtherPlayerIndex(i_PlayerIndex);
            UpdateAllOptionalCellMove(otherPlayerIndex, m_Players[otherPlayerIndex].Sign, ref didEat);
            UpdateAllOptionalCellMove(i_PlayerIndex, m_Players[i_PlayerIndex].Sign, ref didEat);

            if (m_Players[otherPlayerIndex].NumOfTokens == 0)
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.WINNER;
            }
            else if ((m_Players[otherPlayerIndex].PlayerPotentialMoveslist.Count == 0) && (m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Count == 0))
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.TIE;
            }
            else if (m_Players[otherPlayerIndex].PlayerPotentialMoveslist.Count == 0)
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.WINNER;
            }

            return gameOverFlag;
        }

        public void SetComputerMove(int i_PlayerIndex, out Cell o_legalOriginCell, out Cell o_legalDesiredCell, ref bool o_Dideat)
        {
            Random rndNumber = new Random();
            int playerMove = rndNumber.Next(0, m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Count);
            o_legalOriginCell = m_Players[i_PlayerIndex].PlayerPotentialMoveslist[playerMove].originalCell;
            o_legalDesiredCell = m_Players[i_PlayerIndex].PlayerPotentialMoveslist[playerMove].desiredCell;
        }

        public bool CheckIfCellsInThePossibleList(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex)
        {
            bool isLegal = false;
            foreach (Player.PlayerMovelist checkingCells in m_Players[i_PlayerIndex].PlayerPotentialMoveslist)
            {
                if (checkingCells.originalCell.CellCol == i_OriginCell.CellCol && checkingCells.originalCell.CellRow == i_OriginCell.CellRow && checkingCells.desiredCell.CellCol == i_DestCell.CellCol && checkingCells.desiredCell.CellRow == i_DestCell.CellRow)
                {
                    isLegal = true;
                    break;
                }
            }

            return isLegal;
        }

        public bool IsPlayerEligToQuit(int i_PlayerIndex)
        {
            bool isEligToQuit = false;
            int otherPlayerIdx = GetOtherPlayerIndex(i_PlayerIndex);
            int playerCurrScore;
            int otherPlayerCurrScore;

            playerCurrScore = m_Players[i_PlayerIndex].Score + m_Players[i_PlayerIndex].NumOfTokens;
            otherPlayerCurrScore = m_Players[otherPlayerIdx].Score + m_Players[otherPlayerIdx].NumOfTokens;
            if(playerCurrScore <= otherPlayerCurrScore)
            {
                isEligToQuit = true;
            }

            return isEligToQuit;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Ex04.Damka.Logic;
namespace Ex04.Damka.FormUI
{
    public class FormGameSettings : Form
    {       
        private readonly TextBox m_FirstPlayerNameText = new TextBox();
        private readonly TextBox m_SecPlayerNameText = new TextBox();
        private readonly Label m_BoardSizeLabel = new Label();
        private readonly Label m_FirstPlayerLabel = new Label();
        private readonly Label m_SecPlayerLabel = new Label();
        private readonly Label m_PlayersLabel = new Label();
        private readonly CheckBox m_SecPlayerCheckBox = new CheckBox();
        private readonly Button m_DoneButton = new Button();
        private readonly RadioButton m_6X6Size = new RadioButton();
        private readonly RadioButton m_8X8Size = new RadioButton();
        private readonly RadioButton m_10X10Size = new RadioButton();
        private bool m_IsSecondPlayerComputer = true;
        private byte m_BoardSize;
        private GameLogic m_GameLogic;
        private FormDamkaBoard m_FormBoard;

        public FormGameSettings()
        {
            BackColor = Color.LightBlue;
            Size = new Size(300, 300);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game Settings";
        }

        public GameLogic Logic
        {
            get => m_GameLogic;
            set => m_GameLogic = value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initControls();
        }

        private void initControls()
        {
            m_BoardSizeLabel.Text = "Board Size:";
            m_BoardSizeLabel.Location = new Point(10, 10);

            m_6X6Size.Text = "6X6";
            m_6X6Size.Location = new Point(15, 30);
            m_8X8Size.Text = "8X8";
            m_8X8Size.Location = new Point(120, 30);
            m_10X10Size.Text = "10X10";
            m_10X10Size.Location = new Point(230, 30);

            m_FirstPlayerLabel.Text = "Player 1:";
            m_FirstPlayerLabel.Location = new Point(20, 120);
            m_FirstPlayerNameText.Location = new Point(180, 120);

            m_SecPlayerLabel.Text = "Player 2:";
            m_SecPlayerLabel.Location = new Point(25, 170);
            m_SecPlayerNameText.Location = new Point(180, 170);
            m_SecPlayerNameText.Enabled = false;
            m_SecPlayerNameText.Text = "[Computer]";

            m_PlayersLabel.Text = "Players:";
            m_PlayersLabel.Location = new Point(10, 75);          

            m_SecPlayerCheckBox.Location = new Point(10, 165);

            m_DoneButton.Text = "Done";
            m_DoneButton.Location = new Point(200, 220);

            Controls.AddRange(new Control[] { m_PlayersLabel, m_SecPlayerNameText, m_FirstPlayerNameText, m_BoardSizeLabel, m_FirstPlayerLabel, m_SecPlayerLabel, m_DoneButton, m_6X6Size, m_8X8Size, m_10X10Size, m_SecPlayerCheckBox });

            m_SecPlayerCheckBox.Click += new EventHandler(checkBoxButton_Click);
            m_DoneButton.Click += new EventHandler(doneButton_Click);
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if(checkSettingsAreValid())
            {
                initializeGameLogicAndGameForm();
                m_FormBoard.ShowDialog();
                Close();
            }
        }

        private void initializeGameLogicAndGameForm()
        {
            Player playerOne = new Player(ePlayerType.Human, eSign.O, m_FirstPlayerLabel.Text);
            Player playerTwo = new Player(m_IsSecondPlayerComputer ? ePlayerType.Computer : ePlayerType.Human, eSign.X, m_SecPlayerNameText.Text);
            Player[] players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;

            m_GameLogic = new GameLogic(players, m_BoardSize, m_IsSecondPlayerComputer ? eGameType.HumanVsComputer : eGameType.HumanVsHuman);
            m_FormBoard = new FormDamkaBoard(m_BoardSize, m_GameLogic);

        }

        private bool checkSettingsAreValid()
        {
            bool settingsValid = false;
            if (!m_6X6Size.Checked && !m_8X8Size.Checked && !m_10X10Size.Checked)
            {
                const string message = "You must Choose Board Size!";
                MessageBox.Show(message);
            }
            else if (string.IsNullOrEmpty(m_FirstPlayerNameText.Text))
            {
                const string message = "Player 1 Name can't be empty";
                MessageBox.Show(message);
            }

            else if (m_SecPlayerCheckBox.Checked && string.IsNullOrEmpty(m_SecPlayerNameText.Text))
            {
                const string message = "Player 2 Name can't be empty";
                MessageBox.Show(message);
            }
            else
            {
                if (m_6X6Size.Checked)
                {
                    m_BoardSize = 6;
                }
                else if (m_8X8Size.Checked)
                {
                    m_BoardSize = 8;
                }
                else
                {
                    m_BoardSize = 10;
                }

                if (m_SecPlayerCheckBox.Checked)
                {
                    m_IsSecondPlayerComputer = false;
                }
                settingsValid = true;
            }

            return settingsValid;
        }

        private void checkBoxButton_Click(object sender, EventArgs e)
        {
            if (m_SecPlayerCheckBox.Checked)
            {
                m_SecPlayerNameText.Enabled = true;
            }
            else
            {
                m_SecPlayerNameText.Enabled = false;
            }
        }

        public string FirstPlayerName
        {
            get { return m_FirstPlayerNameText.Text; }
            set { m_FirstPlayerNameText.Text = value; }
        }

        public string SecPlayerName
        {
            get { return m_SecPlayerNameText.Text; }
            set { m_SecPlayerNameText.Text = value; }
        }

        public byte BoardSize { get => m_BoardSize; }

    }
}

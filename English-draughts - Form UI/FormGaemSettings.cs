using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex04.Damka.FormUI
{
    public class FormGaemSettings : Form
    {       
        private readonly TextBox m_PlayerOneNameText = new TextBox();
        private readonly TextBox m_PlayerTwoNameText = new TextBox();
        private readonly Label m_BoarsSize = new Label();
        private readonly Label m_PlayerOneLabel = new Label();
        private readonly Label m_PlayerTwoLabel = new Label();
        private readonly Label m_PlayersLabel = new Label();
        private readonly CheckBox m_PlayerTwoCheckBox = new CheckBox();
        private readonly Button m_Done = new Button();
        private readonly RadioButton m_6X6Size = new RadioButton();
        private readonly RadioButton m_8X8Size = new RadioButton();
        private readonly RadioButton m_10X10Size = new RadioButton();
        private bool isSecondPlayerComputer = true;

        public FormGaemSettings()
        {
            BackColor = Color.LightBlue;
            Size = new Size(300, 300);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gaem Settings";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitControls();
        }

        private void InitControls()
        {
            m_BoarsSize.Text = "Board Size:";
            m_BoarsSize.Location = new Point(10, 10);

            m_6X6Size.Text = "6X6";
            m_6X6Size.Location = new Point(15, 30);
            m_8X8Size.Text = "8X8";
            m_8X8Size.Location = new Point(120, 30);
            m_10X10Size.Text = "10X10";
            m_10X10Size.Location = new Point(230, 30);

            m_PlayerOneLabel.Text = "Player 1:";
            m_PlayerOneLabel.Location = new Point(20, 120);
            m_PlayerOneNameText.Location = new Point(180, 120);

            m_PlayerTwoLabel.Text = "Player 2:";
            m_PlayerTwoLabel.Location = new Point(25, 170);
            m_PlayerTwoNameText.Location = new Point(180, 170);
            m_PlayerTwoNameText.Enabled = false;
            m_PlayerTwoNameText.Text = "[Computer]";

            m_PlayersLabel.Text = "Players:";
            m_PlayersLabel.Location = new Point(10, 75);          

            m_PlayerTwoCheckBox.Location = new Point(10, 165);

            m_Done.Text = "Done";
            m_Done.Location = new Point(200, 220);

            Controls.AddRange(new Control[] { m_PlayersLabel, m_PlayerTwoNameText, m_PlayerOneNameText, m_BoarsSize, m_PlayerOneLabel, m_PlayerTwoLabel, m_Done, m_6X6Size, m_8X8Size, m_10X10Size, m_PlayerTwoCheckBox });

            m_PlayerTwoCheckBox.Click += new EventHandler(cheackBoxbutton_Click);
            m_Done.Click += new EventHandler(Done_Click);
        }

        private void Done_Click(object sender, EventArgs e)
        {
            if (!m_6X6Size.Checked && !m_8X8Size.Checked && !m_10X10Size.Checked)
            {
                const string message = "You must Choose Board Size!";
                MessageBox.Show(message);
            }

            if (string.IsNullOrEmpty(m_PlayerOneNameText.Text))
            {
                const string message = "Player 1 Name can't be empty";
                MessageBox.Show(message);
            }

            if (m_PlayerTwoCheckBox.Checked && string.IsNullOrEmpty(m_PlayerTwoNameText.Text))
            {
                const string message = "Player 2 Name can't be empty";
                MessageBox.Show(message);
            }
            else
            {
                if (m_6X6Size.Checked)
                {
                    BoardSize = 6;
                }
                else if (m_8X8Size.Checked)
                {
                    BoardSize = 8;
                }
                else
                {
                    BoardSize = 10;
                }

                if (m_PlayerTwoCheckBox.Checked)
                {
                    isSecondPlayerComputer = false;
                }

                Close();
                createFormBoard();
            }
        }

        private void cheackBoxbutton_Click(object sender, EventArgs e)
        {
            if (m_PlayerTwoCheckBox.Checked)
            {
                m_PlayerTwoNameText.Enabled = true;
            }
            else
            {
                m_PlayerTwoNameText.Enabled = false;
            }
        }

        public string PlayerOneName
        {
            get { return m_PlayerOneNameText.Text; }
            set { m_PlayerOneNameText.Text = value; }
        }

        public string PlayerTwoName
        {
            get { return m_PlayerTwoNameText.Text; }
            set { m_PlayerTwoNameText.Text = value; }
        }

        public byte BoardSize { get; set; }

        private void createFormBoard()
        {           
            FormDamkaBoard newFormBoard = new FormDamkaBoard(PlayerOneName, PlayerTwoName, isSecondPlayerComputer, BoardSize);
            newFormBoard.ShowDialog();
        }
    }
}

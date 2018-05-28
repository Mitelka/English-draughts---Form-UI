using System;
using System.Windows.Forms;
using System.Drawing;

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

        public FormGameSettings()
        {
            BackColor = Color.LightBlue;
            Size = new Size(300, 300);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game Settings";
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
            m_DoneButton.Click += new EventHandler(done_Click);
        }

        private void done_Click(object sender, EventArgs e)
        {
            if (!m_6X6Size.Checked && !m_8X8Size.Checked && !m_10X10Size.Checked)
            {
                const string message = "You must Choose Board Size!";
                MessageBox.Show(message);
            }

            if (string.IsNullOrEmpty(m_FirstPlayerNameText.Text))
            {
                const string message = "Player 1 Name can't be empty";
                MessageBox.Show(message);
            }

            if (m_SecPlayerCheckBox.Checked && string.IsNullOrEmpty(m_SecPlayerNameText.Text))
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

                if (m_SecPlayerCheckBox.Checked)
                {
                    m_IsSecondPlayerComputer = false;
                }

                Close();
                createBoardForm();
            }
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

        public byte BoardSize { get; set; }

        private void createBoardForm()
        {           
            FormDamkaBoard newFormBoard = new FormDamkaBoard(m_FirstPlayerNameText.Text, m_SecPlayerNameText.Text, m_SecPlayerCheckBox.Checked, BoardSize);
            newFormBoard.ShowDialog();
        }
    }
}

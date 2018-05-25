using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex04.Damka.FormUI
{
    public class FormGaemSettings : Form
    {       
        private TextBox m_PlayerOneNameText = new TextBox();
        private TextBox m_PlayerTwoNameText = new TextBox();
        private Label m_BoarsSize = new Label();
        private Label m_PlayerOneLabel = new Label();
        private Label m_PlayerTwoLabel = new Label();
        private Label m_PlayersLabel = new Label();
        private CheckBox m_PlayerTwoCheckBox = new CheckBox();
        private Button m_Done = new Button();
        private RadioButton m_6X6Size = new RadioButton();
        private RadioButton m_8X8Size = new RadioButton();
        private RadioButton m_10X10Size = new RadioButton();

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

            m_PlayersLabel.Text = "Players:";
            m_PlayersLabel.Location = new Point(10, 75);          

            m_PlayerTwoCheckBox.Location = new Point(10, 165);

            m_Done.Text = "Done";
            m_Done.Location = new Point(200, 220);

            Controls.AddRange(new Control[] { m_PlayersLabel, m_PlayerTwoNameText, m_PlayerOneNameText, m_BoarsSize, m_PlayerOneLabel, m_PlayerTwoLabel, m_Done, m_6X6Size, m_8X8Size, m_10X10Size, m_PlayerTwoCheckBox });

            // this.m_ButtonCancel.Click += new EventHandler(m_ButtonCancel_Click);
            // this.m_ButtonOK.Click += new EventHandler(m_ButtonOK_Click);
        }
    }
}

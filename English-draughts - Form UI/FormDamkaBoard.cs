using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Ex04.Damka.FormUI
{
    public class FormDamkaBoard : Form
    {
        public FormDamkaBoard()
        {
            BackColor = Color.LightGray;
            Size = new Size(300, 300);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Damka";
        }
    }
}

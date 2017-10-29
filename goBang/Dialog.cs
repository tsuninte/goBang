using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goBang
{
    public partial class Dialog : Form
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void Dialog_Load(object sender, EventArgs e)
        {

        }

        private void confirmButton_Click(object sender, EventArgs e)
        {

            this.Close();
        }
        
        public void showWinner(bool blackOrWhite) //? false for black and true for white
        {
            if (blackOrWhite)
                textBox.Text = "白棋胜利！";
            else
                textBox.Text = "黑棋胜利！";
            ShowDialog();
        }
    }
}

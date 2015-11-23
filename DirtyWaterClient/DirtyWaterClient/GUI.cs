using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirtyWaterClient
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void RegisterButton_MouseUp(object sender, MouseEventArgs e)
        {
            //Commence Register here
        }

        private void LoginButton_MouseUp(object sender, MouseEventArgs e)
        {
            //Commence Login here
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RegisterPageButton_MouseEnter(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.White; 
        }

        private void RegisterPageButton_MouseLeave(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.Lime;
        }

        private void LoginButton_MouseEnter(object sender, EventArgs e)
        {
            LoginButton.ForeColor = Color.FromArgb(32, 32, 32);
        }

        private void LoginButton_MouseLeave(object sender, EventArgs e)
        {
            LoginButton.ForeColor = Color.DimGray;
        }

        private void RegisterPageButton_MouseUp(object sender, MouseEventArgs e)
        {
            LoginPane.Visible = false;
            RegisterPanel.Visible = true;
        }

        private void LoginPageButton_MouseEnter(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.White;
        }

        private void LoginPageButton_MouseLeave(object sender, EventArgs e)
        {
            RegisterPageButton.ForeColor = Color.Lime;
        }

        private void LoginPageButton_MouseUp(object sender, MouseEventArgs e)
        {
            RegisterPanel.Visible = false;
            LoginPane.Visible = true;
        }

        private void RegisterButton_MouseEnter(object sender, EventArgs e)
        {
            LoginButton.ForeColor = Color.FromArgb(32, 32, 32);
        }

        private void RegisterButton_MouseLeave(object sender, EventArgs e)
        {
            LoginButton.ForeColor = Color.DimGray;
        }


    }
}

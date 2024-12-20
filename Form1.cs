﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advanced_Calculator
{
    public partial class Form1 : Form
    {
        decimal fstNum, secNum = 0.0m;
        string result;
        string operation = "";

        [DllImport("dwmapi", PreserveSig = false)]
        static extern void DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, in bool pvAttribute, int cbAttribute);

        protected override void OnHandleCreated(EventArgs e)
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, true, Marshal.SizeOf<bool>());
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnDot_Click(object sender, EventArgs e)
        {
            if (!txtBox.Text.Contains("."))
            {
                txtBox.Text += ".";
            }
        }

        private void BtnNums_Click(object sender, EventArgs e)
        {
            if (txtBox.Text == result)
                txtBox.Text = "0";

            btnPlus.Enabled = true;
            btnMinus.Enabled = true;
            btnMultiply.Enabled = true;
            btnDivide.Enabled = true;

            Button button = (Button)sender;
            if (txtBox.Text == "0")
            {
                txtBox.Text = button.Text;
            }
            else
            {
                txtBox.Text += button.Text; //txtBox.Text += button.Text ==> txtBox.Text = txtBox.Text + button.Text
            }
        }

        private void Operation_Click(object sender, EventArgs e)
        {
            if (txtBox.Text == "Infinity" || txtBox.Text == "∞")
            {
                txtBox.Text = "0";
                MessageBox.Show("Can not operate with infinity");
                btnC.PerformClick();
            }
            else
            {
                try
                {
                    btnEquals.PerformClick();

                    Button button = (Button)sender;
                    fstNum = decimal.Parse(txtBox.Text);
                    operation = button.Text;
                    txtBox.Text = "0";

                    txtDis.Text = fstNum.ToString() + " " + operation;

                    btnPlus.Enabled = false;
                    btnMinus.Enabled = false;
                    btnMultiply.Enabled = false;
                    btnDivide.Enabled = false;
                }
                catch (System.OverflowException)
                {
                    MessageBox.Show("The Number Is Too Long.");
                }
            }
        }

        private void BtnEquals_Click(object sender, EventArgs e)
        {
            secNum = decimal.Parse(txtBox.Text);
            txtDis.Text = $"{txtDis.Text} {txtBox.Text} =";

            btnPlus.Enabled = true;
            btnMinus.Enabled = true;
            btnMultiply.Enabled = true;
            btnDivide.Enabled = true;

            try
            {
                switch (operation)
                {
                    case "+":
                        result = txtBox.Text = (fstNum + secNum).ToString();
                        break;
                    case "−":
                        result = txtBox.Text = (fstNum - secNum).ToString();
                        break;
                    case "×":
                        result = txtBox.Text = (fstNum * secNum).ToString();
                        break;
                    case "÷":
                        try
                        {
                            result = txtBox.Text = (fstNum / secNum).ToString();
                        }
                        catch (System.DivideByZeroException)
                        {
                            result = txtBox.Text = "Infinity";
                        }
                        break;

                    default:
                        txtDis.Text = $"{txtBox.Text} =";
                        break;
                }
            }
            catch (System.OverflowException)
            {
                MessageBox.Show("The Number Is Too Long.");
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (txtBox.Text.Length > 0)
            {
                txtBox.Text = txtBox.Text.Remove(txtBox.Text.Length - 1, 1);
            }
            if (txtBox.Text == "")
                txtBox.Text = txtBox.Text = "0";
        }

        private void BtnCE_Click(object sender, EventArgs e)
        {
            txtBox.Text = "0";
        }

        private void BtnOperation_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            operation = button.Text;
            switch (operation)
            {
                case "√x":
                    txtDis.Text = $"√({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(txtBox.Text)));
                    break;
                case "^2":
                    txtDis.Text = $"({txtBox.Text})^2";
                    result = txtBox.Text = Convert.ToString(Convert.ToDouble(txtBox.Text) * (Convert.ToDouble(txtBox.Text)));
                    break;
                case "¹/x":
                    txtDis.Text = $"¹/({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(1.0 / Convert.ToDouble(txtBox.Text));
                    break;
                case "%":
                    txtDis.Text = $"%({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(Convert.ToDouble(txtBox.Text) / Convert.ToDouble(100));
                    break;

                default:
                    break;
            }
        }

        private void BtnC_Click(object sender, EventArgs e)
        {
            Point form_pt = new Point(this.Left, this.Top);
            Form1 frm = new Form1
            {
                StartPosition = FormStartPosition.Manual,
                Location = form_pt
            };
            frm.Show();
            this.Hide();
            frm.Closed += (s, args) => this.Close();

        }

        private void BtnMP_Click(object sender, EventArgs e)
        {
            txtBox.Text = Convert.ToString(-1 * double.Parse(txtBox.Text));
        }
    }
}

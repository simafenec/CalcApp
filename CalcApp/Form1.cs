using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCalculationApp
{
    public partial class Form1 : Form
    {
        const string AppName = "計算機";
        CALC.KEISANNKI cal = new CALC.KEISANNKI();//計算機プログラムをインスタンスとして生成
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Key_1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 1;
        }

        private void Key_2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 2;
        }

        private void Key_3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 3;
        }

        private void Key_4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 4;
        }

        private void Key_5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 5;
        }

        private void Key_6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 6;
        }

        private void Key_7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 7;
        }

        private void Key_8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 8;
        }

        private void Key_9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 9;
        }

        private void Key_leftbrak_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '(';
        }

        private void Key_rightbrak_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += ')';
        }

        private void Key_0_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 0;
        }

        private void Key_dot_Click(object sender, EventArgs e)
        {
            richTextBox1.Text +='.';
        }

        private void Key_plus_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '+';
        }

        private void Key_minus_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '-';
        }
        private void Key_times_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '*';
        }

        private void Key_div_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += '/';
        }

        private void Key_equal_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = cal.Formula_simplified(richTextBox1.Text);
            string[] sp_formula = cal.SplitFormula(richTextBox1.Text);
            //MessageBox.Show(String.Join(",",cal.SplitFormula(richTextBox1.Text)));
            richTextBox1.Text = cal.Calc_new(sp_formula);
        }

        private void Key_clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void Key_delete_Click(object sender, EventArgs e)
        {
            String str = richTextBox1.Text;
            if (str.Length >= 1) { 
                str = str.Substring(0, str.Length - 1);
                richTextBox1.Text = str;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = AppName;
        }
    }
}

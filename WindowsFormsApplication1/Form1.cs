using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static Font userFont;
        public Form1()
        {
            InitializeComponent();
        }

        private void userFontShow()
        {
            if (userFont.Italic) { button1.FlatStyle = FlatStyle.Flat; } else { button1.FlatStyle = FlatStyle.Standard; }
            if (userFont.Bold) { button2.FlatStyle = FlatStyle.Flat; } else { button2.FlatStyle = FlatStyle.Standard; }
            if (userFont.Underline) { button3.FlatStyle = FlatStyle.Flat; } else { button3.FlatStyle = FlatStyle.Standard; }
            string name = userFont.Name;
            if (name.Length >0)
            {
                if (comboBox1.FindStringExact(name)<0) { comboBox1.Items.Add(name); }
            }
            decimal j = (decimal)((uint)(userFont.Size * 2));
            if (j > numericUpDown1.Maximum) { j = numericUpDown1.Maximum; }
            if (j < numericUpDown1.Minimum) { j = numericUpDown1.Minimum; }
            numericUpDown1.Value = j;
            comboBox1.Text = name;
            textBox2.Text = "字号 " + userFont.Size;

            listBox1.Items[0] = "字体： " + name;
            listBox1.Items[1] = "斜体： " + userFont.Italic;
            listBox1.Items[2] = "粗体： " + userFont.Bold;
            listBox1.Items[3] = "下划线： " + userFont.Underline;
            listBox1.Items[4] = "阴影： " + (button4.FlatStyle == FlatStyle.Flat);
            listBox1.Items[5] = "字号： " + userFont.Size;

            richTextBox1.Font = userFont; 
        }

        private void userFontSetup(float emSize)
        {
            FontStyle iFontStyle = FontStyle.Regular;
            if (button1.FlatStyle == FlatStyle.Flat) { iFontStyle |= FontStyle.Italic; }else { iFontStyle &= ~FontStyle.Italic; }
            if (button2.FlatStyle == FlatStyle.Flat) { iFontStyle |= FontStyle.Bold; } else { iFontStyle &= ~FontStyle.Bold; }
            if (button3.FlatStyle == FlatStyle.Flat) { iFontStyle |= FontStyle.Underline; } else { iFontStyle &= ~FontStyle.Underline; }
            string fontName = comboBox1.Text;
            userFont = new Font(fontName, emSize, iFontStyle);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(comboBox1.SelectedIndex);
            if (comboBox1.Enabled)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    fontDialog1.Font = userFont;
                    if (fontDialog1.ShowDialog() == DialogResult.OK)
                    {
                        userFont = fontDialog1.Font;
                    }
                }
                else
                {
                    userFontSetup(userFont.Size);
                }
                userFontShow();                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            userFont = new Font("宋体", 9F,FontStyle.Regular);
            userFontShow();
            comboBox2.SelectedIndex = 3;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            checkedListBox1.SetItemChecked(0, true);
            checkedListBox1.SetItemChecked(4, true);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown1.Enabled)
            {
                //float emSize = (float)((double)(numericUpDown1.Value) * 0.6343 - 0.3043);
                float emSize = (float)(numericUpDown1.Value) / 2;
                userFontSetup(emSize);
                userFontShow();
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.FlatStyle == FlatStyle.Flat) { button1.FlatStyle = FlatStyle.Standard; }else { button1.FlatStyle = FlatStyle.Flat; }
            userFontSetup(userFont.Size);
            userFontShow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.FlatStyle == FlatStyle.Flat) { button2.FlatStyle = FlatStyle.Standard; } else { button2.FlatStyle = FlatStyle.Flat; }
            userFontSetup(userFont.Size);
            userFontShow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.FlatStyle == FlatStyle.Flat) { button3.FlatStyle = FlatStyle.Standard; } else { button3.FlatStyle = FlatStyle.Flat; }
            userFontSetup(userFont.Size);
            userFontShow();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.FlatStyle == FlatStyle.Flat) { button4.FlatStyle = FlatStyle.Standard; } else { button4.FlatStyle = FlatStyle.Flat; }
            userFontShow();
        }
    }
}

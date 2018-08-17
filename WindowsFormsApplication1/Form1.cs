using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Font userFont;
        public Color[] userColor = new Color[4];
        public Bitmap preView = new Bitmap(79,127);
        public Bitmap colorView = new Bitmap(68,18);
        public SolidBrush[] sb = new SolidBrush[4];
        public int userX=0, userY=0;
        public bool shadowEnable = false;
        public string userStr;
        public int[,] mformat = new int[7, 2]
        {
            {8,8 },
            {8,16 },
            {12,16 },
            {16,16 },
            {16,32 },  
            {32,32 },    
            {64,64 }
        };
        public int[,] NesColorRGB = new int[64, 3]
        {
            //$00-0F
            {123,121,123},
            {33,0,181},
            {41,0,189},
            {99,16,165},
            {156,32,123},
            {181,16,49},
            {165,48,0},
            {123,65,0},
            {74,89,0},
            {57,105,0},
            {57,109,0},
            {49,97,66},
            {49,81,132},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            //$10-1F
            {181,178,181},
            {66,97,255},
            {66,65,255},
            {148,65,247},
            {222,65,198},
            {222,65,99},
            {231,81,0},
            {198,113,0},
            {140,138,0},
            {82,162,0},
            {74,170,16},
            {74,162,107},
            {66,146,198},
            {0,0,0},
            {0,0,0},
            {0,0,0},
            //$20-2F
            {239,235,239},
            {99,162,255},
            {82,130,255},
            {165,113,255},
            {247,97,255},
            {255,97,181},
            {255,121,49},
            {255,162,0},
            {239,211,33},
            {156,235,0},
            {115,243,66},
            {115,227,148},
            {99,211,231},
            {123,121,123},
            {0,0,0},
            {0,0,0},
            //$30-3F
            {255,255,255},
            {148,211,255},
            {165,186,255},
            {198,178,255},
            {231,178,255},
            {255,186,239},
            {255,203,189},
            {255,219,165},
            {255,243,148},
            {206,243,132},
            {165,243,165},
            {165,255,206},
            {165,255,247},
            {165,162,165},
            {0,0,0},
            {0,0,0},
        };
        public long PalFileLenght = -1;
        public string CfgFileName= "default.listCfg", TextFileName= "default.txt";
        public bool isCfgFileLoaded = false, isTextFileLoaded = false;
        public bool isCfgEdited = false, isTextFileEdited = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void DrawColor()
        {
            Graphics g = Graphics.FromImage(colorView);
            sb[0] = new SolidBrush(userColor[0]);
            sb[1] = new SolidBrush(userColor[1]);
            sb[2] = new SolidBrush(userColor[2]);
            sb[3] = new SolidBrush(userColor[3]);
            g.FillRectangle(sb[0], 0, 0, 17, 18);
            g.FillRectangle(sb[1], 17, 0, 17, 18);
            g.FillRectangle(sb[2], 34, 0, 17, 18);
            g.FillRectangle(sb[3], 51, 0, 17, 18);
            pictureBox2.Image = colorView;
            sb[0].Dispose();
            sb[1].Dispose();
            sb[2].Dispose();
            sb[3].Dispose();
            g.Dispose();
        }

        private void ShowPreview(string s)
        {
            Graphics g = Graphics.FromImage(preView);
            Color c = tableLayoutPanel3.BackColor;
            g.Clear(c);
            g.DrawString("预览", DefaultFont, Brushes.Black, 10, 10);

            //Pen pl = new Pen(Color.White, 1);
            int i = comboBox2.SelectedIndex, k;
            int x, y, w, h;
            int[] j = new int[4];
            j[0] = (int)(numericUpDown3.Value);
            j[1] = (int)(numericUpDown4.Value);
            j[2] = (int)(numericUpDown5.Value);
            j[3] = (int)(numericUpDown6.Value);
            if ((i >= 0)
                && (j[0] != j[1]) && (j[0] != j[2]) && (j[0] != j[3])
                && (j[1] != j[2]) && (j[1] != j[3])
                && (j[2] != j[3]))
            {
                w = mformat[i, 0];
                h = mformat[i, 1];
                x = preView.Width / 2 - w / 2;
                y = preView.Height / 2 - h / 2;
                //g.DrawRectangle(pl, 40 - (w + 1) / 2 - 1, 64 - (h + 1) / 2 - 1, w + 2, h + 2);
                sb[0] = new SolidBrush(userColor[j[0]]);
                sb[1] = new SolidBrush(userColor[j[1]]);
                sb[2] = new SolidBrush(userColor[j[2]]);
                sb[3] = new SolidBrush(userColor[j[3]]);
                g.FillRectangle(sb[3], x, y, w, h);
                x += userX;
                y += userY;
                if (shadowEnable)
                {
                    k = (int)numericUpDown8.Value;
                    g.DrawString(s, userFont, sb[2], x + k, y + k);
                }
                g.DrawString(s, userFont, sb[0], x, y);

                sb[0].Dispose();
                sb[1].Dispose();
                sb[2].Dispose();
                sb[3].Dispose();
            }

            //pl.Dispose();
            pictureBox1.Image = preView;
            g.Dispose();
        }

        private void Draw()
        {
            if (userStr == null) { userStr = "预"; }
            if (userStr.Length>0) { ShowPreview(userStr); } else { ShowPreview("预"); }
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
            decimal j = (int)(userFont.Size * 2);
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
            isCfgEdited = true;
            //刷新预览
            Draw();
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

        private void userXYMark()
        {
            listBox1.Items[6] = "偏移X： " + userX;
            listBox1.Items[7] = "偏移Y： " + userY;
            isCfgEdited = true;
        }

        private void userColorMark()
        {
            listBox1.Items[8] = "色彩集： " + textBox1.Text;
            listBox1.Items[9] = "色彩索引： " + numericUpDown2.Value;
            listBox1.Items[10] = "主色： " + numericUpDown3.Value;
            listBox1.Items[11] = "次色： " + numericUpDown4.Value;
            listBox1.Items[12] = "影色： " + numericUpDown5.Value;
            listBox1.Items[13] = "底色： " + numericUpDown6.Value;
            isCfgEdited = true;
        }

        private void userOptionMark()
        {
            bool[] b = new bool[5];
            b[0] = checkedListBox1.GetItemChecked(0);
            b[1] = checkedListBox1.GetItemChecked(1);
            b[2] = checkedListBox1.GetItemChecked(2);
            b[3] = checkedListBox1.GetItemChecked(3);
            b[4] = checkedListBox1.GetItemChecked(4);
            listBox1.Items[14] = "规格： " + comboBox2.SelectedIndex + "; " + comboBox2.Text;
            listBox1.Items[15] = "压缩： " + comboBox3.SelectedIndex + "; " + comboBox3.Text;
            listBox1.Items[16] = "输出方式： " + comboBox4.SelectedIndex + "; " + comboBox4.Text;
            listBox1.Items[17] = "输出字模索引表： " + b[0];
            listBox1.Items[18] = "零号Tile置为空白： " + b[1];
            listBox1.Items[19] = "即见即得： " + b[2];
            listBox1.Items[20] = "加密： " + b[3];
            listBox1.Items[21] = "输出显示范例： " + b[4];
            listBox1.Items[22] = "影深： " + numericUpDown8.Value;
            listBox1.Items[23] = "密码： " + numericUpDown7.Value;
            isCfgEdited = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(comboBox1.SelectedIndex);
            if (comboBox1.Enabled)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    fontDialog1.Font = userFont;
                    if (fontDialog1.ShowDialog() == DialogResult.OK) { userFont = fontDialog1.Font; }
                }
                else { userFontSetup(userFont.Size); }
                userFontShow();                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {   //form star 1
            //载入默认参数
            userFont = new Font("宋体", 9F,FontStyle.Regular);
            userFontShow();
            comboBox2.SelectedIndex = 3;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            checkedListBox1.SetItemChecked(0, true);
            checkedListBox1.SetItemChecked(4, true);
            userColor[0] = Color.Black;
            userColor[1] = Color.White;
            userColor[2] = Color.Yellow;
            userColor[3] = Color.Gray;
            //载入参数
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
        {   //form star 2
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
            if (button4.FlatStyle == FlatStyle.Flat)
            { button4.FlatStyle = FlatStyle.Standard; shadowEnable = false; }
            else
            { button4.FlatStyle = FlatStyle.Flat; shadowEnable = true; }
            userFontShow();
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            int i = richTextBox1.SelectionStart;
            userStr = richTextBox1.Text.Substring(i,1);
            Draw();
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            userX = 0;
            userY = 0;
            isCfgEdited = true;
            userXYMark();
            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            userX -= 1;
            userXYMark();
            Draw();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            userX += 1;
            userXYMark();
            Draw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            userY -= 1;
            userXYMark();
            Draw();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            userY += 1;
            userXYMark();
            Draw();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            userColorMark();
            Draw();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            userOptionMark();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            userOptionMark();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            userOptionMark();
            Draw();
            if (comboBox2.SelectedIndex==2)
            {
                comboBox3.SelectedIndex = 3;
                comboBox3.Enabled = false;
            }
            else { comboBox3.Enabled = true; }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            userOptionMark();
            Draw();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            userOptionMark();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            int j = 0;
            if ((i >= 0) && (i <= 7))
            { j = 0; }
            else if ((i >= 8) && (i <= 13))
            { j = 1; }
            else if ((i >= 14) && (i <= 23))
            { j = 2; }
            tabControl1.SelectedIndex = j;
        }

        private void LoadPalFile()
        {
            openFileDialog1.Filter = "色彩集*.pal,*.col,*.color|*.pal;*.col;*.color|所有文件|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(textBox1.Text);
            openFileDialog1.FileName = "";
            string filepath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
                textBox1.Text = filepath;
                FileInfo fileInfo = null;
                fileInfo = new FileInfo(filepath);
                long k = -1;
                if (fileInfo != null && fileInfo.Exists) { PalFileLenght = fileInfo.Length ; k = PalFileLenght / 4; }
                //fileInfo.CopyTo("C:/test/test_fileInfo.bin");
                if (k > 0)
                {   //色彩集有数据
                    k -= 1;
                    if (k > 255) { k = 255; }
                    numericUpDown2.Maximum = k;
                    numericUpDown2.Value = 0;
                    //读取
                    ReadPal();
                    DrawColor();
                    Draw();
                }
                else
                {   //色彩集为空
                    textBox1.Text = "色彩集为空";
                    numericUpDown2.Value = 0;
                }
                //System.Diagnostics.Debug.WriteLine("Lenght "+ PalFileLenght);
                //System.Diagnostics.Debug.WriteLine("Max "+ numericUpDown2.Maximum);
            }
        }

        private void ReadPal()
        {
            char[] mbuf = new char[1024];
            int k = (int)numericUpDown2.Value * 4;
            int j;
            StreamReader sr = File.OpenText(textBox1.Text);
            //System.Diagnostics.Debug.WriteLine("Value " + numericUpDown2.Value);
            //System.Diagnostics.Debug.WriteLine("k " + k);
            sr.Read(mbuf, 0,1024);
            sr.Close();
            sr.Dispose();

            j = mbuf[k];
            if ((j >= 0) && (j <= 63))
            { userColor[0] = Color.FromArgb(NesColorRGB[j, 0], NesColorRGB[j, 1], NesColorRGB[j, 2]); }
            else
            { userColor[0] = Color.Black; }
            //System.Diagnostics.Debug.WriteLine(j);
            j = mbuf[k+1];
            if ((j >= 0) && (j <= 63))
            { userColor[1] = Color.FromArgb(NesColorRGB[j, 0], NesColorRGB[j, 1], NesColorRGB[j, 2]); }
            else
            { userColor[1] = Color.Black; }
            //System.Diagnostics.Debug.WriteLine(j);
            j = mbuf[k+2];
            if ((j >= 0) && (j <= 63))
            { userColor[2] = Color.FromArgb(NesColorRGB[j, 0], NesColorRGB[j, 1], NesColorRGB[j, 2]); }
            else
            { userColor[2] = Color.Black; }
            //System.Diagnostics.Debug.WriteLine(j);
            j = mbuf[k+3];
            if ((j >= 0) && (j <= 63))
            { userColor[3] = Color.FromArgb(NesColorRGB[j, 0], NesColorRGB[j, 1], NesColorRGB[j, 2]); }
            else
            { userColor[3] = Color.Black; }
            //System.Diagnostics.Debug.WriteLine(j);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            LoadPalFile();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ReadPal();
            DrawColor();
            Draw();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "文本：" + richTextBox1.Text.Length + "字";
            int i = richTextBox1.SelectionStart;
            if ((richTextBox1.Text.Length > 0)&&(i< richTextBox1.Text.Length)) { userStr = richTextBox1.Text.Substring(i, 1); }
            else { userStr = null; }
            Draw();
            isTextFileEdited = true;
        }

        private void 文本文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文本文档*.txt|*.txt|所有文件|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(TextFileName);
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TextFileName = openFileDialog1.FileName;
                isTextFileLoaded = true;
                richTextBox1.LoadFile(TextFileName, RichTextBoxStreamType.PlainText );
            }
        }

        private void 配置文档ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CfgFileSave();
        }

        private void CfgFileSave()
        {
            if (!isCfgFileLoaded)
            {   //打开保存窗口
                saveFileDialog1.Filter = "配置文档*.listCfg|*.listCfg";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(CfgFileName);
                saveFileDialog1.FileName = CfgFileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    CfgFileName = saveFileDialog1.FileName;
                    isCfgFileLoaded = true;
                    CfgFileSaveTrue();
                    isCfgEdited = false;
                }
            }
            else
            {
                if (isCfgEdited)
                {
                    CfgFileSaveTrue();
                    isCfgEdited = false;

                }
            }
        }

        private void CfgFileSaveTrue()
        {
            int i;
            FileInfo f = new FileInfo(CfgFileName);
            StreamWriter sw = f.CreateText();
            for (i = 0; i < listBox1.Items.Count; i++)
            {
                string s = listBox1.Items[i] as string;
                System.Diagnostics.Debug.WriteLine(s);
                sw.WriteLine(s);
            }
            sw.Close();
            sw.Dispose();
            
        }

        private void 文本文档ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TextFileSave();
        }

        private void TextFileSave()
        {
            if (!isTextFileLoaded)
            {   //打开保存窗口
                saveFileDialog1.Filter = "文本文档*.txt|*.txt";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(TextFileName);
                saveFileDialog1.FileName = TextFileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    TextFileName = saveFileDialog1.FileName;
                    isTextFileLoaded = true;
                    richTextBox1.SaveFile(TextFileName, RichTextBoxStreamType.PlainText );
                    isTextFileEdited = false;
                }
            }
            else
            {
                if(isTextFileEdited)
                {
                    richTextBox1.SaveFile(TextFileName, RichTextBoxStreamType.PlainText);
                    isTextFileEdited = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isCfgEdited = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCfgEdited)
            {
                if(MessageBox.Show("配置参数已改变，是否保存改变？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                { CfgFileSave(); }
            }
            if (isTextFileEdited)
            {
                if(MessageBox.Show("文本已改变，是否保存改变？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                { TextFileSave(); }
            }
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
            Clipboard.Clear();
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void 粘贴数字09ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "0123456789";
            Clipboard.SetDataObject(s, false);
            richTextBox1.Paste();
        }

        private void 粘贴字母azToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "abcdefghijklmnopqrstuvwxyz";
            Clipboard.SetDataObject(s, false);
            richTextBox1.Paste();
        }

        private void 粘贴大字字母AZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Clipboard.SetDataObject(s, false);
            richTextBox1.Paste();
        }

        private void 粘贴ASCII码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = " !\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvxywz{|}~";
            Clipboard.SetDataObject(s, false);
            richTextBox1.Paste();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("名称：NES字模生成工具(NES CHR of Words Builder)\r\n"
                +"版本：v4.0\r\n"
                +"作者：维京猎人\r\n"
                +"2018/08");
        }

        private void 保存所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CfgFileSave();
            TextFileSave();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 配置文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "配置文档*.listCfg|*.listCfg|所有文件|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(CfgFileName);
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CfgFileName = openFileDialog1.FileName;
                StreamReader sr = File.OpenText(CfgFileName);
                isCfgFileLoaded = true;
                listBox1.Items.Clear();
                string s = sr.ReadLine();
                while(s!=null)
                {
                    listBox1.Items.Add(s);
                    s = sr.ReadLine();
                }
                sr.Close();
                sr.Dispose();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {   //form star 3
            DrawColor();
            label1.Text = "文本：" + richTextBox1.Text.Length + "字";
            int i = richTextBox1.SelectionStart;
            userStr = richTextBox1.Text.Substring(i, 1);
            Draw();
            userXYMark();
            userColorMark();
            userOptionMark();
            isCfgEdited = false;
            isTextFileEdited = false;
        }


    }
}

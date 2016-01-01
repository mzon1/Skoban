using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace skoban2
{
    public partial class Form5 : Form//ġƮ��嵵 �������� �Դϴ�. Ʋ�� �κ��� ����Ÿ ����ϴ� ���ϱ��?
    {
        OleDbDataAdapter DBAdapter;
        DataSet DS;
        OleDbCommandBuilder myCommandBuilder;

        const int MAXSTAGE = 3;
        const int BW = 32;
        const int BH = 32;

        StringBuilder[] ns = new StringBuilder[18];
        int i = 0;
        int nStage;
        int nx, ny;
        int nMove;
        Bitmap[] hBit = new Bitmap[5];

        string[,] arStage = {
            {

     "####################",

     "####################",

     "####################",

     "#####   ############",

     "#####O  ############",

     "#####  O############",

     "###  O O ###########",

     "### # ## ###########",

     "#   # ## #####  ..##",

     "# O  O   @      ..##",

     "##### ### # ##  ..##",

     "#####     ##########",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################"

     },

     {

     "####################",

     "####################",

     "####################",

     "####################",

     "####..  #     ######",

     "####..  # O  O  ####",

     "####..  #O####  ####",

     "####..    @ ##  ####",

     "####..  # #  O #####",

     "######### ##O O ####",

     "###### O  O O O ####",

     "######    #     ####",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################"

     },

     {

     "####################",

     "####################",

     "####################",

     "####################",

     "##########     @####",

     "########## O#O #####",

     "########## O  O#####",

     "###########O O #####",

     "########## O # #####",

     "##....  ## O  O  ###",

     "###...    O  O   ###",

     "##....  ############",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################",

     "####################"

     },
        };
        public Form5()
        {
            InitializeComponent();

            try
            {
                string connectionString = "Provider = Microsoft.JET.OLEDB.4.0;" 
                                        + "data source = "
                                        + Application.StartupPath
                                        + @"\Rank.mdb";

                string commandString = "select * from Cheatcode";

                DBAdapter = new OleDbDataAdapter(commandString, connectionString);
                myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DS = new DataSet();
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(900, BH * 18);
            hBit[0] = Properties.Resources.wall;
            hBit[1] = Properties.Resources.pack;
            hBit[2] = Properties.Resources.target;
            hBit[3] = Properties.Resources.empty;
            hBit[4] = Properties.Resources.man;
            try
            {
                DS.Clear();
                DBAdapter.Fill(DS, "Cheatcode");
                DataTable Cheatcodetbl = DS.Tables["Cheatcode"];//����Ÿ ����ϴ� ����� �ռ� �����ϴ�.

                DataColumn[] PrimaryKey = new DataColumn[1];//�÷��� �����ϰ�
                PrimaryKey[0] = Cheatcodetbl.Columns["code"];//�÷��� �����ϰ�
                Cheatcodetbl.PrimaryKey = PrimaryKey;

                DataRow currRow = Cheatcodetbl.Rows.Find(7);//�� ������ 7�̸�(����Ÿ ������ ������ ���� �������ϴ�. �׷��� 7�� ã�� �߽��ϴ�.
                nStage = Convert.ToInt32(currRow["stage"].ToString());//���̺��� ������������ �� ������������ �ֱ�
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
            //������������ ������
            InitStage();//�������� �ʱ� �Լ��� �Ҹ��鼭 ���ϴ� ���������� ��� �˴ϴ�.
        }

        private void Form5_Paint(object sender, PaintEventArgs e)
        {
            int x, y;
            int iBit = 0;

            for (y = 0; y < 18; y++)
            {
                for (x = 0; x < 20; x++)
                {
                    switch (ns[y][x])
                    {
                        case '#':
                            iBit = 0;
                            break;
                        case 'O':
                            iBit = 1;
                            break;
                        case '.':
                            iBit = 2;
                            break;
                        case ' ':
                            iBit = 3;
                            break;
                    }
                    e.Graphics.DrawImage(hBit[iBit], x * BW, y * BH);
                }
            }
            e.Graphics.DrawImage(hBit[4], nx * BW, ny * BH);

            e.Graphics.DrawString("SOKOBAN", Font, Brushes.Black, 700, 10);
            e.Graphics.DrawString("Q:����, R:�ٽ� ����", Font, Brushes.Black, 700, 30);
            e.Graphics.DrawString("��������:" + (nStage + 1), Font, Brushes.Black, 700, 50);
            e.Graphics.DrawString("�̵� Ƚ��:" + nMove, Font, Brushes.Black, 700, 70);
        }

        private void Form5_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    MoveMan(e.KeyCode);
                    if (TestEnd() == true)
                    {
                        MessageBox.Show("���������� Ǯ�����ϴ�!","�˸�");
                        Close();
                    }
                    break;
                case Keys.Q:
                    timer1.Enabled = false;
                    if (MessageBox.Show("������ �׸� �Ͻðڽ��ϱ�?", "�˸�", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Close();
                    }
                    timer1.Enabled = true;
                    break;
                case Keys.R:
                    InitStage();
                    break;
            }
        }

        private void InitStage()
        {
            int x, y;

            for (y = 0; y < 18; y++)
            {
                ns[y] = new StringBuilder(arStage[nStage, y]);
            }

            for (y = 0; y < 18; y++)
            {
                for (x = 0; x < 20; x++)
                {
                    if (ns[y][x] == '@')
                    {
                        nx = x;
                        ny = y;
                        ns[y][x] = ' ';
                    }
                }
            }

            nMove = 0;
            Invalidate();
        }

        private void MoveMan(Keys dir)
        {
            int dx = 0, dy = 0;

            switch (dir)
            {
                case Keys.Left:
                    dx = -1;
                    break;
                case Keys.Right:
                    dx = 1;
                    break;
                case Keys.Up:
                    dy = -1;
                    break;
                case Keys.Down:
                    dy = 1;
                    break;
            }

            if (ns[ny + dy][nx + dx] != '#')
            {
                if (ns[ny + dy][nx + dx] == 'O')
                {
                    if (ns[ny + dy * 2][nx + dx * 2] == ' ' || ns[ny + dy * 2][nx + dx * 2] == '.')
                    {
                        if (arStage[nStage, ny + dy][nx + dx] == '.')
                        {
                            ns[ny + dy][nx + dx] = '.';
                        }
                        else
                        {
                            ns[ny + dy][nx + dx] = ' ';
                        }
                        ns[ny + dy * 2][nx + dx * 2] = 'O';
                    }
                    else
                    {
                        return;
                    }
                }
                nx += dx;
                ny += dy;
                nMove++;
                Invalidate();
            }
        }

        private bool TestEnd()
        {
            int x, y;

            for (y = 0; y < 18; y++)
            {
                for (x = 0; x < 20; x++)
                {
                    if (arStage[nStage, y][x] == '.' && ns[y][x] != 'O')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "�÷��̽ð� : " + i.ToString() + "��";
            i++;
        }
    }
}

       

       

        
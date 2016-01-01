using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb; //����Ÿ ���̽��� ����ϸ� �ҷ��� �ϴ� 

namespace skoban2
{
    public partial class Form3 : Form
    {
        OleDbDataAdapter DBAdapter;//����Ÿ ���̽� ����� ����
        DataSet DS;                 //����Ÿ�� ����
        OleDbCommandBuilder myCommandBuilder;

        const int MAXSTAGE = 3;
        const int BW = 32;
        const int BH = 32;
        string cheatcode;

        StringBuilder[] ns = new StringBuilder[18];
        int nStage;
        int nx, ny;
        int nMove;

        int pStage = 0;
        int i;
        int ptime = 0;
        int pMove = 0;
        int nPoint;
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

        public Form3()
        {
            InitializeComponent();

            try//�̰��� ������ ������ ���� �ڵ��Դϴ�. 
            {
                string connectionString = "Provider = Microsoft.JET.OLEDB.4.0;" //
                                        + "data source = "
                                        + Application.StartupPath//�̰��� �� ������Ʈ ���� �������� ã�� �ٴ� �ǹ�
                                        + @"\Rank.mdb";
                string commandString = "select * from Ranklist";

                DBAdapter = new OleDbDataAdapter(commandString, connectionString);
                myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DS = new DataSet();
            }
            catch (Exception DE)//���� ������ ã�� ���ϸ� ����ó��
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(900, BH * 18); //���� �ε��ؼ� ������ ���
            hBit[0] = Properties.Resources.wall;//������ �׸��� ����ϴ�. �̰��� ���ҽ� �κп� ��Ʈ�� ������ ����ָ� �̸��� ������ ã�Ƽ� ���� ��ŵ�ϴ�.
            hBit[1] = Properties.Resources.pack;
            hBit[2] = Properties.Resources.target;
            hBit[3] = Properties.Resources.empty;
            hBit[4] = Properties.Resources.man;
            nStage = 0;

            InitStage();
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
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
                    e.Graphics.DrawImage(hBit[iBit], x * BW, y * BH);// ���� ������ �׸��� �׸��ϴ�.
                }
            }
            e.Graphics.DrawImage(hBit[4], nx * BW, ny * BH);//��������

            e.Graphics.DrawString("SOKOBAN", Font, Brushes.Black, 700, 10);//���̺� ����� ��ó�� �۾��� �����մϴ�.
            e.Graphics.DrawString("Q:����, R:�ٽ� ����", Font, Brushes.Black, 700, 30);
            e.Graphics.DrawString("��������:" + (nStage + 1), Font, Brushes.Black, 700, 50);
            e.Graphics.DrawString("�̵� Ƚ��:" + nMove, Font, Brushes.Black, 700, 70);
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
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
                        if (nStage < MAXSTAGE - 1)
                        {
                            if (nStage == 0)//Ŭ����� �޼��� �ڽ��� ���� �ݴϴ�. �ڵ带 ���
                            {
                                cheatcode = "jin";
                            }
                            else if (nStage == 1)
                            {
                                cheatcode = "mzon";
                            }
                            MessageBox.Show((nStage + 1) + "���������� Ǯ�����ϴ�! " + "���� ���������� �̵��մϴ�.", "�˸�(" +cheatcode + ")");
                            nStage++;
                            InitStage();
                        }
                        else if (nStage == 2)//������ ���������� Ŭ����� �ڵ� ��ϸ�忡 ����
                        {
                            MessageBox.Show("��� ���������� Ŭ���� �߽��ϴ�. �ڵ� ���� �մϴ�", "�˸�");
                            nPoint = pStage * 1000 - pMove - ptime;
                            try// �̰��� ����Ÿ ���̽��� �����ϴ� ���δ��.
                            {
                                DS.Clear();
                                DBAdapter.Fill(DS, "Ranklist");// ���̺��� ���ڴ� ���
                                DataTable nrank = DS.Tables["Ranklist"]; //���̺� ����
                                DataRow newRow = nrank.NewRow();//�� ����

                                newRow["Point"] = nPoint;//�� ���̺� �´� ���� �ڷ� �Է�
                                newRow["Stage"] = pStage;
                                newRow["Move"] = pMove;
                                

                                nrank.Rows.Add(newRow);// �װ��� ���θ��� ���� �߰�

                                DBAdapter.Update(DS, "Ranklist");//������Ʈ�ϰ�
                                DS.AcceptChanges();
                            }
                            catch (DataException DE)
                            {
                                MessageBox.Show(DE.Message);
                            }
                            catch (Exception DE)
                            {
                                MessageBox.Show(DE.Message);
                            }
                            Close();
                        }
                    }
                    break;
                case Keys.Q:
                    timer1.Enabled = false;// �޼���â�� �߸� Ÿ�̸Ӵ� ����
                    if(MessageBox.Show("���� ������ ���� �÷����� ���������� ��� ���� �ʽ��ϴ�. ��� �Ͻðڽ��ϱ�?", "�˸�", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        nPoint = pStage * 1000 - pMove - ptime;
                        if (nStage > 0)
                        {
                            try// �̰��� �ߵ��� ��������Դϴ�. 0���� ū ������ ù �������� �ߵ������ ���� �ȵǰ�...
                            {
                                DS.Clear();
                                DBAdapter.Fill(DS, "Ranklist");
                                DataTable nrank = DS.Tables["Ranklist"];
                                DataRow newRow = nrank.NewRow();

                                newRow["Point"] = nPoint;
                                newRow["Stage"] = pStage;
                                newRow["Move"] = pMove;
                                

                                nrank.Rows.Add(newRow);

                                DBAdapter.Update(DS, "Ranklist");
                                DS.AcceptChanges();
                            }
                            catch (DataException DE)
                            {
                                MessageBox.Show(DE.Message);
                            }
                            catch (Exception DE)
                            {
                                MessageBox.Show(DE.Message);
                            }
                        }
                        Close();
                    }
                    timer1.Enabled = true;// �ٽ� ����.
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
            timer1.Start();
            i = 0;
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
            timer1.Stop();
            ptime += i;
            pStage++;
            pMove += nMove;
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)//Ÿ�̸Ӵ� �ʴ����� ��� �÷��� �ð��� �ʷ� ǥ��
        {
            toolStripStatusLabel1.Text = "�÷��� �ð� : " + i.ToString() + "��";
            i++;
        }
    }
}
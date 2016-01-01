using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb; //데이타 베이스를 사용하면 불러야 하는 

namespace skoban2
{
    public partial class Form3 : Form
    {
        OleDbDataAdapter DBAdapter;//데이타 베이스 어답터 선언
        DataSet DS;                 //데이타셋 선언
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

            try//이것은 엑세스 파일을 찬는 코드입니다. 
            {
                string connectionString = "Provider = Microsoft.JET.OLEDB.4.0;" //
                                        + "data source = "
                                        + Application.StartupPath//이것은 본 프로젝트 실행 폴더에서 찾는 다는 의미
                                        + @"\Rank.mdb";
                string commandString = "select * from Ranklist";

                DBAdapter = new OleDbDataAdapter(commandString, connectionString);
                myCommandBuilder = new OleDbCommandBuilder(DBAdapter);

                DS = new DataSet();
            }
            catch (Exception DE)//만약 파일을 찾지 못하면 예외처리
            {
                MessageBox.Show(DE.Message);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(900, BH * 18); //폼을 로드해서 사이즈 잡고
            hBit[0] = Properties.Resources.wall;//각각의 그림을 잡습니다. 이것은 리소스 부분에 비트맵 파일을 잡아주면 이름이 같으면 찾아서 구현 시킵니다.
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
                    e.Graphics.DrawImage(hBit[iBit], x * BW, y * BH);// 이제 폼에서 그림을 그립니다.
                }
            }
            e.Graphics.DrawImage(hBit[4], nx * BW, ny * BH);//마찬가지

            e.Graphics.DrawString("SOKOBAN", Font, Brushes.Black, 700, 10);//레이블 사용한 것처럼 글씨를 구현합니다.
            e.Graphics.DrawString("Q:종료, R:다시 시작", Font, Brushes.Black, 700, 30);
            e.Graphics.DrawString("스테이지:" + (nStage + 1), Font, Brushes.Black, 700, 50);
            e.Graphics.DrawString("이동 횟수:" + nMove, Font, Brushes.Black, 700, 70);
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
                            if (nStage == 0)//클리어시 메세지 박스를 보여 줍니다. 코드를 섞어서
                            {
                                cheatcode = "jin";
                            }
                            else if (nStage == 1)
                            {
                                cheatcode = "mzon";
                            }
                            MessageBox.Show((nStage + 1) + "스테이지를 풀었습니다! " + "다음 스테이지로 이동합니다.", "알림(" +cheatcode + ")");
                            nStage++;
                            InitStage();
                        }
                        else if (nStage == 2)//마지막 스테이지는 클리어시 자동 기록모드에 저장
                        {
                            MessageBox.Show("모든 스테이지를 클리어 했습니다. 자동 저장 합니다", "알림");
                            nPoint = pStage * 1000 - pMove - ptime;
                            try// 이것이 데이타 베이스에 저장하는 것인대요.
                            {
                                DS.Clear();
                                DBAdapter.Fill(DS, "Ranklist");// 테이블을 쓰겠다 명시
                                DataTable nrank = DS.Tables["Ranklist"]; //테이블 선언
                                DataRow newRow = nrank.NewRow();//열 선언

                                newRow["Point"] = nPoint;//그 테이블에 맞는 열에 자료 입력
                                newRow["Stage"] = pStage;
                                newRow["Move"] = pMove;
                                

                                nrank.Rows.Add(newRow);// 그것을 새로만든 열에 추가

                                DBAdapter.Update(DS, "Ranklist");//업데이트하고
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
                    timer1.Enabled = false;// 메세지창이 뜨면 타이머는 스톱
                    if(MessageBox.Show("지금 끝내면 현재 플레이한 스테이지는 등록 되지 않습니다. 계속 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        nPoint = pStage * 1000 - pMove - ptime;
                        if (nStage > 0)
                        {
                            try// 이것은 중도에 끝낼경우입니다. 0보다 큰 이유는 첫 스테이지 중도포기는 저장 안되게...
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
                    timer1.Enabled = true;// 다시 가동.
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

        private void timer1_Tick(object sender, EventArgs e)//타이머는 초당으로 잡고 플레이 시간은 초로 표현
        {
            toolStripStatusLabel1.Text = "플레이 시간 : " + i.ToString() + "초";
            i++;
        }
    }
}
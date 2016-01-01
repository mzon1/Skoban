using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace skoban2
{
    public partial class Form1 : Form // 메인 폼 설명
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close(); //이것은 폼을 닫는 종료 버튼 이벤트
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form2 = new Form4(); //폼을 생성해서
            form2.StartPosition = FormStartPosition.CenterScreen;//화면 가운데 위치에 나타나게 하고
            form2.ShowDialog(); //요것은 새로 생성된 폼을 끝내지 않으면 기존 폼을 사용할 수 없도록 하게 하는
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (치트모드ToolStripMenuItem.Checked == true)// 간단히 if문으로 잡았습니다. 즉 툴스트립바에 치트모드가 체크된게 true라면 치트모드를 띄우고
            {                                               //아니면 기록모드를 띄우고
                Form6 form2 = new Form6();
                form2.StartPosition = FormStartPosition.CenterScreen;
                form2.ShowDialog(); 
            }
            else
            {
                Form3 form1 = new Form3();
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.ShowDialog(); 
            }
        }

        private void 게임정보AToolStripMenuItem_Click(object sender, EventArgs e)//정보창 띄우기
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.ShowDialog(); 
        }
    }
}
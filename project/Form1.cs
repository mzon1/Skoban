using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace skoban2
{
    public partial class Form1 : Form // ���� �� ����
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close(); //�̰��� ���� �ݴ� ���� ��ư �̺�Ʈ
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form2 = new Form4(); //���� �����ؼ�
            form2.StartPosition = FormStartPosition.CenterScreen;//ȭ�� ��� ��ġ�� ��Ÿ���� �ϰ�
            form2.ShowDialog(); //����� ���� ������ ���� ������ ������ ���� ���� ����� �� ������ �ϰ� �ϴ�
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ġƮ���ToolStripMenuItem.Checked == true)// ������ if������ ��ҽ��ϴ�. �� ����Ʈ���ٿ� ġƮ��尡 üũ�Ȱ� true��� ġƮ��带 ����
            {                                               //�ƴϸ� ��ϸ�带 ����
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

        private void ��������AToolStripMenuItem_Click(object sender, EventArgs e)//����â ����
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.ShowDialog(); 
        }
    }
}
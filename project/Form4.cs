using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;// ����Ÿ ���̽��� ���

namespace skoban2
{
    public partial class Form4 : Form
    {
        

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)//�� �̰��� �ڵ����� ����� �ڵ�, �̰� �����ϸ� ����Ÿ�׸����� ������������ ��ũ ��Ű�� �ڵ����� �ڵ��˴ϴ�.�� ������ ������ ���� �� �ְ� ���ִ� ������
        {
            // TODO: �� �ڵ�� �����͸� 'rankDataSet.Ranklist' ���̺� �ε��մϴ�. �ʿ��� ��� �� �ڵ带 �̵��ϰų� ������ �� �ֽ��ϴ�.
            this.ranklistTableAdapter.Fill(this.rankDataSet.Ranklist);

            
        }

       
    }
}
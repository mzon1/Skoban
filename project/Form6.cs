using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace skoban2
{
    public partial class Form6 : Form//���⵵ ����Ÿ ���̽� ��� �� �κ��� �����ϱ� �Դϴ�.
    {
        OleDbDataAdapter DBAdapter;
        DataSet DS;
        OleDbCommandBuilder myCommandBuilder;

        public Form6()
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                try
                {
                    DS.Clear();
                    DBAdapter.Fill(DS, "Cheatcode");
                    DataTable Cheatcodetbl = DS.Tables["Cheatcode"];

                    DataColumn[] PrimaryKey = new DataColumn[1];//�ռ��� ���������� �ڵ带 ���
                    PrimaryKey[0] = Cheatcodetbl.Columns["code"];
                    Cheatcodetbl.PrimaryKey = PrimaryKey;

                    DataRow currRow = Cheatcodetbl.Rows.Find(7);//�� �ڵ尪�� ���� ��
                    currRow.BeginEdit();//�����Ҷ� �����ְ�
                    currRow["stage"] = "0";//�̷��� ����
                    currRow.EndEdit();//�ݾ��ְ�

                    DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                    DBAdapter.Update(UpdatedSet, "Cheatcode");//������Ʈ ���ְ�...
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

                Form5 form2 = new Form5();
                this.Visible = false;
                form2.StartPosition = FormStartPosition.CenterScreen;
                form2.ShowDialog(); 
                Close();
            }
            else if (textBox1.Text == "jin")
            {
                try
                {
                    DS.Clear();
                    DBAdapter.Fill(DS, "Cheatcode");
                    DataTable Cheatcodetbl = DS.Tables["Cheatcode"];

                    DataColumn[] PrimaryKey = new DataColumn[1];
                    PrimaryKey[0] = Cheatcodetbl.Columns["code"];
                    Cheatcodetbl.PrimaryKey = PrimaryKey;

                    DataRow currRow = Cheatcodetbl.Rows.Find(7);
                    currRow.BeginEdit();
                    currRow["stage"] = "1";
                    currRow.EndEdit();

                    DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                    DBAdapter.Update(UpdatedSet, "Cheatcode");
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

                Form5 form2 = new Form5();
                this.Visible = false;
                form2.StartPosition = FormStartPosition.CenterScreen;
                form2.ShowDialog(); 
                Close();
            }
            else if (textBox1.Text == "mzon")
            {
                try
                {
                    DS.Clear();
                    DBAdapter.Fill(DS, "Cheatcode");
                    DataTable Cheatcodetbl = DS.Tables["Cheatcode"];

                    DataColumn[] PrimaryKey = new DataColumn[1];
                    PrimaryKey[0] = Cheatcodetbl.Columns["code"];
                    Cheatcodetbl.PrimaryKey = PrimaryKey;

                    DataRow currRow = Cheatcodetbl.Rows.Find(7);
                    currRow.BeginEdit();
                    currRow["stage"] = "2";
                    currRow.EndEdit();

                    DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                    DBAdapter.Update(UpdatedSet, "Cheatcode");
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

                Form5 form2 = new Form5();
                this.Visible = false;
                form2.StartPosition = FormStartPosition.CenterScreen;
                form2.ShowDialog(); 
                Close();
            }
            else
            {
                MessageBox.Show("��Ȯ�� ġƮ�ڵ带 �Է��� �ּ���! ", "�˸�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
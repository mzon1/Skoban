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
    public partial class Form6 : Form//여기도 데이타 베이스 사용 이 부분은 수정하기 입니다.
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

                    DataColumn[] PrimaryKey = new DataColumn[1];//앞서와 마찬가지고 코드를 명시
                    PrimaryKey[0] = Cheatcodetbl.Columns["code"];
                    Cheatcodetbl.PrimaryKey = PrimaryKey;

                    DataRow currRow = Cheatcodetbl.Rows.Find(7);//그 코드값이 참일 때
                    currRow.BeginEdit();//수정할때 열어주고
                    currRow["stage"] = "0";//이렇게 수정
                    currRow.EndEdit();//닫아주고

                    DataSet UpdatedSet = DS.GetChanges(DataRowState.Modified);
                    DBAdapter.Update(UpdatedSet, "Cheatcode");//업데이트 해주고...
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
                MessageBox.Show("정확한 치트코드를 입력해 주세요! ", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;// 데이타 베이스를 사용

namespace skoban2
{
    public partial class Form4 : Form
    {
        

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)//자 이것은 자동으로 생기는 코드, 이게 뭐냐하면 데이타그리드뷰로 엑세스파일을 링크 시키면 자동으로 코딩됩니다.즉 엑세스 파일을 보일 수 있게 해주는 것이죠
        {
            // TODO: 이 코드는 데이터를 'rankDataSet.Ranklist' 테이블에 로드합니다. 필요한 경우 이 코드를 이동하거나 제거할 수 있습니다.
            this.ranklistTableAdapter.Fill(this.rankDataSet.Ranklist);

            
        }

       
    }
}
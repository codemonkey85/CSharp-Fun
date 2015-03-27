using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace LinqFun
{
    public partial class frmData : Form
    {
        List<Person> People;
        BindingSource bs = new BindingSource();
        public frmData()
        {
            InitializeComponent();
        }
        public void SetDataSource(List<Person> People)
        {
            this.People = People;
            bs.DataSource = this.People;
            dgData.DataSource = bs;
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (People != null)
            {
                if (txtFilter.Text == string.Empty)
                {
                    bs.DataSource = People;
                }
                else
                {
                    bs.DataSource = People.Where(person =>
                            person.LastName.ToLower().Contains(txtFilter.Text.ToLower().Trim()) ||
                            person.FirstName.ToLower().Contains(txtFilter.Text.ToLower().Trim())
                            ).ToList();
                }
            }
        }
        private void frmData_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgData.EndEdit();
        }
    }
}

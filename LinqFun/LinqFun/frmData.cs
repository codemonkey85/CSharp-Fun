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
            dgData.Columns.Remove("Gender");
            DataGridViewComboBoxColumn clmn = new DataGridViewComboBoxColumn();
            clmn.Name = "Gender";
            clmn.HeaderText = clmn.Name;
            clmn.DataSource = Enum.GetValues(typeof(Genders));
            clmn.DataPropertyName = "Gender";
            dgData.Columns.Add(clmn);
            dgData.Columns["isFemale"].Visible = false;
            dgData.Columns["isMale"].Visible = false;
            cbGender.Items.Clear();
            cbGender.Items.Add(new ComboboxItem { Text = "", Value = null });
            cbGender.Items.AddRange(Enum.GetValues(typeof(Genders)).Cast<object>().ToArray());
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }
        private void frmData_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgData.EndEdit();
        }
        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }
        private void UpdateFilter()
        {
            if (People != null)
            {
                if (txtFilter.Text == string.Empty && cbGender.SelectedItem == null)
                {
                    bs.DataSource = People;
                }
                else
                {
                    bs.DataSource = People.Where(person =>
                            (person.LastName.ToLower().Contains(txtFilter.Text.ToLower().Trim()) ||
                            person.FirstName.ToLower().Contains(txtFilter.Text.ToLower().Trim())) &&
                            cbGender.SelectedValue == null ? true : person.Gender.ToString() == cbGender.SelectedItem.ToString()
                            ).ToList();
                }
            }
        }
    }
}

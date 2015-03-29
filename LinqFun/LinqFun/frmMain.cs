using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace LinqFun
{
    public partial class frmMain : Form
    {
        bool xml = true;
        const string FileName = @"People";
        public List<Person> People = new List<Person>();
        BindingSource bs = new BindingSource();
        private CurrencyManager currencyManager = null;
        public frmMain()
        {
            InitializeComponent();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            currencyManager.Position--;
            UpdateForm();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            currencyManager.Position++;
            UpdateForm();
        }
        private void UpdateForm()
        {
            btnNext.Enabled = currencyManager.Position < currencyManager.Count - 1;
            btnPrevious.Enabled = currencyManager.Position > 0;
            btnDelete.Enabled = currencyManager.Count > 0;
            ssLabelCurrent.Text = string.Format("{0} / {1}", currencyManager.Position + 1, currencyManager.Count);
        }
        public void SavePeople(string filename, bool xml = false)
        {
            if (xml)
            {
                // XML Serialization
                Stream TestFileStream = File.Create(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
                serializer.Serialize(TestFileStream, People);
                TestFileStream.Close();
            }
            else
            {
                // Binary serialization
                Stream TestFileStream = File.Create(filename);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(TestFileStream, People);
                TestFileStream.Close();
            }
        }
        public void LoadPeople(string filename, bool xml = false)
        {
            if (xml)
            {
                // XML Serialization
                if (File.Exists(filename))
                {
                    Stream TestFileStream = File.OpenRead(filename);
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<Person>));
                    People = (List<Person>)deserializer.Deserialize(TestFileStream);
                    TestFileStream.Close();
                }
            }
            else
            {
                // Binary serialization
                if (File.Exists(filename))
                {
                    Stream TestFileStream = File.OpenRead(filename);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    People = (List<Person>)deserializer.Deserialize(TestFileStream);
                    TestFileStream.Close();
                }
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (xml)
            {
                LoadPeople(FileName + ".xml", xml);
            }
            else
            {
                LoadPeople(FileName + ".bin", xml);
            }
            bs.DataSource = People;
            currencyManager = (CurrencyManager)this.BindingContext[bs];
            btnNext.Enabled = People.Count > 1;
            txtFirstName.DataBindings.Add("Text", bs, "FirstName");
            txtLastName.DataBindings.Add("Text", bs, "LastName");
            rbMale.DataBindings.Add("Checked", bs, "isMale");
            rbFemale.DataBindings.Add("Checked", bs, "isFemale");
            UpdateForm();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (xml)
            {
                SavePeople(FileName + ".xml", xml);
            }
            else
            {
                SavePeople(FileName + ".bin", xml);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            currencyManager.AddNew();
            currencyManager.Position = currencyManager.Count - 1;
            UpdateForm();
            txtFirstName.Focus();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            currencyManager.RemoveAt(currencyManager.Position);
            UpdateForm();
        }
        private void btnGrid_Click(object sender, EventArgs e)
        {
            frmData dataForm = new frmData();
            dataForm.SetDataSource(People);
            dataForm.ShowDialog();
            if (currencyManager.Position >= currencyManager.Count)
            {
                currencyManager.Position = currencyManager.Count - 1;
            }
            UpdateForm();
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }
        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }
    }
}

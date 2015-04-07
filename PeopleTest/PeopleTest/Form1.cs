using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace PeopleTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string Filename = @"TEST.bin";
        private Person person = new Person();
        private List<Person> People = new List<Person>();
        private BindingSource bs = new BindingSource();

        private void button1_Click(object sender, EventArgs e)
        {
            //person.Gender = Gender.Male;
            //bs.ResetBindings(false);

            //int thesize = Marshal.SizeOf(typeof(rcSpan2));
            //System.Diagnostics.Debug.WriteLine(string.Format("{0}", thesize));

            //rcSpan2 test = new rcSpan2();
            //test.Value1 = 1;
            //test.Value2 = 2;
            //test.Value3 = 3;
            //test.Value4 = 4;
            //test.Value5 = 5;
            //test.Value6 = 6;
            //test.Value7 = 0;
            //test.Value8 = 1;

            //test.Value1 = 31;
            //test.Value2 = 31;
            //test.Value3 = 31;
            //test.Value4 = 31;
            //test.Value5 = 31;
            //test.Value6 = 31;

            //test.Value7 = true;
            //test.Value8 = true;

            //System.Diagnostics.Debug.WriteLine(string.Format("{0}", test.Value1));

            //System.Diagnostics.Debug.WriteLine(string.Format("{0}", test.Data));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            person = StructUtils.RawDeserialize<Person>(Filename);
            if (person == null)
            {
                person = new Person { FirstName = "", LastName = ""/*, Gender = Gender.Male*/ };
            }
            People.Add(person);
            bs.DataSource = People;
            dgData.DataSource = bs;
            dgData.AllowUserToAddRows = false;
            dgData.AllowUserToDeleteRows = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StructUtils.RawSerialize(person, Filename);
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 48, Pack = 1, CharSet = CharSet.Unicode)]
    [Serializable]
    public class Person
    {
        [FieldOffset(0)]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 10)]
        private string firstname;

        [FieldOffset(20)]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 10)]
        private string lastname;

        [FieldOffset(40)]
        [MarshalAsAttribute(UnmanagedType.U1)]
        private Gender gender;

        [FieldOffset(44)]
        [MarshalAsAttribute(UnmanagedType.Struct)]
        private rcSpan2 rcSpanTest;

        [DisplayName("First Name")]
        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
            }
        }

        [DisplayName("Last Name")]
        public string LastName
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
            }
        }

        //[DisplayName("Gender")]
        //public Gender Gender
        //{
        //    get { return this.gender; }
        //    set { this.gender = value; }
        //}

        [DisplayName("TEST1")]
        public uint Value1
        {
            get { return rcSpanTest.Value1; }
            set { rcSpanTest.Value1 = value; }
        }

        [DisplayName("TEST2")]
        public uint Value2
        {
            get { return rcSpanTest.Value2; }
            set { rcSpanTest.Value2 = value; }
        }

        [DisplayName("TEST3")]
        public uint Value3
        {
            get { return rcSpanTest.Value3; }
            set { rcSpanTest.Value3 = value; }
        }

        [DisplayName("TEST4")]
        public uint Value4
        {
            get { return rcSpanTest.Value4; }
            set { rcSpanTest.Value4 = value; }
        }

        [DisplayName("TEST5")]
        public uint Value5
        {
            get { return rcSpanTest.Value5; }
            set { rcSpanTest.Value5 = value; }
        }

        [DisplayName("TEST6")]
        public uint Value6
        {
            get { return rcSpanTest.Value6; }
            set { rcSpanTest.Value6 = value; }
        }

        [DisplayName("TEST7")]
        public bool Value7
        {
            get { return rcSpanTest.Value7; }
            set { rcSpanTest.Value7 = value; }
        }

        [DisplayName("TEST8")]
        public bool Value8
        {
            get { return rcSpanTest.Value8; }
            set { rcSpanTest.Value8 = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("firstname", firstname, typeof(string));
            info.AddValue("lastname", lastname, typeof(string));
        }

        // The special constructor is used to deserialize values.
        public Person(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            firstname = (string)info.GetValue("firstname", typeof(string));
            lastname = (string)info.GetValue("lastname", typeof(string));
        }

        public Person()
        {
            FirstName = string.Empty;
            LastName = string.Empty;

            rcSpanTest = new rcSpan2();
        }
    }

    public class StructUtils
    {
        static public TType RawDeserialize<TType>(byte[] rawData, int position = 0)
        {
            Type anyType = typeof(TType);
            int rawsize = Marshal.SizeOf(anyType);
            if (rawsize > rawData.Length) return default(TType);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anyType);
            Marshal.FreeHGlobal(buffer);
            return (TType)retobj;
        }

        static public TType RawDeserialize<TType>(string fileName)
        {
            if (File.Exists(fileName))
            {
                byte[] data = null;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        data = br.ReadBytes((int)fs.Length);
                        br.Close();
                        fs.Close();
                    }
                }
                return RawDeserialize<TType>(data, 0);
            }
            else
            {
                return default(TType);
            }
        }

        static public void RawSerialize<TType>(byte[] rawData, int position, TType value)
        {
            var Data = RawSerialize(value);
            Array.Copy(Data, 0, rawData, position, Data.Length);
        }

        public static void RawSerialize(object anything, string fileName)
        {
            if (anything == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            byte[] data = RawSerialize(anything);
            if (data != null)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(data);
                        bw.Close();
                        fs.Close();
                    }
                }
            }
        }

        public static byte[] RawSerialize(object anything)
        {
            int rawSize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawDatas = new byte[rawSize];
            Marshal.Copy(buffer, rawDatas, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawDatas;
        }
    }

    public enum Gender : byte
    {
        Male = 0,
        Female
    }

    [StructLayout(LayoutKind.Explicit, Size = 4, Pack = 1, CharSet = CharSet.Unicode)]
    [Serializable]
    public class rcSpan2
    {
        public rcSpan2()
        {
            Data = 0u;
        }

        [FieldOffset(0)]
        [MarshalAsAttribute(UnmanagedType.U4)]
        internal uint Data;

        public uint Value1
        {
            get { return (Data >> 00) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 00)) | (value & 0x1Fu) << 00; }
        }

        public uint Value2
        {
            get { return (Data >> 05) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 05)) | (value & 0x1Fu) << 05; }
        }

        public uint Value3
        {
            get { return (Data >> 10) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 10)) | (value & 0x1Fu) << 10; }
        }

        public uint Value4
        {
            get { return (Data >> 15) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 15)) | (value & 0x1Fu) << 15; }
        }

        public uint Value5
        {
            get { return (Data >> 20) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 20)) | (value & 0x1Fu) << 20; }
        }

        public uint Value6
        {
            get { return (Data >> 25) & 0x1F; }
            set { Data = (Data & ~(0x1Fu << 25)) | (value & 0x1Fu) << 25; }
        }

        public bool Value7
        {
            get { return ((Data >> 30) & 0x01u) == 1; }
            set { Data = (Data & ~(0x01u << 30)) | (Convert.ToUInt32(value) & 0x01u) << 30; }
        }

        public bool Value8
        {
            get { return ((Data >> 31) & 0x01u) == 1; }
            set { Data = (Data & ~(0x01u << 31)) | (Convert.ToUInt32(value) & 0x01u) << 31; }
        }
    }
}
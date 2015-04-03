using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
        List<Person> People = new List<Person>();
        BindingSource bs = new BindingSource();
        private void button1_Click(object sender, EventArgs e)
        {
            ////Person person = new Person { FirstName = "FirstName", LastName = "LastName" };
            ////People.Add(person);
            ////System.Diagnostics.Debug.WriteLine("Size of Person class is: {0}", Marshal.SizeOf(typeof(Person)));
            ////bs.DataSource = People;
            ////dgData.DataSource = bs;
            //Person person = new Person { FirstName = "Michael", LastName = "Bond" };
            ////BinaryFormatter binaryFormatter = new BinaryFormatter();
            ////SerializeItem(person, Filename, binaryFormatter);

            //StructUtils.RawSerialize(person, Filename);
            //person = StructUtils.RawDeserialize<Person>(Filename);

            //System.Diagnostics.Debug.WriteLine("");
        }

        public static void SerializeItem(Person person, string fileName, IFormatter formatter)
        {
            FileStream s = new FileStream(fileName, FileMode.Create);
            formatter.Serialize(s, person);
            s.Close();
        }


        public static void DeserializeItem(ref Person person, string fileName, IFormatter formatter)
        {
            FileStream s = new FileStream(fileName, FileMode.Open);
            person = (Person)formatter.Deserialize(s);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            person = StructUtils.RawDeserialize<Person>(Filename);
            if (person == null) 
            {
                person = new Person { FirstName = "", LastName = ""};
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

    [StructLayout(LayoutKind.Explicit, Size = 40, Pack = 1, CharSet = CharSet.Unicode)]
    [Serializable]
    public class Person : ISerializable
    {
        [FieldOffset(0)]
        private string firstname;

        [FieldOffset(20)]
        private string lastname;

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

}
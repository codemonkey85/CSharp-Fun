using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace LinqFun
{
    [Serializable]
    public class Person
    {
        public Person()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Gender = Genders.Male;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public bool isMale
        {
            get
            {
                return this.Gender == Genders.Male;
            }
            set
            {
                if (value)
                {
                    this.Gender = Genders.Male;
                }
                else
                {
                    this.Gender = Genders.Female;
                }
            }
        }
        public bool isFemale
        {
            get
            {
                return this.Gender == Genders.Female;
            }
            set
            {
                if (value)
                {
                    this.Gender = Genders.Female;
                }
                else
                {
                    this.Gender = Genders.Male;
                }
            }
        }
    }
    public enum Genders : int
    {
        Male = 0,
        Female = 1
    }
}

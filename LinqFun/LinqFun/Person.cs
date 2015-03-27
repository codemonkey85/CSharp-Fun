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
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

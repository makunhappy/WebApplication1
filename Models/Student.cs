using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public School SchoolInfo { get; set; }
    }
}

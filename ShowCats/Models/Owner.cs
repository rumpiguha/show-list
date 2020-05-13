using ShowCats.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShowCats.Models
{
    public class Owner
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string Age { get; set; }
        public List<Pet> Pets { get; set; }
    }
}

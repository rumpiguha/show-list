using System;
using System.Collections.Generic;
using System.Text;

namespace ShowCats.Models
{
    public class Response
    {
        public List<Owner> OwnerList { set; get; }

        public string Error { get; set; }
    }
}

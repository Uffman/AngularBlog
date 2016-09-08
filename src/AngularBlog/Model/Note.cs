﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularBlog.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
    }

}

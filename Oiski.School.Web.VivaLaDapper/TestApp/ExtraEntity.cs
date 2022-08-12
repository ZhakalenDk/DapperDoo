﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    [Table("Extras")]
    public class ExtraEntity
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}

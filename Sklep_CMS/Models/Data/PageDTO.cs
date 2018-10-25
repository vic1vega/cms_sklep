﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sklep_CMS.Models.Data
{
    [Table("tblPages")]
    public class PageDTO
    {
        [Key]
        public int Id { get; set; }
        public string Title{ get; set; }
        public string Slug { get; set; }
        public string Body{ get; set; }
        public string Sorting { get; set; }
        public bool HasSidebar { get; set; }

    }
}
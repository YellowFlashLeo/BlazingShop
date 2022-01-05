using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingShop.Shared
{
   public class Stats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int Visits { get; set; }
        public DateTime? LastVisit { get; set; }
    }
}

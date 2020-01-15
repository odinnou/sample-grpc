using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class Product
    {
        [StringLength(50)]
        public string Reference { get; set; }
        [StringLength(20)]
        public string Declination { get; set; }
        public int Order { get; set; }
        public bool IsBio { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}

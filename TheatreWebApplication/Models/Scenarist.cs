using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApplication
{
    public partial class Scenarist
    {
        public Scenarist()
        {
            Acts = new HashSet<Act>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Ім'я сценариста")]
        public string ScenaristName { get; set; } = null!;

        public virtual ICollection<Act> Acts { get; set; }
    }
}

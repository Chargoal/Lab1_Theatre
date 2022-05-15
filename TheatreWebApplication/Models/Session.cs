using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApplication
{
    public partial class Session
    {
        public int Id { get; set; }
        [Display(Name = "Назва постанови")]
        public int ActId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Дата і час вистави")]
        public DateTime DateAndTime { get; set; }

        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Ім'я картини")]
        public virtual Act Act { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApplication
{
    public partial class ActActor
    {
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Постанова")]
        public int ActId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Актор")]
        public int ActorId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Дата затверждення контракту")]
        public DateTime ContractDate { get; set; }

        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Постанова")]
        public virtual Act Act { get; set; } = null!;
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Актор")]
        public virtual Actor Actor { get; set; } = null!;
    }
}

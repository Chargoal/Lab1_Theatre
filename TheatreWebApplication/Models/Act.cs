using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApplication
{
    public partial class Act
    {
        public Act()
        {
            ActActors = new HashSet<ActActor>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Ім'я картини")]
        public string ActName { get; set; } = null!;
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Бюджет")]
        public decimal Budget { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Тривалість")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Продюсер")]
        public int ProducerId { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Сценарист")]
        public int ScenaristId { get; set; }

        //[Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Продюсери")]
        public virtual Producer Producer { get; set; } = null!;
        //[Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Сценаристи")]
        public virtual Scenarist Scenarist { get; set; } = null!;
        public virtual ICollection<ActActor> ActActors { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}

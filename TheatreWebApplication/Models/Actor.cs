using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreWebApplication
{
    public partial class Actor
    {
        public Actor()
        {
            ActActors = new HashSet<ActActor>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле повинно бути не порожнім")]
        [Display(Name = "Актори")]
        public string ActorName { get; set; } = null!;
        [Required(ErrorMessage = "Будь ласка вкажіть дату")]
        [Display(Name = "Дата початку кар'єри")]
        public DateTime CareerStart { get; set; }

        public virtual ICollection<ActActor> ActActors { get; set; }
    }
}

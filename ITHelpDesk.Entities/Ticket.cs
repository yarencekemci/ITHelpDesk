using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace ITHelpDesk.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        //model doğrulamasının temeli
        //geçersiz verileri reddetmesini sağlayacağız.

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [StringLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string Description { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public int? AssignedToId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public User AssignedTo { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
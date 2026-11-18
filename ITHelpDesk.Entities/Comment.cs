
using System.ComponentModel.DataAnnotations;

namespace ITHelpDesk.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Yorum alanı zorunludur.")]
        [StringLength(1000, ErrorMessage = "Yorum en fazla 1000 karakter olabilir.")]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public Ticket? Ticket { get; set; }

        public User? User { get; set; }
    }
}

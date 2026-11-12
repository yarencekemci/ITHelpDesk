namespace ITHelpDesk.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedDate { get; set; }

        public Ticket Ticket { get; set; }

        public User User { get; set; }
    }
}
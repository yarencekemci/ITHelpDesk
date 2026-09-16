using System.Net.Sockets;

namespace ITHelpDesk.Entities
{
    public class Status
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace client.Models.Messages
{
    public class MessageEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTime Time{ get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
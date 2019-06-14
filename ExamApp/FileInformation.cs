using System;

namespace ExamApp
{
    public class FileInformation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedDate { get; set; }
        
        public string Url { get; set; }

        public string FilePath { get; set; }
    }
}

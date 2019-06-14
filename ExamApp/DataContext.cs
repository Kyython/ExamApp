namespace ExamApp
{
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext() : base("name=DataContext")
        {
        }

        public DbSet<FileInformation> FileInformations { get; set; }
    }
}
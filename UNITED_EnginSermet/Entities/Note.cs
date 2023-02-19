namespace UNITED_EnginSermet.Entities
{
    public class Note
    {
        public Note()
        {
            SubNotes = new HashSet<SubNote>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<SubNote> SubNotes { get; set; }
    }
}

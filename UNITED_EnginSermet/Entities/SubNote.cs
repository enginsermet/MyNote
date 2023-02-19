namespace UNITED_EnginSermet.Entities
{
    public class SubNote
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }

        public virtual Note? Note { get; set; }
    }
}

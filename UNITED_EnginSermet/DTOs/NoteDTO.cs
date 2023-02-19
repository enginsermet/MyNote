namespace UNITED_EnginSermet.DTOs
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public int? NoteId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
    }
}

namespace NoteApp.Models;

public class Note
{
    public Note(string title, string description)
    {
        Title = title;
        Description = description;
        CreateAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public DateTime CreateAt { get; init; }
}

namespace NoteApp.Contracts;

public record GetNoteRequest(string? Search, string? SortItem, string? SortOrder);
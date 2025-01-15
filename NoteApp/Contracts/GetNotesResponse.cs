namespace NoteApp.Contracts;

public record GetNotesResponse(List<NoteDto> notes);
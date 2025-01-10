using Microsoft.AspNetCore.Mvc;

public interface IA2Repo
{
    Task<bool> ValidLoginAsync(string username, string password);
    Task<bool> IsOrganizerAsync(string username, string password);
    Task<Event> GetEventDetailsAsync(int eventId);
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;

public class A2Repo : IA2Repo
{

    private readonly A2DbContext _repo;
    public A2Repo(A2DbContext repo)
    {
        _repo = repo;
    }

    public async Task<bool> ValidLoginAsync(string username, string password)
    {
        Users user = await _repo.Users.FirstOrDefaultAsync(e => e.UserName == username && e.Password == password);

        if (user != null)
            return true;
        return false;

    }

    public async Task<bool> IsOrganizerAsync(string username, string password)
    {
        Organizer user = await _repo.Organizers.FirstOrDefaultAsync(c=>c.Name == username && c.Password == password);   
        if (user != null) return true;
        return false;
    }

    public async Task<Event> GetEventDetailsAsync(int eventId)
    {
        return  await _repo.Events.FirstOrDefaultAsync(e=>e.Id == eventId);
    }
}
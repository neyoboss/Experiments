public interface IMatchService
{
    Task CreateUpdateMatch(string Id, User loggedUsed,User user);
}
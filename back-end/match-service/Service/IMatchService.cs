public interface IMatchService
{
    Task CreateUpdateMatch(User loggedUser, User user);
    Task<List<string>> ids(string id);
}
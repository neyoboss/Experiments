public interface IMatchService
{
    Task CreateUpdateMatch(User loggedUser, User user);
}
using BackendServer.Models.UserModels;

namespace BackendServer.Services.UserServices.Repository;

public interface IUserRepository
{
    public Task CreateUser(User user, string password, string role);
    
    public Task<bool> AreCredentialsTaken(string email, string username);
    
    public Task<string> LoginUser(string email, string password);
    
    public bool IsUserLoggedIn(string username);
    
    public Task LogoutUser(string username);

    public ValueTask<User?> GetUserById(string id);
    
    public Task UpdateKarma(User user, int karma);
    
    //public Task Upvote();
    
    //public Task Downvote();
    
    //public Task RemoveUpvote();
    
    //public Task RemoveDownvote();

    public ValueTask<User?> GetUserByUsername(string username);
    
    public ValueTask<User?> GetUserOnlyProblems(string username);
    
    public ValueTask<User?> GetUserOnlyFixes(string username);
    
    //public Task UpdateUser(User user, string? password);
}
namespace LearnAPI.Repos.Models
{
    public interface IRefreshHandler
    {
        Task<string> GenerateToken(string username);
    }
}

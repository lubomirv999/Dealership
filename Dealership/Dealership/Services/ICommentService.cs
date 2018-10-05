namespace Dealership.Services
{
    using Dealership.Models;

    public interface ICommentService
    {
        void Add(int carId, string content, string userId, int? replyTo);
    }
}

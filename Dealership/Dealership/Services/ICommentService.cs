namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models;

    public interface ICommentService
    {
        void Add(int carId, string content, string userId, int? replyTo);
        Comment FindById(int? commentId);
        void Delete(int commentId);

    }
}

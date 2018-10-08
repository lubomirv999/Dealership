namespace Dealership.Services
{
    using Dealership.Data;

    public interface ICommentService
    {
        void Add(int carId, string content, string userId, int? parentCommentId);

        Comment FindById(int? commentId);

        void Delete(int commentId);
    }
}

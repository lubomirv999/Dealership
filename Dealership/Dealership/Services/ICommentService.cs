namespace Dealership.Services
{
    using Dealership.Data;

    public interface ICommentService
    {
        Comment FindById(int? commentId);

        void Add(int carId, string content, string userId, int? parentCommentId);        

        void Delete(int commentId);
    }
}

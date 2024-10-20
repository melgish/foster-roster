using FosterRoster.Domain;
using System.Net.Http.Json;

namespace FosterRoster.Client.Services;

public sealed class ClientCommentRepository(
    HttpClient httpClient
) : ICommentRepository
{
    const string Route = "api/comments";

    public async Task<Comment> AddAsync(Comment comment)
    {
        var rs = await httpClient.PostAsJsonAsync<CommentEditModel>(Route, new(comment));
        return await rs.Content.ReadFromJsonAsync<Comment>() ?? comment;
    }

    public async Task<bool> DeleteByKeyAsync(int commentId)
    {
        var rs = await httpClient.DeleteAsync($"{Route}/{commentId}");
        return await rs.Content.ReadFromJsonAsync<bool>();
    }
}
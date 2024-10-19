namespace FosterRoster.Services;

using FosterRoster.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
namespace FosterRoster.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FosterRoster.Domain;
using System;

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
}
namespace FosterRoster.Services;

using System.Threading.Tasks;

using FosterRoster.Data;
using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;

public sealed class ServerCommentRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory
) : ICommentRepository
{
    public async Task<Comment> AddAsync(Comment comment)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var entry = await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteByKeyAsync(int commentId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context
            .Comments
            .Where(c => c.Id == commentId)
            .ExecuteDeleteAsync()) > 0;
    }
}
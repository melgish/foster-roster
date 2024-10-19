namespace FosterRoster.Services;

using FosterRoster.Data;
using FosterRoster.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public sealed class ServerCommentRepository(
    IDbContextFactory<FosterRosterDbContext> contextFactory,
    TimeProvider timeProvider
) : ICommentRepository
{
    public async Task<Comment> AddAsync(Comment comment)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        // Workaround because of issues getting ValueGeneratedOnAdd() to work.
        comment.TimeStamp = timeProvider.GetUtcNow().UtcDateTime;
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
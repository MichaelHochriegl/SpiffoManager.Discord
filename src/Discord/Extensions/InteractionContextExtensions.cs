namespace Discord.Extensions;

/// <summary>
/// Extensions for <see cref="IInteractionContext"/> related operations
/// </summary>
public static class InteractionContextExtensions
{
    /// <summary>
    /// Checks if the given <see cref="IUser"/> has the given <paramref name="roleId"/>
    /// </summary>
    /// <param name="user">The <see cref="IUser"/> to check</param>
    /// <param name="roleId">The roleId to check against</param>
    /// <returns></returns>
    public static bool HasRole(this IUser user, ulong roleId)
    {
        var guildUser = user as IGuildUser;
        return guildUser != null && guildUser.RoleIds.Contains(roleId);
    } 
}
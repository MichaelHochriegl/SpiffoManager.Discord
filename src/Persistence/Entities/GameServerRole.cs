namespace Persistence.Entities;

public class GameServerRole
{
    public ulong Id { get; set; }
    public ulong GameServerId { get; set; }
    public ulong GuildId { get; set; }
    public ulong RoleId { get; set; }

    private GameServerRole()
    {
        
    }
    
    public GameServerRole(ulong gameServerId, ulong guildId, ulong roleId)
    {
        GameServerId = gameServerId;
        GuildId = guildId;
        RoleId = roleId;
    }
}
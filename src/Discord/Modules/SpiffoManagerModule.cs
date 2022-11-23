using Discord.Extensions;
using Discord.Interactions;
using Discord.Modals;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Entities;
using Persistence.Repositories;

namespace Discord.Modules;

/// <summary>
/// Discord interaction module for commands targeting common bot management
/// </summary>
public class SpiffoManagerModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IServiceProvider _serviceProvider;
    private const string AddGameServerModalCustomId = "add_pz_gs";

    public SpiffoManagerModule(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// Issues a modal to the user that allows for adding a new pz server
    /// </summary>
    [UsedImplicitly]
    [SlashCommand("add-server", "Adds a gameserver to the bot to manage.")]
    [RequireOwner]
    public async Task HandleAddServerCommand()
    {
        await RespondWithModalAsync<AddGameServerModal>(AddGameServerModalCustomId);
    }

    /// <summary>
    /// Catches the modal confirmation and adds the server if not already managed
    /// </summary>
    /// <param name="modal">The <see cref="AddGameServerModal"/> the user sent</param>
    [UsedImplicitly]
    [ModalInteraction(AddGameServerModalCustomId)]
    public async Task HandleAddGameServerModal(AddGameServerModal modal)
    {
        using var scope = _serviceProvider.CreateScope();
        var gameServerRepository = scope.ServiceProvider.GetRequiredService<IGameServerRepository>();
        var nameUsed = await gameServerRepository.IsGameServerNameAlreadyUsedAsync(modal.ServerName);
        if (nameUsed)
        {
            await RespondAsync(embed: $"A gameserver with the name '{modal.ServerName}' is already present!".ToErrorEmbed());
            return;
        }

        var alreadyManaged = await gameServerRepository.IsGameServerInstallAlreadyUsedAsync(modal.ServerInstallPath);
        if (alreadyManaged)
        {
            await RespondAsync(embed: $"A gameserver with the path '{modal.ServerInstallPath}' is already managed!"
                .ToErrorEmbed());
            return;
        }

        var gs = new GameServer(modal.ServerName, modal.ServerInstallPath);
        try
        {
            await gameServerRepository.AddAsync(gs);
        }
        catch (Exception)
        {
            await RespondAsync(embed: "There was an error while saving!".ToErrorEmbed());
            return;
        }

        await RespondAsync(embed: "Gameserver saved!".ToSuccessEmbed());
    }
}
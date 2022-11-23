using Discord.Interactions;

namespace Discord.Modals;

public sealed class AddGameServerModal : IModal
{
    public string Title => "Add PZ GameServer";

    [InputLabel("Name of the gameserver")]
    [ModalTextInput("gs_name", TextInputStyle.Short, placeholder: "pzserver", maxLength: 50)]
    public string ServerName { get; set; } = "pzserver";

    [InputLabel("Relative path to the LGSM install directory")]
    [ModalTextInput("gs_rel_inst_path", TextInputStyle.Short, placeholder: "../", maxLength: 150)]
    public string ServerInstallPath { get; set; } = "../";
}
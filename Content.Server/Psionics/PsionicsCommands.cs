using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Psionics.Abilities;
using Robust.Shared.Console;
using Content.Shared.Actions;
using Robust.Shared.Player;

namespace Content.Server.Psionics;

[AdminCommand(AdminFlags.Logs)]
public sealed class ListPsionicsCommand : IConsoleCommand
{
    public string Command => "lspsionics";
    public string Description => Loc.GetString("command-lspsionic-description");
    public string Help => Loc.GetString("command-lspsionic-help");
    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        SharedActionsSystem actions = default!;
        var entMan = IoCManager.Resolve<IEntityManager>();
        foreach (var (actor, psionic, meta) in entMan.EntityQuery<ActorComponent, PsionicComponent, MetaDataComponent>())
        {
            // filter out xenos, etc, with innate telepathy
            actions.TryGetActionData( psionic.PsionicAbility, out var actionData );
            if (actionData == null || actionData.ToString() == null)
                return;

            var psiPowerName = actionData.ToString();
            if (psiPowerName == null)
                return;

            shell.WriteLine(meta.EntityName + " (" + meta.Owner + ") - " + actor.PlayerSession.Name + Loc.GetString(psiPowerName));
        }
    }
}

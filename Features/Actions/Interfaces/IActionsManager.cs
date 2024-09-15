using Molde.Entities;

namespace Molde.Features.Actions.Interfaces;

public interface IActionsManager
{
    Task<bool> ExecuteActions(List<ActionEntity> actions, Dictionary<string, string> variables);
}

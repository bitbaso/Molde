using Molde.Entities;

namespace Molde.Features.PromptManager.Interfaces;

public interface IPromptManager
{
    Task<bool> RunPromptAsync(List<ConfigEntity> configurations);
}

using Molde.Entities;

namespace Molde.Features.ConfigFiles.Interfaces;

public interface IConfigFilesManager
{
    Task<List<ConfigEntity>> LoadConfigFilesAsync(string path);
}

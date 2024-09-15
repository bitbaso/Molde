namespace Molde.Entities;

public class ConfigFileEntity
{
    public string Name { get; set; }
    public List<string> MoldeFiles { get; set; }
    public List<PromptEntity> Prompts { get; set; }
    public List<ActionEntity> Actions { get; set; }
}
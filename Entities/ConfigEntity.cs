namespace Molde.Entities;

public class ConfigEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PromptEntity> Prompts { get; set; }
    public List<ActionEntity> Actions { get; set; }
}
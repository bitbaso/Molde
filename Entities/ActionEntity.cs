namespace Molde.Entities;

public class ActionEntity
{
    public string Type { get; set; }
    public string Template { get; set; }
    public string TemplateFile { get; set; }
    public string Output { get; set; }
    public string Path { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public string Command { get; set; }
    public string TargetFile { get; set; }
    public string Marker { get; set; }
    public string TemplateDir { get; set; }
    public string OutputDir { get; set; }
    public List<string> Files { get; set; }
}

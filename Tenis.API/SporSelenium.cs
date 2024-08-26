namespace Tenis.API;
public class Model
{
    public string? Day { get; set; }
    public List<Form> Forms { get; set; } = new List<Form>();
}
public class Form
{
    public string? Time { get; set; }
    public string? State { get; init; }
}

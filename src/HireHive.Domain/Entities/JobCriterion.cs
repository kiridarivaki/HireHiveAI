namespace HireHive.Domain.Entities;

public class JobCriterion
{
    public string Name { get; set; } = null!;
    public int Weight { get; set; }

    public JobCriterion() { }
    public JobCriterion(string name, int weight)
    {
        Name = name;
        Weight = weight;
    }
}

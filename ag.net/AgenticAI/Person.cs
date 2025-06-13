using Json.Schema.Generation;

namespace AgenticAI;

[Title("Person")]
[Description("Represents a person with various attributes such as name, age, occupation, location, and interests.")]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Occupation { get; set; }
    public string Location { get; set; }
    public List<string> Interests { get; set; }

    public override string ToString()
    {
        return @$"Name: {Name}, Age: {Age}, Occupation: {Occupation}, Location: {Location}, 
            Interests: {string.Join(", ", Interests)} ";
    }
}
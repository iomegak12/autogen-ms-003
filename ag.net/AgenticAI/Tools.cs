using AutoGen.Core;

namespace AgenticAI;

public partial class Tools
{
    [Function("UpperCase", "Converts a string to uppercase.")]
    public async Task<string> UpperCase(string input)
    {
        return input.ToUpper();
    }

    [Function("Concat", "Concatenates an array of strings into a single string with a specified separator.")]
    public async Task<string> Concat(string[] messages, string separator = ", ")
    {
        if (messages == null || messages.Length == 0)
        {
            return string.Empty;
        }

        return string.Join(separator, messages);
    }

    [Function("CalculateTax", "Calculates the tax on a given amount based on a specified tax rate.")]
    public async Task<string> CalculateTax(decimal amount, decimal taxRate)
    {
        if (amount < 0 || taxRate < 0)
        {
            throw new ArgumentException("Amount and tax rate must be non-negative.");
        }

        decimal tax = amount * taxRate / 100;
        return $"The tax on {amount:C} at a rate of {taxRate}% is {tax:C}.";
    }
}
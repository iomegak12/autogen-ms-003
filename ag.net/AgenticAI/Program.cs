using System;
using System.Threading.Tasks;
using AutoGen.Basic.Sample;

namespace AgenticAI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select an option:");

                Console.WriteLine("1. Connect with Azure OpenAI");
                Console.WriteLine("2. Connect with Azure OpenAI Eval");
                Console.WriteLine("3. Agent with Custom Middleware");
                Console.WriteLine("4. Agent with RAG Middleware");
                Console.WriteLine("5. Two Agents Talking to Each Other");
                Console.WriteLine("6. Tools with Agent Middleware");
                Console.WriteLine("7. Tools with Agent Middleware No Invoke");
                Console.WriteLine("8. Structured Output Demo");
                Console.WriteLine("9. Multimodality Model Demo (Background Image)");
                Console.WriteLine("10. Multimodality Model Demo (Chart Analysis)");
                Console.WriteLine("11. Simple Semantic Kernel Demo");
                Console.WriteLine("12. Using SK Plugin in AutoGen Middleware");
                Console.WriteLine("13. Brave Search SK Plugin in AutoGen Middleware");
                Console.WriteLine("14. Round Robin Group Chat Demo");
                Console.WriteLine("15. Two Agent Fill Application Demo");

                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ConnectWithAzureOpenAI.RunAsync().ConfigureAwait(false);
                        break;
                    case "2":
                        await ConnectWithAzureOpenAIEval.RunAsync().ConfigureAwait(false);
                        break;
                    case "3":
                        await AgentWithCustomMiddleware.RunAsync().ConfigureAwait(false);
                        break;
                    case "4":
                        await AgentWithRAGMiddleware.RunAsync().ConfigureAwait(false);
                        break;
                    case "5":
                        await TwoAgentsTalkingEachOther.RunAsync().ConfigureAwait(false);
                        break;
                    case "6":
                        await ToolsWithAgentMiddleware.RunAsync().ConfigureAwait(false);
                        break;
                    case "7":
                        await ToolsWithAgentMiddlewareNoInvoke.RunAsync().ConfigureAwait(false);
                        break;
                    case "8":
                        await StructuredOutputDemo.RunAsync().ConfigureAwait(false);
                        break;
                    case "9":
                        await MultimodalityModelDemo.RunAsync("background.png").ConfigureAwait(false);
                        break;
                    case "10":
                        await MultimodalityModelDemo.RunAsync("USMortageRate.png").ConfigureAwait(false);
                        break;
                    case "11":
                        await SimpleSKDemo.RunAsync().ConfigureAwait(false);
                        break;
                    case "12":
                        await UsingSKPluginInAutoGenMiddleware.RunAsync().ConfigureAwait(false);
                        break;
                    case "13":
                        await BraveSearchSKPluginInAutoGenMiddlewareDemo.RunAsync().ConfigureAwait(false);
                        break;
                    case "14":
                        await RoundRobinGroupChatDemo.RunAsync().ConfigureAwait(false);
                        break;  
                    case "15":
                        await TwoAgent_Fill_Application.RunAsync().ConfigureAwait(false);
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
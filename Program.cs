using System;
using LogicInterpreter.Core;
using LogicInterpreter.Parsing;
using LogicInterpreter.Commands;

namespace LogicInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Logic Expression Interpreter");
            Console.WriteLine("============================");
            Console.WriteLine("Commands: DEFINE, SOLVE");
            Console.WriteLine("Type 'exit' to quit");
            Console.WriteLine();

            FunctionRegistry registry = new FunctionRegistry();
            TreeEvaluator evaluator = new TreeEvaluator(registry.GetFunctions());

            DefineCommand defineCmd = new DefineCommand(registry);
            SolveCommand solveCmd = new SolveCommand(registry, evaluator);
            CommandParser parser = new CommandParser();

            Console.WriteLine();
            Console.WriteLine("Ready for commands.");
            Console.WriteLine();

            // Main command loop
            bool running = true;
            while (running)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (input == null || input.Length == 0)
                    continue;

                try
                {
                    ParsedCommand command = parser.Parse(input);

                    switch (command.Type)
                    {
                        case CommandType.DEFINE:
                            defineCmd.Execute(command);
                            break;

                        case CommandType.SOLVE:
                            solveCmd.Execute(command);
                            break;

                        case CommandType.EXIT:
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;

                        case CommandType.UNKNOWN:
                            Console.WriteLine("Unknown command. Available commands: DEFINE, SOLVE, EXIT");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }
    }
}

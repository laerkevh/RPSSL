using System; // imports the System namespace, giving access to console functions.

namespace Rpssl; // defines the program's namespace.

internal class Program // defines the Program class, which contains the game logic.
{
    private const int WinningScore = 3; // number of points required to win the game.

    private static void Main(string[] args) // entry point for the program.
    {
        Console.WriteLine("RPSSL — Rock, Paper, Scissors, Spock, Lizard"); // prints the game title.
        Console.WriteLine($"First to {WinningScore} wins. Choose R/P/S/Sp/L or Q to quit.\n"); // explains the rules and choices.

        int userScore = 0, agentScore = 0; // keeps track of user and agent scores.

        while (userScore < WinningScore && agentScore < WinningScore) // loop until one player wins.
        {
            Console.Write("Your choice (R/P/S/Sp/L or Q): "); // prompt for input.
            var raw = Console.ReadLine(); // read user input as a string.

            if (string.Equals(raw, "q", StringComparison.OrdinalIgnoreCase)) // check for quit command.
            {
                Console.WriteLine("Exiting the game. Thanks for playing!"); // exit message.
                return; // terminate the program.
            }

            if (!TryParseShape(raw, out var user)) // attempt to convert input into a Shape enum. Returns false if invalid.
            {
                Console.WriteLine("Invalid input. Use R/P/S/Sp/L (or Q)."); // notify about invalid input.
                continue; // restart loop.
            }

            var agent = PickAgent(); // randomly select the agent's shape.
            var result = ResolveRound(user, agent); // determine round outcome.

            if (result == Result.Win) userScore++; // user wins → increase score.
            else if (result == Result.Lose) agentScore++; // agent wins → increase score.

            PrintRound(user, agent, result, userScore, agentScore); // show round result and updated score.
        }

        Console.WriteLine(userScore > agentScore ? "\nYou won the game!" : "\nThe agent won the game!"); // final winner message.
    }

    // Helper functions

    private static bool TryParseShape(string? input, out Shape shape) // tries to convert string input into a Shape.
    {
        switch ((input ?? string.Empty).Trim().ToLower()) // normalize input.
        {
            case "r":
            case "rock":     shape = Shape.Rock;     return true;
            case "p":
            case "paper":    shape = Shape.Paper;    return true;
            case "s":
            case "scissors": shape = Shape.Scissors; return true;
            case "sp":
            case "spock":    shape = Shape.Spock;    return true;
            case "l":
            case "lizard":   shape = Shape.Lizard;   return true;
            default:
                shape = default; // invalid input → return default and false.
                return false;
        }
    }

    private static Shape PickAgent() // randomly picks the agent's shape.
        => (Shape)Random.Shared.Next(0, 5); // returns a number 0–4, cast to Shape.

    private static Result ResolveRound(Shape p1, Shape p2) // determines round outcome.
    {
        if (p1 == p2) return Result.Tie; // same choices → tie.

        int diff = (int)p2 - (int)p1; // compute difference between shapes.

        // determine win/lose based on difference.
        switch (diff)
        {
            case -4:
            case -2:
            case 1:
            case 3:
                return Result.Win; // user wins.

            case -3:
            case -1:
            case 2:
            case 4:
                return Result.Lose; // user loses.

            default:
                return Result.Tie; // fallback (should not happen).
        }
    }

    private static void PrintRound(Shape user, Shape agent, Result result, int userScore, int agentScore) // prints round details.
    {
        Console.WriteLine($"\nYou: {user}  vs  Agent: {agent}");
        Console.WriteLine(result switch // switch expression for result message
        {
            Result.Win  => "→ You won the round!",
            Result.Lose => "→ The agent won the round!",
            _           => "→ It's a tie!",
        });
        Console.WriteLine($"Score: You {userScore} : {agentScore} Agent\n"); // show updated score.
    }

    // Types

    private enum Shape // the five possible shapes.
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
        Spock = 3,
        Lizard = 4
    }

    private enum Result { Win, Lose, Tie } // round outcome.
}


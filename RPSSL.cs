using System;

namespace RpsslConsole;

internal class Program
{
    private const int WinningScore = 3;

    private static void Main(string[] args)
    {
        Console.WriteLine("RPSSL — Rock, Paper, Scissors, Spock, Lizard");
        Console.WriteLine($"Først til {WinningScore} vinder. Vælg R/P/S/Sp/L eller Q for at stoppe.\n");

        int userScore = 0, agentScore = 0;

        while (userScore < WinningScore && agentScore < WinningScore)
        {
            Console.Write("Dit valg (R/P/S/Sp/L eller Q): ");
            var raw = Console.ReadLine();

            if (string.Equals(raw, "q", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Afslutter spillet. Tak for kampen!");
                return;
            }

            if (!TryParseShape(raw, out var user))
            {
                Console.WriteLine("Ugyldigt input. Brug R/P/S/Sp/L (eller Q).");
                continue;
            }

            var agent = PickAgent();
            var result = ResolveRound(user, agent);

            if (result == Result.Win) userScore++;
            else if (result == Result.Lose) agentScore++;

            PrintRound(user, agent, result, userScore, agentScore);
        }

        Console.WriteLine(userScore > agentScore ? "\n🎉 Du vandt spillet!" : "\n🤖 Agenten vandt spillet!");
    }

    // ====== Hjælpefunktioner ======

    private static bool TryParseShape(string? input, out Shape shape)
    {
        switch ((input ?? string.Empty).Trim().ToLower())
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
                shape = default;
                return false;
        }
    }

    private static Shape PickAgent()
        => (Shape)Random.Shared.Next(0, 5); // 0..4

    // Returnerer resultat set fra p1 (spilleren)
    private static Result ResolveRound(Shape p1, Shape p2)
    {
        if (p1 == p2) return Result.Tie;

        // Brug den viste tabel: p2 - p1
        int diff = (int)p2 - (int)p1;

        // p1 vinder ved -4, -2, 1, 3
        switch (diff)
        {
            case -4:
            case -2:
            case 1:
            case 3:
                return Result.Win;

            // p1 taber ved -3, -1, 2, 4
            case -3:
            case -1:
            case 2:
            case 4:
                return Result.Lose;

            default:
                // Bør ikke ske, men fallback:
                return Result.Tie;
        }
    }

    private static void PrintRound(Shape user, Shape agent, Result result, int userScore, int agentScore)
    {
        Console.WriteLine($"\nDu: {user}  vs  Agent: {agent}");
        Console.WriteLine(result switch
        {
            Result.Win  => "→ Du vandt runden!",
            Result.Lose => "→ Agenten vandt runden!",
            _           => "→ Uafgjort!",
        });
        Console.WriteLine($"Stilling: Du {userScore} : {agentScore} Agent\n");
    }

    // ====== Typer ======

    private enum Shape
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
        Spock = 3,
        Lizard = 4
    }

    private enum Result { Win, Lose, Tie }
}

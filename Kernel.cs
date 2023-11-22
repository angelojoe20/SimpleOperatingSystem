using System;

public class Kernel : Cosmos.System.Kernel
{
    private const string Username = "brd";
    private const string Password = "beard";
    private const string KernelVersion = "1.0";

    private bool loggedIn = false;

    protected override void BeforeRun()
    {
        Console.WriteLine("================================================================================");
        Console.WriteLine("Welcome to Operating System Demo! We're Grp 7-BS Computer Engineering CPE 0313-3");
        Console.WriteLine("================================================================================");
        Console.WriteLine("Type 'login' to log in or 'exit' to quit.");
        Console.WriteLine("================================================================================");
    }

    protected override void Run()
    {
        while (true)
        {
            if (!loggedIn)
            {
                if (!Login())
                {
                    break; // Exit if login fails
                }
                continue; // Skip the command handling and re-run loop after successful login
            }

            Console.Write("> ");
            var input = Console.ReadLine();

            // Handle CLI commands
            HandleCommand(input);
        }
    }

    private string GetMaskedPassword()
    {
        Console.Write("Enter password: ");
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Handle backspace
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Remove(password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after entering the password
        return password;
    }

    private bool Login()
    {
        Console.Write("Enter Username: ");
        var enteredUsername = Console.ReadLine();

        string enteredPassword = GetMaskedPassword();

        if (enteredUsername == Username && enteredPassword == Password)
        {
            loggedIn = true;
            Console.Clear();
            Console.WriteLine($"Logged in as {Username}");
            DisplayMenu();
            return true;
        }
        else
        {
            Console.WriteLine("Invalid username or password. Try again or type 'exit' to quit.");
            return false;
        }
    }

    private void DisplayKernelVersion()
    {
        Console.WriteLine($"Kernel Version: {KernelVersion}");
    }

    private void GuessTheNumber()
    {
        Console.WriteLine("Welcome to Guess the Number!");
        Console.WriteLine("I'm thinking of a number between 1 and 100.");

        Random random = new Random();
        int randomNumber = random.Next(1, 101);

        int attempts = 0;
        int guess = 0;

        while (guess != randomNumber)
        {
            Console.Write("Enter your guess: ");

            if (!int.TryParse(Console.ReadLine(), out guess))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

            if (guess < randomNumber)
            {
                Console.WriteLine("Too low! Try again.");
            }
            else if (guess > randomNumber)
            {
                Console.WriteLine("Too high! Try again.");
            }

            attempts++;
        }

        Console.WriteLine($"Congratulations! You guessed the number in {attempts} attempts.");
        DisplayMenu(); // Return to the menu after exiting the calculator
    }



    private void HandleCommand(string input)
    {
        switch (input.ToLower())
        {
            case "menu":
                DisplayMenu();
                break;
            case "shutdown":
                Console.WriteLine("Shutting down...");
                Cosmos.System.Power.Shutdown();
                break;
            case "restart":
                Console.WriteLine("Restarting...");
                Cosmos.System.Power.Reboot();
                break;
            case "time":
                DisplayTime();
                break;
            case "date":
                DisplayDate();
                break;
            case "sysinfo":
                DisplaySystemInfo();
                break;
            case "calc":
                Calculator();
                break;
            case "version":
                DisplayKernelVersion();
                break;
            case "guess":
                GuessTheNumber();
                break;
            case "clear":
                Console.Clear();
                DisplayMenu();
                break;
            case "logout":
                loggedIn = false;
                Console.Clear();
                Console.WriteLine("Logged out.");
                BeforeRun();
                break;
            case "exit":
                Console.WriteLine("Exiting...");
                Cosmos.System.Power.Shutdown();
                break;
            case "history":
                ShowCommandHistory();
                break;
            default:
                Console.WriteLine("Invalid command. Type 'menu' for options.");
                break;
        }
    }

    private void ShowCommandHistory()
    {
        throw new NotImplementedException();
    }

    private void Calculator()
    {
        Console.WriteLine("Welcome to Calculator!");
        Console.WriteLine("Enter an expression (e.g., 2 + 2) or type 'exit' to return to the menu.");

        string expression;
        do
        {
            Console.Write("> ");
            expression = Console.ReadLine();

            if (expression.ToLower() != "exit")
            {
                try
                {
                    double result = EvaluateExpression(expression);
                    Console.WriteLine($"Result: {result:F2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        } while (expression.ToLower() != "exit");

        DisplayMenu(); // Return to the menu after exiting the calculator
    }

    private double EvaluateExpression(string expression)
    {
        var parts = expression.Split(' ');
        if (parts.Length != 3)
        {
            throw new ArgumentException("Invalid expression format. Use 'number operator number'.");
        }

        double operand1, operand2;
        if (!double.TryParse(parts[0], out operand1) || !double.TryParse(parts[2], out operand2))
        {
            throw new ArgumentException("Invalid number format.");
        }

        double result = 0;
        switch (parts[1])
        {
            case "+":
                result = operand1 + operand2;
                break;
            case "-":
                result = operand1 - operand2;
                break;
            case "*":
                result = operand1 * operand2;
                break;
            case "/":
                if (operand2 == 0)
                {
                    throw new DivideByZeroException("Division by zero is not allowed.");
                }
                result = operand1 / operand2;
                break;
            default:
                throw new ArgumentException("Invalid operator.");
        }

        return result;
    }

    private void DisplayMenu()
    {
        Console.WriteLine("================================================================================");
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Shutdown");
        Console.WriteLine("2. Restart");
        Console.WriteLine("3. Time");
        Console.WriteLine("4. Date");
        Console.WriteLine("5. System Information");
        Console.WriteLine("6. Calculator");
        Console.WriteLine("7. Version");
        Console.WriteLine("8. Game");
        Console.WriteLine("9. Clear");
        Console.WriteLine("10. Logout");
        Console.WriteLine("================================================================================");


        Console.WriteLine("================================================================================");
        Console.WriteLine("Type 'exit' to return to the command prompt.");
        Console.WriteLine("================================================================================");
    }

    private void DisplayTime()
    {
        var time = Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second;
        Console.WriteLine("Current time: " + time);
    }

    private void DisplayDate()
    {
        var date = Cosmos.HAL.RTC.Month + "/" + Cosmos.HAL.RTC.DayOfTheMonth + "/" + Cosmos.HAL.RTC.Year;
        Console.WriteLine("Current date: " + date);
    }

    private void DisplaySystemInfo()
    {
        var osName = "YourOS"; // Replace with your OS name
        var osVersion = "1.0"; // Replace with your OS version

        System.Console.WriteLine($"Operating System: {osName} Version {osVersion}");
        System.Console.WriteLine(" Not Available ");
        System.Console.WriteLine(" Not Available ");

        System.Console.WriteLine("\nPress 'm' to return to the menu or any other key to continue.");

        var keyInfo = System.Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.M)
        {
            System.Console.Clear(); // Clear the screen
            DisplayMenu(); // Show the menu
        }
        else
        {
            System.Console.Clear(); // Clear the screen
            System.Console.WriteLine("Type 'menu' for the menu or enter a command:");
        }
    }
}
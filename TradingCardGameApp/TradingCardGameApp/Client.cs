using System.Text.RegularExpressions;
using TradingCardGameApp.Models;

namespace TradingCardGameApp;

public class Client
{
    public int Port = 10001;

    private HttpClient _client;
    private string _url;
    private string _name;
    private string _password;
    private string _confirmPassword;

    private Player _player;

    /// <summary>
    /// Starts the HTTP Client.
    /// </summary>
    public void Start()
    {
        _client = new HttpClient();
        _url = "http://localhost:" + Port.ToString() + "/";

        RenderStartingMenu();
    }

    /// <summary>
    /// Quits the program.
    /// </summary>
    private void Quit()
    {
        Environment.Exit(0);
    }

    private async Task RenderStartingMenu()
    {
        char c;

        do
        {
            // Clear the Console
            Console.Clear();
            
            Console.WriteLine("(L)ogin (R)egister (Q)uit:");
            c = Console.ReadKey().KeyChar;

        } while (!IsValidStartingMenuInput(c));

        switch (c)
        {
            case 'L':
            case 'l':
                await RenderLoginMenu();
                break;
            case 'R':
            case 'r':
                await RenderRegistrationMenu();
                break;
            case 'Q':
            case 'q':
                Quit();
                break;
        }
    }

    private bool IsValidStartingMenuInput(char c)
    {
        return c == 'L' || c =='l' || c == 'R' || c == 'r' || c == 'Q' || c == 'q';
    }

    private async Task RenderLoginMenu()
    {
        char c;

        do
        {
            // Clear the Console
            Console.Clear();

            Console.WriteLine("(E)nter Credentials (B)ack (Q)uit:");
            c = Console.ReadKey().KeyChar;

        } while (!IsValidLoginMenuInput(c));

        switch (c)
        {
            case 'E':
            case 'e':
                GetNameAndPassword(false);
                break;
            case 'B':
            case 'b':
                RenderStartingMenu();
                break;
            case 'Q':
            case 'q':
                Quit();
                break;
        }

        await SendLoginRequest();
    }

    private bool IsValidLoginMenuInput(char c)
    {
        return c == 'E' || c == 'e' || c == 'B' || c == 'b' || c == 'Q' || c == 'q';
    }

    private async Task SendLoginRequest()
    {
        var player = new Player(_name, _password);

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(player);

        HttpContent content = new StringContent(json);

        var res = await _client.PostAsync(_url + "login", content);

        if (res.IsSuccessStatusCode && res.Headers.Contains("Set-Cookie"))
        {
            // Get body of response
            var responseContent = await res.Content.ReadAsStringAsync();

            // Update player state
            _player = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(responseContent);

            Console.WriteLine(_player.Id);
            Console.WriteLine(_player.Name);
            Console.WriteLine(_player.Password);
            Console.WriteLine(_player.Coins);

            // await RenderSessionMenu();
        }
    }

    private async Task RenderRegistrationMenu()
    {
        char c;

        do
        {
            // Clear the Console
            Console.Clear();

            Console.WriteLine("(E)nter Credentials (B)ack (Q)uit:");
            c = Console.ReadKey().KeyChar;

        } while (!IsValidRegistrationMenuInput(c));

        switch (c)
        {
            case 'E':
            case 'e':
                GetNameAndPassword(true);
                break;
            case 'B':
            case 'b':
                await RenderStartingMenu();
                break;
            case 'Q':
            case 'q':
                Quit();
                break;
        }

        await ValidateRegistration();
    }

    private bool IsValidRegistrationMenuInput(char c)
    {
        return IsValidLoginMenuInput(c);
    }

    private void GetNameAndPassword(bool withConfirmPassword)
    {
        Console.Clear();

        // Get name
        Console.WriteLine("Enter your username:");
        _name = Console.ReadLine();

        // Get password
        Console.WriteLine("Enter password:");

        var password = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        _password = password;

        // Get confirmed password if boolean is set
        if (withConfirmPassword)
        {
            Console.WriteLine("\nConfirm password:");

            var confirmPassword = string.Empty;
            ConsoleKey key2;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key2 = keyInfo.Key;

                if (key2 == ConsoleKey.Backspace && confirmPassword.Length > 0)
                {
                    Console.Write("\b \b");
                    confirmPassword = confirmPassword[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    confirmPassword += keyInfo.KeyChar;
                }
            } while (key2 != ConsoleKey.Enter);

            _confirmPassword = confirmPassword;
        }
    }

    private async Task ValidateRegistration()
    {
        bool isValid = true;

        Console.Clear();

        if (_name.Length < 4)
        {
            Console.WriteLine("The username must be between 4 and 36 characters long!");
            isValid = false;
        }

        if (_password.Length < 4)
        {
            Console.WriteLine("The password must be at least 4 characters long!");
            isValid = false;
        }

        if (_password != _confirmPassword)
        {
            Console.WriteLine("The passwords do not match!");
            isValid = false;
        }

        Console.WriteLine("Press any key to continue!");
        // Wait for any key press
        Console.ReadKey();

        if (isValid)
        {
            await SendRegistrationRequest();
        }
        else
        {
            await RenderRegistrationMenu();
        }
    }

    private async Task SendRegistrationRequest()
    {
        var player = new Player(_name, _password);

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(player);

        HttpContent content = new StringContent(json);

        var res = await _client.PostAsync(_url + "registration", content);

        if (res.IsSuccessStatusCode && res.Headers.Contains("Set-Cookie"))
        {
            // Update player state
            _player = player;

            await RenderSessionMenu();
        }
    }

    private async Task RenderSessionMenu()
    {
        char c;

        do
        {
            // Clear the Console
            Console.Clear();

            Console.WriteLine("(B)uy (C)ards (P)lay (S)core (T)rade (Q)uit:");
            c = Console.ReadKey().KeyChar;

        } while (!IsValidRegistrationMenuInput(c));

        switch (c)
        {
            case 'B':
            case 'b':
                break;
            case 'C':
            case 'c':
                break;
            case 'P':
            case 'p':
                break;
            case 'S':
            case 's':
                break;
            case 'T':
            case 't':
                break;
            case 'Q':
            case 'q':
                Quit();
                break;
        }
    }
}
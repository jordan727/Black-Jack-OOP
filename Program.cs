// Black Jack (OOP) by Jordan A
using System.Text.Json;
#nullable disable

// Console.Clear();
var game = new Game();

while (game.main == true)
{
    game.Menu();
    game.Initialize();
    while (game.complete == false)
    {
        game.Render();
        game.Update();
    }
    game.EndScreen();
}

public class Game
{
    // Handles logic of game
    public List<Cards> deck;
    public Hand player;
    public Hand dealer;
    public bool complete;
    public bool main = true;
    public Random rnd = new Random();

    public void Menu()
    {   
        Console.Clear();
        Console.WriteLine("BLACK JACK");
        Console.WriteLine("[Enter] Start");
        Console.WriteLine("[Esc] Exit");
        EnterToContinue();
    }

    public void Initialize()
    {
        string cardsData = File.ReadAllText("card-data.json");
        deck = JsonSerializer.Deserialize<List<Cards>>(cardsData);

        this.complete = false;

        RandomCard(dealer.hand, 2);
        RandomCard(player.hand, 2);
    }

    public void Update()
    {
        if (dealer.stand == false)
            {
                Console.WriteLine("TYPE 1 TO HIT | TYPE 2 TO STAND");
                string choice = Console.ReadLine();
                if (choice == "1") {
                    RandomCard(player.hand, 1);
                } 
                else if (choice == "2") 
                {
                    if (dealer.sum < 17)
                    {
                        dealer.stand = true;
                    }
                    else
                    {
                        return;
                    }
                } 
            }

        dealer.sum = dealer.hand.Sum(Cards => Cards.Value);
        player.sum = player.hand.Sum(Cards => Cards.Value);



        if (dealer.sum >= 21 && player.sum >= 21)
        {
            this.complete = true;
        }
        
        if (complete)
        {
            for (int n = 0; n < dealer.hand.Count; n++)
            {
                dealer.info += $"|{dealer.hand[n].ToString()}|";
            }
        } 
        else
        {
            dealer.info += $"|{dealer.hand[0].ToString()}|";
            for (int n = 0; n < dealer.hand.Count - 1; n++)
            {
                dealer.info += "|?|";
            }
        }

        for (int n = 0; n < player.hand.Count; n++)
        {
            player.info += $"|{player.hand[n].ToString()}|";
        }    
    }

    public void Render()
    {
        if (this.complete)
        {
            Console.WriteLine("(Dealer)");
            Console.WriteLine(dealer.info);
            Console.WriteLine($"Total: {dealer.sum}");
            Console.WriteLine("(Player)");
            Console.WriteLine(player.info);
            Console.WriteLine($"Total: {player.sum}");
        } 
        else
        {
            Console.WriteLine("(Dealer)");
            Console.WriteLine(dealer.info);
            Console.WriteLine($"Total: ?");
            Console.WriteLine("(Player)");
            Console.WriteLine(player.info);
            Console.WriteLine($"Total: {player.sum}");
        }  
    }

    public Cards RandomCard(List<Cards> hand, int amount)
    {
        Cards random = new Cards();
        for (int n = 0; n < amount; n++)
        {
            int i = rnd.Next(0, deck.Count);
            // ACE
            if (deck[i].Symbol == "A" && (hand.Sum(Cards => Cards.Value) + 11 > 21))
            {
                random = new Cards(deck[i].Type, deck[i].Symbol, 1);
            }
            else
            {
            random = deck[i];
            }
            deck.RemoveAt(i);
            return random; 
        }
    }

    public void EndScreen()
    {

    }

    public void AddDealer()
    {
        if (dealer.sum < 17)
        {
            RandomCard(dealer.hand, 1);
        }
        else if (dealer.stand == true && dealer.sum >= 17)
        {
            return;
        } 
    }

    public void PromptAction()
    {
        Console.WriteLine("[1] Hit | [2] Stand");
        HandleInput();
    }

    public void HandleInput()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.D1:
                    RandomCard(player.hand, 2);
                    break;
            case ConsoleKey.D2:
                    this.complete = false;
                    this.main = false;
                    break;
            default: goto GetInput;       
        }
    }

    public void EnterToContinue()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.Enter:
                    this.complete = false;
                    break;
            case ConsoleKey.Escape:
                    this.complete = false;
                    this.main = false;
                    break;
            default: goto GetInput;       
        }
    }
}

public class Hand
{
    public int sum;
    public string info;
    public bool stand;
    public List<Cards> hand;
    public Hand()
    {
        this.sum = 0;
        this.stand = false;
        this.info = "";
    }
    public void AddCard(Cards card)
    {
        hand.Add(card);
    }
    
}

public class Cards
{
    // Properties
    public string Type { get; set; }
    public string Symbol { get; set; }
    public int Value { get; set; }

    public Cards(string type, string symbol, int value)
    {
        this.Type = type;
        this.Symbol = symbol;
        this.Value = value;
    }
    public override string ToString()
    {
        return $"{Type} {Symbol}";
    }
}

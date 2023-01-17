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
        game.Update();
        game.Render();
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

        player = new Hand();
        dealer = new Hand();
        this.complete = false;

        RandomCard(player, 2);
        RandomCard(dealer, 2);
    }

    public void Update()
    {
        if (dealer.stand == false)
        {
            Console.WriteLine("[1] Hit | [2] Stand");
            HandleInput();
        }

        dealer.Update();
        player.Update();

        if (dealer.sum < 21 && player.sum < 21 || dealer.stand == true)
        {
            complete = false;
        }
        else
        {
            complete = true;
            main = false;
        }


        if (dealer.sum > 17)
        {
            dealer.stand = true;
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

    public void RandomCard(Hand hand, int amount)
    {
        int i;
        // ACE
        for (int n = 0; n < amount; n++)
        {
            i = rnd.Next(0, deck.Count);
            if (deck[i].Symbol == "A" && (hand.sum + 11 > 21))
            {
                hand.AddCard(new Cards(deck[i].Type, deck[i].Symbol, 1));
            }
            else
            {
                hand.AddCard(deck[i]);
            }
            deck.RemoveAt(i);
        }
    }

    public void EndScreen()
    {
        if (dealer.sum == 21 && player.sum != 21) 
        {
            Console.WriteLine("Lose the dealer got to 21 before you");
        } 
        else if (player.sum == 21 && dealer.sum != 21) 
        {
            Console.WriteLine("Win you got to 21 before the dealer");
        } 
        else if (player.sum > 21 && dealer.sum > 21)
        {
            Console.WriteLine("Player Bust");
        }
        else if (dealer.sum > 21) 
        {
            Console.WriteLine("Dealer Bust");
        } 
        else if (player.sum > 21) 
        {
            Console.WriteLine("Player Bust");
        } 
        else if (player.sum == dealer.sum)
        {
            Console.WriteLine("Tie");
        } 
        else if (dealer.sum > player.sum)
        {
            Console.WriteLine("Dealer Win");
        } 
        else if (player.sum > dealer.sum)
        {
            Console.WriteLine("Player Win");
        } 
    }

    public void AddDealer()
    {
        if (dealer.sum < 17)
        {
            RandomCard(dealer, 1);
        }
        else if (dealer.stand == true && dealer.sum >= 17)
        {
            return;
        } 
    }

    public void HandleInput()
    {
    GetInput:
        ConsoleKey key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.D1:
                    RandomCard(player, 1);
                    break;
            case ConsoleKey.D2:
                    player.stand = true;
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
        this.hand = new List<Cards>();
    }
    public void AddCard(Cards card)
    {
        hand.Add(card);
    }
    
    public void Update()
    {
        this.sum = this.hand.Sum(Cards => Cards.Value);
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

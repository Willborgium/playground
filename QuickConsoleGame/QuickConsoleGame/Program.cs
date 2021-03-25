using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickConsoleGame
{
    public class Fighter
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public IEnumerable<FighterAttack> Attacks { get; set; }
    }

    public class FighterAttack
    {
        public Attack Attack { get; }
        public int MaxUses { get; }
        public int RemainingUses { get; set; }
        public FighterAttack(Attack attack, int maxUses)
        {
            Attack = attack;
            RemainingUses = MaxUses = maxUses;
        }
    }

    public class Attack
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public Attack(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
    }

    public class Program
    {
        private static readonly Random Random = new Random();

        private static readonly string[] Names =
        {
            "John",
            "Ashley",
            "Steve",
            "Jenna"
        };

        private static readonly Attack[] Attacks =
        {
            new Attack("Punch", 5),
            new Attack("Kick", 10),
            new Attack("Slap", 3),
            new Attack("Knee", 7),
            new Attack("Elbow", 8),
            new Attack("Headbutt", 9),
            new Attack("Tackle", 15),
            new Attack("Take Down", 20),
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Game start!");

            Console.WriteLine("Your name");
            var name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Clear();
                Console.WriteLine("Your name");
                name = Console.ReadLine();
            }

            var user = new Fighter
            {
                Name = name,
                Attacks = GetRandomAttacks(),
                MaxHealth = 100,
                CurrentHealth = 100
            };

            var computer = new Fighter
            {
                Name = Names.GetRandom(),
                MaxHealth = Random.Next(50, 100),
                Attacks = GetRandomAttacks()
            };
            computer.CurrentHealth = computer.MaxHealth;

            Console.Clear();

            Console.WriteLine($"{user.Name} will be fighting {computer.Name}!");
            Console.WriteLine("Press any key to begin!");
            Console.ReadKey();

            while (computer.CurrentHealth > 0 && user.CurrentHealth > 0)
            {
                Console.Clear();
                RenderFighter(computer);
                Console.WriteLine();
                RenderFighter(user);

                Console.WriteLine("Choose your attack");
                var index = 1;
                foreach (var attack in user.Attacks)
                {
                    Console.WriteLine($"{index}) {attack.Attack.Name} | {attack.Attack.Damage} | {attack.RemainingUses}/{attack.MaxUses}");
                    index++;
                }

                if (!user.Attacks.All(a => a.RemainingUses == 0))
                {
                    Console.WriteLine("Choose your attack");
                    var moveInput = Console.ReadLine();

                    if (!int.TryParse(moveInput, out var chosenMoveIndex))
                    {
                        Console.WriteLine("That's not a valid attack!");
                        Console.ReadKey();
                        continue;
                    }

                    if (chosenMoveIndex < 1 || chosenMoveIndex > 4)
                    {
                        Console.WriteLine("That's not a valid attack!");
                        Console.ReadKey();
                        continue;
                    }

                    var userMove = user.Attacks.ElementAt(chosenMoveIndex - 1);

                    if (userMove.RemainingUses == 0)
                    {
                        Console.WriteLine("You cannot use that attack!");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine($"{user.Name} attacks with {userMove.Attack.Name} for {userMove.Attack.Damage} damage");
                    computer.CurrentHealth -= userMove.Attack.Damage;
                    userMove.RemainingUses -= 1;
                }

                if (computer.CurrentHealth <= 0)
                {
                    continue;
                }

                var computerMove = computer.Attacks
                    .Where(a => a.RemainingUses > 0)
                    .GetRandom();

                if (default(KeyValuePair<Attack, int>).Equals(computerMove))
                {
                    Console.WriteLine($"{computer.Name} cannot attack!");
                }
                else
                {
                    Console.WriteLine($"{computer.Name} attacks with {computerMove.Attack.Name} for {computerMove.Attack.Damage} damage");
                    user.CurrentHealth -= computerMove.Attack.Damage;
                    computerMove.RemainingUses -= 1;
                }

                Console.ReadKey();
            }

            Console.Clear();
            RenderFighter(computer);
            Console.WriteLine();
            RenderFighter(user);

            if (user.CurrentHealth <= 0)
            {
                Console.WriteLine($"{computer.Name} wins!");
            }
            else
            {
                Console.WriteLine($"{user.Name} wins!");
            }

            Console.WriteLine("Game end!");
        }

        private static void RenderFighter(Fighter f)
        {
            var increment = f.MaxHealth / 10;
            var healthBars = f.CurrentHealth / increment;
            var health = string.Join("", Enumerable.Range(0, healthBars).Select(x => "|"));
            var emptyBars = 10 - healthBars;
            var empty = "";
            if (emptyBars > 0)
            {
                empty = string.Join("", Enumerable.Range(0, emptyBars).Select(x => "-"));
            }
            Console.WriteLine(f.Name);
            Console.WriteLine($"\t[{health}{empty}] ({f.CurrentHealth}/{f.MaxHealth})");
        }

        private static IEnumerable<FighterAttack> GetRandomAttacks()
        {
            var output = new List<FighterAttack>();
            while (output.Count < 4)
            {
                var attack = Attacks.GetRandom(output.Select(o => o.Attack));
                output.Add(new FighterAttack(attack, Random.Next(3, 6)));
            }
            return output;
        }
    }

    public static class EnumerableExtensions
    {
        private static readonly Random Random = new Random();

        public static T GetRandom<T>(this IEnumerable<T> collection, IEnumerable<T> exceptValues)
        {
            var reducedCollection = collection
                .Except(exceptValues);

            if (!reducedCollection.Any())
            {
                return default;
            }

            return reducedCollection.ElementAt(Random.Next(0, reducedCollection.Count()));
        }

        public static T GetRandom<T>(this IEnumerable<T> collection) => GetRandom(collection, Enumerable.Empty<T>());
    }
}

namespace FunManipulator;

public static class Testing
{
    public enum Messages
    {
        Great = 0,     // You're just great!
        Nice = 1,      // You look nice today!
        Claws = 2,     // Are those claws natural?
        Spiffy = 3,    // You're super spiffy!
        Wonderful = 4, // Have a wonderful day!
        Sweet = 5,     // Is this as sweet as you?
        Hug = 6,       // (An illustration of a hug.)
        Love = 7       // Love yourself! I love you!
    }

    public static Search.IElement Message(Messages message)
    {
        return new Search.ElementRandomInRange(8, (double)message, (double)(message + 1));
    }

    public static int Calls(Messages message, int frames)
    {
        switch (message) {
            case Messages.Great:     return 90 * frames;
            case Messages.Nice:      return 94 * frames;
            case Messages.Claws:     return 102 * frames;
            case Messages.Spiffy:    return 94 * frames;
            case Messages.Wonderful: return 96 * frames;
            case Messages.Sweet:     return 102 * frames;
            case Messages.Hug:       return 108 * frames;
            case Messages.Love:      return 106 * frames;
        }
        return 0;
    }
    
    public static int Calls(Messages message, int frames, int framesunmashed)
    {
        if (framesunmashed == 1)
            return Calls(message, frames) + 2;
        if (framesunmashed == 2)
            return Calls(message, frames) + 6;
        if (framesunmashed == 3)
            return Calls(message, frames) + 12;

        return Calls(message, frames) + (framesunmashed * framesunmashed) + framesunmashed;
    }

    public static void Run()
    {
        Search.Pattern pattern = new();
        pattern.AllowedErrors = 0;

        //pattern.Elements.Add(Message(Messages.Love));
        //pattern.Elements.Add(new Search.ElementUnknown(true));
        //pattern.Elements.Add(new Search.ElementChooseIndex(10, 4));
        //new Search.ElementRandomInRange(8, message, (message + 1));
        for (int i = 0; i < 23; i++)
        {
            pattern.Elements.Add(new Search.ElementUnknown(false)); // x
            pattern.Elements.Add(new Search.ElementRandomInRange(1100, 618, 1100)); // y
            var hspeed = new Search.ElementRandomInRange(6, 1.05, 4.85);
            hspeed.Inverted = true;
            pattern.Elements.Add(hspeed);
            pattern.Elements.Add(new Search.ElementUnknown(true)); // scale x2
        }

        uint seed;
        int ind;
        List<(uint, int)> seedList = new();
        if (Search.TryFindSeedWithinRange(pattern, 0, 1<<30, out seed, out ind, seedList))
        {
            Console.WriteLine($"Found seed {seed} at {ind}");
            RNG jacksrng = new(seed);
            for (int i = 0; i < pattern.GetSize(); i++) jacksrng.Next();
            Console.WriteLine("The next ten choose results, offset by 2:");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("- " + (jacksrng.Next() % 10));
                jacksrng.Next();
            }
        }
        else
        {
            if (seedList.Count != 0)
            {
                Console.WriteLine("Found multiple seeds:");
                foreach ((uint, int) s in seedList)
                {
                    Console.WriteLine($" -> {s.Item1} (at {s.Item2})");
                    RNG jacksrng = new(s.Item1);
                    for (int i = 0; i < pattern.GetSize(); i++) jacksrng.Next();
                    Console.WriteLine(" The next ten choose results, offset by 2:");
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine(" - " + (jacksrng.Next() % 10));
                        jacksrng.Next();
                    }
                }
            }
            else
            {
                Console.WriteLine("No seeds matched!");
            }
        }
    }
}
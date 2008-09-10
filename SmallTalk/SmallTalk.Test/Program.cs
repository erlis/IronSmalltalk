using System;

namespace SmallTalk.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SmallCharacter ch = new SmallCharacter("$A");
            Console.WriteLine(ch);
            Console.WriteLine(ch.SendMessage("asLowercase"));

            SmallInteger n = new SmallInteger("12345");
            Console.WriteLine(n);
            Console.WriteLine(n.SendMessage("negated"));
        }
    }
}
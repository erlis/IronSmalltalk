using IronSmalltalk.Compiler;
using Microsoft.Scripting.Hosting;
using System;
using Microsoft.Scripting;

namespace IronSmalltalk.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //LexerTest();
            ObjectTest();
            //ParserTest();
        }

        private static void ParserTest()
        {
            // Create runtime:
            ScriptRuntime runtime = ScriptRuntime.Create();

            runtime.LoadAssembly(typeof(string).Assembly);

            // Load engine:
            ScriptEngine engine = runtime.GetEngine(typeof(IronSmalltalkLanguageContext));

            // Create a scope at global level:
            ScriptScope globals = engine.CreateScope();

            // Execute command:
            ScriptSource src = engine.CreateScriptSourceFromString("$A. $B. $C.", SourceCodeKind.Statements);
            object obj = src.Execute(globals);
            Console.WriteLine(obj);

            // Shutdown engine:
            engine.Shutdown();
        }

        private static void ObjectTest()
        {
            SmallCharacter ch = new SmallCharacter("$P");
            Console.WriteLine(ch);
            Console.WriteLine(ch.SendMessage("value"));
            Console.WriteLine(ch.SendMessage("asLowercase"));
        }

        private static void LexerTest()
        {
            string code =
            "s := 'hello world!'.\n" +
            "s set: 'HERE: ''IT'' IS." +
            "haha! !($%(#&.\"'''; asLowercase. -3.14159265359 $c $A $0 $! 3r209E1" +
            "1e1 3r1e1 3rAEe45 5r-123.53e9 Ar12.7" +
            "# #12 #AB12 12: asLowercase:" +
            "'hello ''\n'''' world!' setRed:green:blue:";

            Lexer lexer = new Lexer("LexerGrammar.xml");
            Lexer.TokenStream tokens = lexer.Tokenize(code);
            while (!tokens.AtEnd)
            {
                Console.WriteLine(tokens.Read());
            }
        }

        private static void Test()
        {
            // Create runtime:
            ScriptRuntime runtime = ScriptRuntime.Create();

            // Load engine:
            ScriptEngine engine = runtime.GetEngine(typeof(IronSmalltalkLanguageContext));

            // Execute command:
            ScriptSource src = engine.CreateScriptSourceFromString("<Any script here>");
            src.Execute();

            // Shutdown engine:
            engine.Shutdown();
        }
    }
}
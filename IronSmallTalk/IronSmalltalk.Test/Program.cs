using IronSmalltalk.Compiler;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Text;

namespace IronSmalltalk.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //LexerTest();
            //ObjectTest();
            //ParserTest();

            RunConsole();
        }

        private static void RunConsole()
        {
            string command;

            Console.WriteLine("IronSmalltalk v0.1");

            // Create runtime:
            ScriptRuntime runtime = ScriptRuntime.Create();

            runtime.LoadAssembly(typeof(string).Assembly);

            // Load engine:
            ScriptEngine engine = runtime.GetEngine(typeof(IronSmalltalkLanguageContext));

            // Create a scope at global level:
            ScriptScope globals = engine.CreateScope();

            while (true)
            {
                // Write prompt:
                Console.Write(">> ");
                command = ReadCommand();

                string lowerCommand = command.ToLower();

                // Exit:
                if (lowerCommand.Equals("exit"))
                {
                    break;
                }

                // Execute commands:
                try
                {
                    Console.WriteLine(RunProgram(engine, command, globals));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
                }
            }

            // Shutdown engine:
            engine.Shutdown();
        }

        /// <summary>
        /// Run a command on a DLR engine.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="command"></param>
        /// <param name="globals"></param>
        /// <returns></returns>
        private static object RunProgram(ScriptEngine engine, string command, ScriptScope globals)
        {
            ScriptSource src = engine.CreateScriptSourceFromString(command, SourceCodeKind.Statements);
            return src.Execute(globals);
        }
        
        /// <summary>
        /// Read a multi-line command.
        /// </summary>
        /// <returns></returns>
        private static string ReadCommand()
        {
            // Loop on each line:
            StringBuilder command = new StringBuilder();
            while (true)
            {
                // Read a line:
                string line = Console.ReadLine();
                command.Append(line);

                // Special command:
                string value = command.ToString().ToLower();
                if (value.Equals("exit"))
                {
                    break;
                }

                // Stop if not ended by a ';'
                int length = line.Length;
                if (length == 0)
                {
                    break;
                }
                char lastChar = line[length - 1];
                if (lastChar != ';')
                {
                    break;
                }

                // Wait for next line:
                command.AppendLine();
                Console.Write("> ");
            }

            return command.ToString();
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
            ScriptSource src = engine.CreateScriptSourceFromString("$A. $B. $C asLowercase; next.", SourceCodeKind.Statements);
            object obj = src.Execute(globals);
            Console.WriteLine(obj);

            // Shutdown engine:
            engine.Shutdown();
        }

        private static void ObjectTest()
        {
            SmallCharacter ch = new SmallCharacter("$P");
            Console.WriteLine(ch);
            Console.WriteLine(ch.SendMessage(new SmallSymbol("#value")));
            Console.WriteLine(ch.SendMessage(new SmallSymbol("#asLowercase")));
        }

        private static void LexerTest()
        {
            string code =
            "s := 'hello world!'.\n" +
            "s set: 'HERE: ''IT'' IS." +
            "haha! !($%(#&.\"'''; asLowercase. -3.14159265359 $c $A $0 $! 3r209E1" +
            "1e1 3r1e1 3rAEe45 5r-123.53e9 Ar12.7" +
            "# #12 #AB12 12: asLowercase:" +
            "'hello ''\n'''' world!' setRed:green:blue:" +
            "#symbol #'this is a symbol' #'Trey''s symbol'";

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
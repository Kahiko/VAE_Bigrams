// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;


namespace bigram_parsing;

internal partial class Program
{

    public static void Main(string[] args)
    {
        BigramParser mBigramParser = new();
        _ = mBigramParser.Parse(args);
    }

}
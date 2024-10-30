

using System.Text.RegularExpressions;

namespace bigram_parsing;

/// Naming conventions:
///     lower case for methods indicate they are private
///     upper case for methods indicate they are public
///     variables starting with lower case m indicate they scoped to the method
///     camel case for variables indicate they are parameters

public partial class BigramParser
{

    /// <summary>
    /// Checks the validity of the file name both that it is not null and that it exists
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private void checkFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) 
        { 
            showHelp();
            throw new ArgumentNullException("The file name cannot be null.", "-file"); 
        }
        if (!File.Exists(fileName)) 
        { 
            showHelp();
            throw new ArgumentException("The file does not exist.", "-file"); 
        }
    }

    /// <summary>
    /// Checks the validity of the string value
    /// </summary>
    /// <param name="stringValue"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void checkString(string stringValue)
    {
        if (string.IsNullOrEmpty(stringValue)) 
        { 
            showHelp();
            throw new ArgumentNullException("The string value cannot be null.", "-string"); 
        }
    }

    /// <summary>
    /// Returns the value of the desired argument
    /// </summary>
    /// <param name="args"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private string getArgument(string[] args, string name)
    {
        string mRetVal = string.Empty;
        for (int i = 0; i < args.Length; i++)
        {
            string argument = args[i];
            if (argument.Equals(name, StringComparison.CurrentCultureIgnoreCase) && i < args.Length)
            {
                mRetVal = args[i + 1];
                break;
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Parses the arguments and then processes the file and or string
    /// </summary>
    /// <param name="args"></param>
    /// <returns>int as the total number of bigrams</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <remarks>Used by Main and the xUnit tests</remarks>
    public int Parse(string[] args)
    {
        bool mStatus = false;
        int mRetVal = 0;
        string mFileName = getArgument(args, "-file");
        string mString = getArgument(args, "-string");
        if (!string.IsNullOrEmpty(mFileName))
        {
            checkFile(mFileName);
            mRetVal = processFile(mFileName);
            mStatus = true;
        }
        if (!string.IsNullOrEmpty(mString))
        {
            checkString(mString);
            List<string> mBigrams = parseString(mString);
            mRetVal = showOutput(mBigrams);
            mStatus = true;
        }
        if (mStatus == false)
        {
            throw new ArgumentException("At least one argument must be specified.", "-file or -string");
        }
        return mRetVal;
    }
    
    /// <summary>
    /// Opens a text file and reads it into a string.
    /// The string is then processed by parseString and the number of bigrams
    /// are returned.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>int as the total number of bigrams</returns>
    /// <remarks>Used by Parse</remarks>
    private int processFile(string fileName)
    {
        // Open the text file using a stream reader.
        using StreamReader reader = new(fileName);
        // Read the stream as a string.
        string mText = reader.ReadToEnd();
        // Replace line-breaks with spaces.
        mText = mText.Replace("\r\n", " ");
        // Write the text to the console.
        List<string> mBigrams = parseString(mText);
        return showOutput(mBigrams);
    }
    
    /// <summary>
    /// Parses a given string into a list of bigrams (A bigram is a pair of consecutive words).
    /// </summary>
    /// <param name="stringValue">The input string to be parsed into bigrams.</param>
    /// <returns>A list of bigrams extracted from the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null or empty.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when a sentence in the input string contains fewer than two words.
    /// </exception>
    /// <remarks>
    /// The function splits the input string into sentences, removes non-alphanumeric 
    /// characters, and generates bigrams for each sentence. 
    /// Sentences with fewer than two words are not processed.
    /// </remarks>
    private List<string> parseString(string stringValue)
    {
        List<string> mRetVal = [];
        if (string.IsNullOrEmpty(stringValue))
        {
            throw new ArgumentNullException(stringValue, "The string value cannot be null.");
        }
        Regex mRegex = AlphaNumbericOnly();
        List<string> mSentences = stringValue.Split(".").ToList();
        foreach (string mSentence in mSentences)
        {
            if (!string.IsNullOrEmpty(mSentence))
            {
                string mStringValue = mRegex.Replace(mSentence, "");
                List<string> mWords = mStringValue.Trim().Split(' ').ToList();
                if (mStringValue.Trim().Length != 0 && mWords.Count < 2)
                {
                    throw new ArgumentException("The strings value must contain at least two words.", "-string");
                }
                for (int i = 0; i < mWords.Count - 1; i++)
                {
                    mRetVal.Add(mWords[i].Trim() + " " + mWords[i + 1].Trim());
                }
            }
        }
        return mRetVal;
    }

    private void showHelp()
    {
        string mTab = "    ";
        Console.WriteLine("Usage:");
        Console.WriteLine(mTab + "-file \"<path_filename>\"");
        Console.WriteLine(mTab + mTab + "OR");
        Console.WriteLine(mTab + "-string \"a string to parse\"" + Environment.NewLine);
    }

    /// <summary>
    /// Displays the bigram counts
    /// </summary>
    /// <param name="bigrams"></param>
    /// <returns></returns>
    private int showOutput(List<string> bigrams)
    {
        List<string> mDistinctBigrams = bigrams.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
        Dictionary<string, int> mBigramsDictionary = new Dictionary<string, int>();
        foreach (string mBigram in mDistinctBigrams)
        {
            int mCount = bigrams.Count(x => x.Equals(mBigram, StringComparison.CurrentCultureIgnoreCase));
            mBigramsDictionary.Add(mBigram, mCount);
        }
        foreach (KeyValuePair<string, int> kvp in mBigramsDictionary)
        {
            // I'm not a fan of altering the data but it was all lower case in the example output
            Console.WriteLine("\"" + kvp.Key.ToLower() + "\": " + kvp.Value);
        }
        // Next line was not asked for but I found it hellpfull during testing
        // Console.WriteLine("Total bigrams: " + mDistinctBigrams.Count);
        return mDistinctBigrams.Count;
    }

    [GeneratedRegex("[^a-zA-Z0-9 -]")]
    private static partial Regex AlphaNumbericOnly();
}
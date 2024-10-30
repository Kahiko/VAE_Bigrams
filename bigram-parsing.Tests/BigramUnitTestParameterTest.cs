using Xunit.Abstractions;

namespace bigram_parsing.Tests;

public class BigramUnitTestParameterTest
{
    private readonly BigramParser m_BigramParser = new();

    [Theory]
    [InlineData("-string", "The ")]
    [InlineData("-file", "notfound.txt")]
    [InlineData("-wringParameterName", "notfound.txt")]
    public void BigramUnitTestParameterTest_FailIncorrectParameters(string argument, string value)
    {
        // Arrange
        string[] args = { argument, value };
        // Act
        Assert.Throws<ArgumentException>(() => m_BigramParser.Parse(args));
    }


    [Theory]
    [InlineData("-string", "The quick brown fox and the quick blue hare.")]
    [InlineData("-file", "D:/Development/Tutorials/bigram-parsing/single.txt")]
    public void BigramUnitTestParameterTest_Valid(string argument, string value)
    {
        // Arrange
        string[] args = { argument, value };
        int mBigramCount = m_BigramParser.Parse(args);
        // Act
        Assert.True(mBigramCount == 7, "The actual count was ${mBigramCount}");
    }

    [Fact]
    public void BigramUnitTestParameterTest_ValidMultipleValues()
    {
        // Arrange
        string[] args = { "-file", "D:/Development/Tutorials/bigram-parsing/bigram.txt" };
        int mBigramCount = m_BigramParser.Parse(args);
        // Act
        Assert.True(mBigramCount == 7, "The actual count was ${mBigramCount}");
    }
}
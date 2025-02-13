namespace WordFinderTests;

public class WordFinderUnitTests // Does not contains async methods for the sake of simplicity
{
    [Fact]
    public void WordFinder_Constructor_OK()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { "ab", "cd"  };

        // Act
        WordFinder finder = new WordFinder(matrix);

        // Assert
        Assert.NotNull(finder);
    }
    
    [Fact]
    public void WordFinder_Matrix_Empty()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { };

        // Act
        Action act = () => new WordFinder(matrix);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void WordFinder_NotSquareMatrix()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { "ab", "cde" };

        // Act
        Action act = () => new WordFinder(matrix);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void WordFinder_Find()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { "ab", "cd" };
        IEnumerable<string> wordStream = new List<string> { "ab", "cd" };
        WordFinder finder = new WordFinder(matrix);

        // Act
        var result = finder.Find(wordStream);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void WordFinder_Find_Top10()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { 
            "abc",
            "aaa",
            "abc" };

        IEnumerable<string> wordStream = new List<string> { "aa", "bc" };
        WordFinder finder = new WordFinder(matrix);

        // Act
        var result = finder.Find(wordStream);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("aa", result.First());
        Assert.Equal("bc", result.Last());
    }

    [Fact]
    public void WordFinder_Find_DoesNotFind()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { "ab", "cd" };
        IEnumerable<string> wordStream = new List<string> { "t", "x" };
        WordFinder finder = new WordFinder(matrix);

        // Act
        var result = finder.Find(wordStream);

        // Assert
        Assert.Equal(0, result.Count());
    }

    [Fact]
    public void WordFinder_Find_WithoutStreamOfWords()
    {
        // Arrange
        IEnumerable<string> matrix = new List<string> { "ab", "cd" };
        IEnumerable<string> wordStream = new List<string> {  };
        WordFinder finder = new WordFinder(matrix);

        // Act
        var result = finder.Find(wordStream);

        // Assert
        Assert.Equal(0, result.Count());
    }
}
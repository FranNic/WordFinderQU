using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[ThreadingDiagnoser]
[SimpleJob(RunStrategy.ColdStart, iterationCount: 10)]
public class Program
{
    static void Main()
    {   // Benchmarks to see performance
        var summary = BenchmarkRunner.Run<Program>();

    }

    [Benchmark]
    public void RunBigMatrixWithSomeWords()
    {
        string filePath = "Matrix_64x64.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(filePath);

        // Words to search for
        List<string> wordStream = new List<string> { "snow", "snowfall", "winter", "fog", "owl" };

        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        var result = finder.Find(wordStream);

        return;

    }

    [Benchmark]
    public void RunBigMatrixWithLotsOfWords()
    {
        string matrixFilePath = "Matrix_64x64.txt";
        string wordStreamFilePath = "WordStream.txt";

        if (!File.Exists(matrixFilePath))
        {
            Console.WriteLine($"Error: File {matrixFilePath} not found!");
            return;
        }
        if (!File.Exists(wordStreamFilePath))
        {
            Console.WriteLine($"Error: File {wordStreamFilePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(matrixFilePath);

        // Words to search for
        IEnumerable<string> wordStream = File.ReadAllLines(wordStreamFilePath);
        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        var result = finder.Find(wordStream);

        return;
    }

    [Benchmark]
    public void RunBigMatrixWithLotsOfWordsAsync()
    {
        string matrixFilePath = "Matrix_64x64.txt";
        string wordStreamFilePath = "WordStream.txt";

        if (!File.Exists(matrixFilePath))
        {
            Console.WriteLine($"Error: File {matrixFilePath} not found!");
            return;
        }
        if (!File.Exists(wordStreamFilePath))
        {
            Console.WriteLine($"Error: File {wordStreamFilePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(matrixFilePath);

        // Words to search for
        IEnumerable<string> wordStream = File.ReadAllLines(wordStreamFilePath);
        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        _ = finder.FindAsync(wordStream);

        return;
    }

    [Benchmark]
    public void RunSmallMatrixWithSomeWords()
    {
        string filePath = "Matrix_10x10.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(filePath);

        // Words to search for
        List<string> wordStream = new List<string> { "snow", "snowfall", "winter", "fog", "owl" };

        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        var result = finder.Find(wordStream);

        return;

    }

    [Benchmark]
    public void RunSmallMatrixWithLotsOfWords()
    {
        string matrixFilePath = "Matrix_10x10.txt";
        string wordStreamFilePath = "WordStream.txt";

        if (!File.Exists(matrixFilePath))
        {
            Console.WriteLine($"Error: File {matrixFilePath} not found!");
            return;
        }
        if (!File.Exists(wordStreamFilePath))
        {
            Console.WriteLine($"Error: File {wordStreamFilePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(matrixFilePath);

        // Words to search for
        IEnumerable<string> wordStream = File.ReadAllLines(wordStreamFilePath);
        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        var result = finder.Find(wordStream);

        return;
    }

    [Benchmark]
    public void RunSmallMatrixWithLotsOfWordsAsync()
    {
        string matrixFilePath = "Matrix_10x10.txt";
        string wordStreamFilePath = "WordStream.txt";

        if (!File.Exists(matrixFilePath))
        {
            Console.WriteLine($"Error: File {matrixFilePath} not found!");
            return;
        }
        if (!File.Exists(wordStreamFilePath))
        {
            Console.WriteLine($"Error: File {wordStreamFilePath} not found!");
            return;
        }

        // Read matrix from file
        IEnumerable<string> matrix = File.ReadAllLines(matrixFilePath);

        // Words to search for
        IEnumerable<string> wordStream = File.ReadAllLines(wordStreamFilePath);
        // Run benchmark
        WordFinder finder = new WordFinder(matrix);

        _ = finder.FindAsync(wordStream);

        return;
    }

}

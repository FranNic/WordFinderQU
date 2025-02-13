using BenchmarkDotNet.Attributes;

using System.Collections.Concurrent;

public class WordFinder
{
    private readonly char[][] _charMatrix;
    private readonly string[] _columns;
    private readonly string[] _rows;
    private readonly int _rowsInt;
    private readonly int _colsInt;
    public static readonly int d = 256;
    private HashSet<string> uniqueWords = new HashSet<string>();

    private Dictionary<string, int> foundWords = new Dictionary<string, int>();
    private ConcurrentDictionary<string, int> foundWordsAsync = new ConcurrentDictionary<string, int>();
    private List<Task> tasks = new List<Task>();

    public WordFinder(IEnumerable<string> matrix)
    {
        // check matrix is not empty
        if (!matrix.Any())
            throw new ArgumentException("Matrix must not be empty");

        // check matrix is square
        if (matrix.Any(row => row.Length != matrix.Count()))
            throw new ArgumentException("Matrix must be square");

        _rows = matrix.Select(r => r.ToLower()).ToArray();
        _charMatrix = matrix.Select(row => row.ToLower().ToCharArray()).ToArray();
        _rowsInt = _charMatrix.Length;
        _colsInt = _rowsInt;

        // Precompute column storage as rows
        _columns = new string[_colsInt];
        for (int col = 0; col < _colsInt; col++)
        {
            char[] colChars = new char[_rowsInt];
            for (int row = 0; row < _rowsInt; row++)
                colChars[row] = _charMatrix[row][col];

            _columns[col] = new string(colChars);
        }
    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        // Following rule from the task description:
        // If any word in the word stream is found more than once within the stream, the search results should count it only once
        uniqueWords = wordstream.Distinct().ToHashSet();

        // Separate search in rows and columns
        RowSearch();
        ColumnSearch();

        return foundWordsAsync.OrderByDescending(x => x.Value)
                         .Take(10)
                         .Select(x => x.Key);
    }

    /// <summary>
    /// Robin-Karp search algorithm for pattern matching -> see https://www.geeksforgeeks.org/rabin-karp-algorithm-for-pattern-searching/
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="txt"></param>
    /// <param name="q"></param>
    private void RobinKarpSearch(string pattern, string txt, int q) // Used this to avoid scenario of multiple words in the same row (for example: "snow" and "snowfall")
    {
        int M = pattern.Length;
        int N = txt.Length;
        int i, j;
        int p = 0; // hash value for pattern
        int t = 0; // hash value for txt
        int h = 1;

        // The value of h would be "pow(d, M-1)%q"
        for (i = 0; i < M - 1; i++)
            h = (h * d) % q;

        // Calculate the hash value of pattern and first
        // window of text
        for (i = 0; i < M; i++)
        {
            p = (d * p + pattern[i]) % q;
            t = (d * t + txt[i]) % q;
        }

        // Slide the pattern over text one by one
        for (i = 0; i <= N - M; i++)
        {
            // Check the hash values of current window of
            // text and pattern. If the hash values match
            // then only check for characters one by one
            if (p == t)
            {
                /* Check for characters one by one */
                for (j = 0; j < M; j++)
                {
                    if (txt[i + j] != pattern[j])
                        break;
                }

                // if p == t and pattern[0...M-1] = txt[i, i+1,
                // ...i+M-1]
                if (j == M)
                {
                        foundWordsAsync.AddOrUpdate(pattern, 1, (key, oldValue) => oldValue + 1);
                    
                }
            }

            // Calculate hash value for next window of text:
            // Remove leading digit, add trailing digit
            if (i < N - M)
            {
                t = (d * (t - txt[i] * h) + txt[i + M]) % q;

                // We might get negative value of t,
                // converting it to positive
                if (t < 0)
                    t = (t + q);
            }
        }
    }


    private void RowSearch()
    {
        // Check all precomputed rows
        // NOTE: Decision over row vs unique words is based on the assumption that the number of unique words is much smaller than the POTENTIALY number of rows
        foreach (var row in _rows)
        {
            foreach (var word in uniqueWords)
            {
                RobinKarpSearch(word, row, 101); // 101 = prime number
            }
        }
    }

    private void ColumnSearch()
    {
        // Check all columns (transposed precomputed rows)
        foreach (var col in _columns)
        {
            foreach (var word in uniqueWords)
            {
                RobinKarpSearch(word, col, 101); // 101 = prime number
            }
        }
    }


    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Took the liberty to do it async as well but this is an extra to the interface that was provided in the challenge.
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public async Task<IEnumerable<string>> FindAsync(IEnumerable<string> wordstream)
    {
        // Following rule from the task description:
        // If any word in the word stream is found more than once within the stream, the search results should count it only once
        uniqueWords = wordstream.Distinct().ToHashSet();

        var cts = new CancellationTokenSource();

        // Separate search in rows and columns in two different tasks
        tasks.Add(RowSearchAsync(cts.Token));
        tasks.Add(ColumnSearchAsync(cts.Token));

        Task.WaitAll(tasks.ToArray());

        return foundWords.OrderByDescending(x => x.Value)
                         .Take(10)
                         .Select(x => x.Key);
    }

    private async Task RowSearchAsync(CancellationToken cancellationToken)
    {
        // Check all precomputed rows in parallel
        await Parallel.ForEachAsync(_rows, async (row,cancellationToken) =>
        {
            foreach (var word in uniqueWords)
            {
                RobinKarpSearch(word, row, 101); // 101 = prime number // NOTE: this should also be async but for the sake of simplicity I'm leaving it as is
            }
        });

    }

    private async Task ColumnSearchAsync(CancellationToken cancellationToken)
    {
        // Check all columns (transposed precomputed rows) in parallel
        await Parallel.ForEachAsync(_columns, async (col, cancellationToken) =>
        {
            foreach (var word in uniqueWords)
            {
                RobinKarpSearch(word, col, 101); // 101 = prime number // NOTE: this should also be async but for the sake of simplicity I'm leaving it as is
            }
        });
    }
}
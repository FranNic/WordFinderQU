# WordFinderQU
A DotNet challenge for QU

## Overview
Presented with a character matrix and a large stream of words, WordFinder Class searches the matrix to look for the words from the word stream. 
Words are looked horizontally, from left to right, and vertically, from top to bottom. 

## Preload 
For the sake of simplicity columns are treated as rows in different variable.

`private readonly string[] _columns;
 private readonly string[] _rows;`
    
HashSet was used to improve IEnumerable performance as we don't need order and duplicates are erased.

`private HashSet<string> uniqueWords = new HashSet<string>();`

## Robin Karp
Used algorithm comes from the following hypothesis:
- Finding substrings multiple times inside the matrix at the same row/column (e.g. FOGmFOGtuv -> FOG found twice).
- Shorter words inside of longer words (e.g. SNOWFALL -> SNOW, NOW, FALL, ALL).

## Scenarios
Small matrix (10x10) vs big matrix (64x64)
- Small matrix with some words: 10x10 matrix with defined collection of 5 words `{ "snow", "snowfall", "winter", "fog", "owl" }`.
- Small matrix with lots of words: 10x10 matrix with Bip39 collection of 2048 words. (see https://github.com/bitcoin/bips/blob/master/bip-0039/english.txt)
- Small matrix with lots of words async: same as above but asynchronous and parallel use.

  Big matrix contains the same scenarios.

## Files
- WordStream.txt contains all words from BIP39 in english. (see https://github.com/bitcoin/bips/blob/master/bip-0039/english.txt)
- Matrix_10x10.txt contains a manually built matrix with words to be found in UPPERCASE. These words are:
  - Now - 4 times
  - Snow - 4 times
  - Fog - 2 times
  - Snowfall - 1 time
  - All - 1 time
  - Own - 1 time
  - Fall - 1 time
  - Owl - 1 time
- Matrix_64x64.txt contains a manually built matrix with words to be found in UPPERCASE.

## Benchmarks
| Method                             | Mean       | Error     | StdDev    | Median      | Code Size | Allocated |
|----------------------------------- |-----------:|----------:|----------:|------------:|----------:|----------:|
| RunBigMatrixWithSomeWords          |   4.903 ms | 15.125 ms | 10.004 ms |   1.7227 ms |   3,541 B |  53.68 KB |
| RunBigMatrixWithLotsOfWords        | 113.133 ms | 20.368 ms | 13.472 ms | 107.8106 ms |   3,074 B | 481.75 KB |
| RunBigMatrixWithLotsOfWordsAsync   |  47.631 ms | 40.186 ms | 26.580 ms |  39.3075 ms |   2,717 B | 483.39 KB |
| RunSmallMatrixWithSomeWords        |   3.347 ms | 12.636 ms |  8.358 ms |   0.6898 ms |   4,061 B |  14.47 KB |
| RunSmallMatrixWithLotsOfWords      |   9.226 ms | 17.425 ms | 11.526 ms |   5.1928 ms |   3,074 B | 442.54 KB |
| RunSmallMatrixWithLotsOfWordsAsync |   9.626 ms | 18.837 ms | 12.459 ms |   5.5796 ms |   2,717 B | 444.18 KB |

Benchmarks suggest that parallelization is only worth it as matrix becomes bigger.

## Further improvements
- Consider comparing size of rows & columns vs. the word stream and programmatically switch the iteration to find the words (bigger collection being parallelized).
- Special characters safety.
- Language determination.
- Re-use of parts of code.

## Tests
Unit tests built to ensure the code functionality.

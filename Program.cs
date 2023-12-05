class Program
{
    static async Task Main()
    {
        // Define matrices A and B
        int[,] matrixA = GenerateMatrix(3, 4);
        int[,] matrixB = GenerateMatrix(4, 2);

        // Sequential Matrix Multiplication
        var sequentialResult = MultiplyMatricesSequential(matrixA, matrixB);
        Console.WriteLine("Sequential Result:");
        PrintMatrix(sequentialResult);

        // Task Parallelism (TPL)
        var tplResult = MultiplyMatricesWithTPL(matrixA, matrixB);
        Console.WriteLine("\nTPL Result:");
        PrintMatrix(tplResult);

        // Parallel.ForEach
        var parallelForEachResult = MultiplyMatricesWithParallelForEach(matrixA, matrixB);
        Console.WriteLine("\nParallel.ForEach Result:");
        PrintMatrix(parallelForEachResult);

        // Asynchronous Programming (async/await)
        var asyncResult = await MultiplyMatricesAsync(matrixA, matrixB);
        Console.WriteLine("\nAsync/Await Result:");
        PrintMatrix(asyncResult);

        // Data Parallelism
        var dataParallelResult = MultiplyMatricesWithDataParallelism(matrixA, matrixB);
        Console.WriteLine("\nData Parallelism Result:");
        PrintMatrix(dataParallelResult);

        // Parallel.For and Parallel.ForEach are used depending if you are working with a numeric range of collection of elements
    }

    static int[,] GenerateMatrix(int rows, int cols)
    {
        var random = new Random();
        int[,] matrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = random.Next(1, 10);
            }
        }

        return matrix;
    }


    // Sequential Matrix Multiplication
    static int[,] MultiplyMatricesSequential(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < colsA; k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        }

        return result;
    }

    // Task Parallelism (TPL)
    static int[,] MultiplyMatricesWithTPL(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        Parallel.For(0, rowsA, i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < matrixA.GetLength(1); k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        });

        return result;
    }

    // Parallel.ForEach
    static int[,] MultiplyMatricesWithParallelForEach(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        Parallel.ForEach(Enumerable.Range(0, rowsA), i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < matrixA.GetLength(1); k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        });

        return result;
    }

    // Asynchronous Programming (async/await)
    static async Task<int[,]> MultiplyMatricesAsync(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        await Task.Run(() =>
        {
            Parallel.For(0, rowsA, i =>
            {
                for (int j = 0; j < colsB; j++)
                {
                    for (int k = 0; k < matrixA.GetLength(1); k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            });
        });

        return result;
    }

    // Data Parallelism
    static int[,] MultiplyMatricesWithDataParallelism(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        Parallel.For(0, rowsA, i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < matrixA.GetLength(1); k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        });

        return result;
    }

    static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}

static class ArrayExtensions
{
    public static T[,] Reshape<T>(this T[] array, int rows, int cols)
    {
        if (array.Length != rows * cols)
            throw new ArgumentException("Invalid reshape parameters.");

        T[,] result = new T[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = array[i * cols + j];

        return result;
    }
}

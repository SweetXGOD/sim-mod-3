using MathNet.Numerics.Distributions; // Для статистических функций

public class LCG
{
    private long seed;
    private const long a = 1664525;
    private const long c = 1013904223;
    private const long m = 4294967296;

    public LCG(long seed)
    {
        this.seed = seed;
    }

    public double Next()
    {
        seed = (a * seed + c) % m;
        return (double)seed / m;
    }
}

public class ChiSquaredTest
{
    public static double ChiSquared(double[] numbers, int intervals)
    {
        int[] observed = new int[intervals];
        double expected = numbers.Length / (double)intervals;

        // Разбиение данных по интервалам
        foreach (var num in numbers)
        {
            int index = (int)(num * intervals);
            if (index == intervals) index = intervals - 1; // Обработка граничного случая
            observed[index]++;
        }

        // Расчет статистики χ²
        double chiSquare = 0;
        for (int i = 0; i < intervals; i++)
        {
            double difference = observed[i] - expected;
            chiSquare += difference * difference / expected;
        }

        return chiSquare;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Параметры теста
        int sampleSize = 1000; // Размер выборки
        int intervals = 10;    // Количество интервалов

        // Генерация псевдослучайных чисел с помощью LCG
        LCG lcg = new LCG(12345);
        double[] lcgNumbers = new double[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            lcgNumbers[i] = lcg.Next();
        }

        // Проведение χ² теста для LCG
        double chiSquaredLCG = ChiSquaredTest.ChiSquared(lcgNumbers, intervals);
        Console.WriteLine($"Статистика χ² для LCG: {chiSquaredLCG}");

        // Генерация псевдослучайных чисел с помощью Random
        Random random = new Random();
        double[] randomNumbers = new double[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            randomNumbers[i] = random.NextDouble();
        }

        // Проведение χ² теста для Random
        double chiSquaredRandom = ChiSquaredTest.ChiSquared(randomNumbers, intervals);
        Console.WriteLine($"Статистика χ² для Random: {chiSquaredRandom}");

        // Получение критического значения χ² для уровня значимости 0.05
        double chiSquaredCritical = ChiSquared.InvCDF(intervals - 1, 0.95);
        Console.WriteLine($"Критическое значение χ² (0.05): {chiSquaredCritical}");

        // Результаты тестов
        if (chiSquaredLCG < chiSquaredCritical)
        {
            Console.WriteLine("Распределение чисел LCG проходит тест χ².");
        }
        else
        {
            Console.WriteLine("Распределение чисел LCG не проходит тест χ².");
        }

        if (chiSquaredRandom < chiSquaredCritical)
        {
            Console.WriteLine("Распределение чисел Random проходит тест χ².");
        }
        else
        {
            Console.WriteLine("Распределение чисел Random не проходит тест χ².");
        }
    }
}
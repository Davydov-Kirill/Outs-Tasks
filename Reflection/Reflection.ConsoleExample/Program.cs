using Newtonsoft.Json;
using Reflection.ConsoleExample;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        const int iterations = 10000;
        var fInstance = F.Get();
        Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine("Сериализуемый класс: class F { int i1, i2, i3, i4, i5;}");
        Console.WriteLine($"количество замеров: {iterations} итераций");
        Console.WriteLine("мой рефлекшен:");

        var collection = new List<F>();

        for (int i = 0; i < iterations; i++)
        {
            collection.Add(fInstance);
        }

        stopwatch.Start();
        var csv = CsvSerializer.Serialize(collection);
        stopwatch.Stop();
        Console.WriteLine($"Время на сериализацию = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        csv = CsvSerializer.Serialize(collection);
        Console.WriteLine(csv);
        stopwatch.Stop();
        Console.WriteLine($"Время на вывод результата сериализации = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        var deserializeCollection = CsvSerializer.Deserialize<F>(csv);
        stopwatch.Stop();
        Console.WriteLine($"Время на десериализацию = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        deserializeCollection = CsvSerializer.Deserialize<F>(csv);
        deserializeCollection.ToList().ForEach(item => Console.WriteLine(item));
        stopwatch.Stop();
        Console.WriteLine($"Время на вывод результата десериализации = {stopwatch.ElapsedMilliseconds} мс");



        Console.WriteLine("стандартный механизм(NewtonsoftJson):");

        stopwatch.Restart();
        var jsonString = JsonConvert.SerializeObject(collection);
        stopwatch.Stop();
        Console.WriteLine($"Время на сериализацию = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        jsonString = JsonConvert.SerializeObject(collection);
        Console.WriteLine(jsonString);
        stopwatch.Stop();
        Console.WriteLine($"Время на вывод результата сериализации = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        var jsonCollection = JsonConvert.DeserializeObject<List<F>>(jsonString);
        stopwatch.Stop();
        Console.WriteLine($"Время на десериализацию = {stopwatch.ElapsedMilliseconds} мс");

        stopwatch.Restart();
        jsonCollection = JsonConvert.DeserializeObject<List<F>>(jsonString);
        jsonCollection?.ForEach(item => Console.WriteLine(item));
        stopwatch.Stop();
        Console.WriteLine($"Время на вывод результата десериализации = {stopwatch.ElapsedMilliseconds} мс");

        Console.ReadKey();
    }
}
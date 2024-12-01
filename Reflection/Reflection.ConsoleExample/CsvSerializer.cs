using System.Reflection;
using System.Text;

namespace Reflection.ConsoleExample
{
    public class CsvSerializer
    {
        public static string Serialize<T>(IEnumerable<T> collection)
        {
            if (collection == null || !collection.Any())
            {
                return string.Empty;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var csvBuilder = new StringBuilder();

            csvBuilder.AppendLine(string.Join(',', properties.Select(f => EscapeCsvValue(f.Name))));

            foreach (T item in collection)
            {
                var values = properties.Select(f => EscapeCsvValue(f.GetValue(item)?.ToString()));
                csvBuilder.AppendLine(string.Join(',', values));
            }
            return csvBuilder.ToString();
        }

        private static string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "\"\"";

            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        public static IEnumerable<T> Deserialize<T>(string csv) where T : new()
        {
            if (string.IsNullOrWhiteSpace(csv))
                return Enumerable.Empty<T>();

            var lines = csv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var header = lines[0].Split(',').Select(h => UnescapeCsvValue(h)).ToArray();
            var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var collection = new List<T>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');

                var item = new T();

                for (int j = 0; j < values.Length; j++)
                {
                    var field = fields.FirstOrDefault(f => string.Equals(f.Name, header[j], StringComparison.OrdinalIgnoreCase));

                    if (field != null && j < values.Length)
                    {
                        var value = UnescapeCsvValue(values[j]);

                        if (field.FieldType == typeof(int))
                        {
                            if (int.TryParse(value, out var intValue))
                            {
                                field.SetValue(item, intValue);
                            }
                        }
                        else
                        {
                            field.SetValue(item, value);
                        }
                    }
                }
                collection.Add(item);
            }
            return collection;
        }

        private static string UnescapeCsvValue(string value)
        {
            if (value.StartsWith('"') && value.EndsWith('"'))
            {
                value = value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
            }
            return value;
        }
    }
}

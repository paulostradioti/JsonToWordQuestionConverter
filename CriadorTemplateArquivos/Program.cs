using System.Text.Json;

namespace CriadorTemplateArquivos
{
    internal class Program
    {
        private 
        static void Main(string[] args)
        {
            var model = new MultipleChoiceModel();
            var content = JsonSerializer.Serialize(model, new JsonSerializerOptions{WriteIndented = true});
            string root = @"C:\Users\paulo\source\repos\be-cs-backend-c-sharp\BE-CS-006 PROGRAMAÇÃO WEB I (ASP.NET MVC)\Exercícios";

            var inicio = 16;
            var fim = 40;

            for (int i = inicio; i <= fim; i++)
            {
                var path = Path.Combine(root, $"be_cs_006_{i}.json");
                if (File.Exists(path))
                    File.Delete(path);

                File.WriteAllText(path, content);
            }
        }
    }
}
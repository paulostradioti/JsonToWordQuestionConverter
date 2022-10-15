using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace ExtratorTexto
{
    public static class Extensions
    {
        private static readonly JsonDocument nullDocument = JsonDocument.Parse("{}");
        public static FileInfo[] GetJsonFiles(this DirectoryInfo directoryInfo) =>
            directoryInfo.GetFiles("*.json", SearchOption.TopDirectoryOnly);

        public static JsonDocument NullDocument(this JsonDocument _) => nullDocument;

        public static JsonDocument LodAsJsonDocument(this FileInfo file)
        {
            if (!file.Exists)
                return JsonDocument.Parse("{}");

            var fileContent = File.ReadAllText(file.FullName);
            var document = JsonDocument.Parse(fileContent, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            });

            return document;
        }
    }
}

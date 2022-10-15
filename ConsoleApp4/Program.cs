using ExtratorTexto.Models;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.Json;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static ExtratorTexto.Models.ChoicesQuestionModel;

namespace ExtratorTexto
{
    public partial class Program
    {
        static Program()
        {
            outputDocument = DocX.Create(OutPutFileName(), DocumentTypes.Pdf);
        }

        private static char alternativa = 'a';
        private static string OutPutFileName(string extension = "docx") => Path.Combine(GetTargetDirectoryParameter().FullName, $"output.{extension}");
        private static DocX outputDocument;
        private static IEnumerable<FileInfo> exerciseFiles;
        private static int questionCounter = 1;

        //private static StringBuilder _output = new();

        private static DirectoryInfo GetTargetDirectoryParameter()
        {
            var firstArgument = Environment
                                    .GetCommandLineArgs()
                                    .Skip(1)
                                    .FirstOrDefault()
                                ??
                                AppDomain.CurrentDomain.BaseDirectory;


            return new DirectoryInfo(firstArgument);
        }


        private static void GetAllFiles()
        {
            var directory = GetTargetDirectoryParameter();
            exerciseFiles = directory.GetJsonFiles();
        }

        private static void GetContent(BaseQuestionModel model)
        {
            if (model is ChoicesQuestionModel choice)
            {
                var paragraph = outputDocument.InsertParagraph();
                paragraph.Append($"QUESTÃO NÚMERO {questionCounter}").Heading(HeadingType.Heading1);

                paragraph = outputDocument.InsertParagraph();
                paragraph.Append("ENUNCIADO:").Heading(HeadingType.Heading2);

                paragraph = outputDocument.InsertParagraph();
                paragraph.Append(string.Join(Environment.NewLine, choice.Text));

                paragraph = outputDocument.InsertParagraph();
                paragraph.Append("ALTERNATIVAS:").Heading(HeadingType.Heading2);

                paragraph = outputDocument.InsertParagraph();

                

                for (int index = 0; index < choice.Alternatives.Length; index++)
                {
                    if (index > 0)
                        paragraph.AppendLine();

                    var alternative = choice.Alternatives[index];

                    paragraph.Append($"{(char)(alternativa+index)})", new Formatting() { Bold = true });
                    paragraph.Append($" {alternative.Text} ");
                    AppendCorrect(alternative.Correct);
                }
                questionCounter++;

                void AppendCorrect(bool alternativeCorrect)
                {
                    var texo = alternativeCorrect ? "correta" : "incorreta";
                    var color = alternativeCorrect ? Color.Blue : Color.Red;

                    paragraph.Append($"({texo})", new Formatting() { FontColor = color, Bold = true });
                }

            }
            else if (model is GapsQuestionModel gaps)
            {
                var paragraph = outputDocument.InsertParagraph();
                paragraph.Append($"QUESTÃO NÚMERO {questionCounter}").Heading(HeadingType.Heading1);

                paragraph = outputDocument.InsertParagraph();
                paragraph.Append("ENUNCIADO:").Heading(HeadingType.Heading2);

                paragraph = outputDocument.InsertParagraph();
                var text = string.Join(Environment.NewLine, gaps.Text).Replace("{{GAP}}", "________________");
                paragraph.Append(text);

                paragraph = outputDocument.InsertParagraph();
                paragraph.Append("RESPOSTAS:").Heading(HeadingType.Heading2);

                paragraph = outputDocument.InsertParagraph();

                for (int index = 0; index < gaps.Gaps.Length; index++)
                {
                    if (index > 0)
                        paragraph.AppendLine();

                    var gap = gaps.Gaps[index];

                    paragraph.Append(gap.Expected);
                }

                questionCounter++;
            }
        }



        private static BaseQuestionModel? GetModelLoader(string? getString, JsonDocument document)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };

            return getString?.ToUpper() switch
            {
                "VF" => document.Deserialize<ChoicesQuestionModel>(options),
                "MULT" => document.Deserialize<ChoicesQuestionModel>(options),
                "GAP" => document.Deserialize<GapsQuestionModel>(options),
                _ => null
            };
        }

        private static void ExtractContent()
        {
            foreach (var exerciseFile in exerciseFiles)
            {
                var document = exerciseFile.LodAsJsonDocument();
                var valid = document.RootElement.TryGetProperty("type", out var element);

                if (!valid)
                    continue;

                var model = GetModelLoader(element.GetString(), document);
                GetContent(model);
            }

            outputDocument.Save();
            Process.Start(@"C:\Program Files\Microsoft Office\root\Office16\WINWORD.EXE", $"\"{OutPutFileName()}\"");
        }
    }
}

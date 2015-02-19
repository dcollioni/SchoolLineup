using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace SchoolLineup.Util
{
    public class HtmlTemplateHelper
    {
        public static string FillTemplate(string templateFileName, IDictionary<string, string> parameters)
        {
            var templateHtml = ReadHtmlTemplate(templateFileName);

            foreach (var parameter in parameters)
            {
                var key = string.Concat("{", parameter.Key, "}");
                templateHtml = templateHtml.Replace(key, parameter.Value);
            }

            return templateHtml;
        }

        private static string ReadHtmlTemplate(string templateFileName)
        {
            var templateDirectory = ConfigurationManager.AppSettings["TemplateDirectory"];
            var templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateDirectory, templateFileName);

            var templateHtml = string.Empty;

            using (StreamReader reader = new StreamReader(templateFilePath))
            {
                templateHtml = reader.ReadToEnd();
            }

            return templateHtml;
        }
    }
}
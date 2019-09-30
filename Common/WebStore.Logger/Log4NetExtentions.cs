using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace WebStore.Logger
{
    public static class Log4NetExtentions
    {
        public static ILoggerFactory AddLog4Net(
            this ILoggerFactory factoty,
            string configurationFile = "log4net.config")
        {
            // Если директория относительная, то прибавляем к пути путь до директории.
            if (!Path.IsPathRooted(configurationFile))
            {
                var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Не найдена сборка испольнительного файла.");
                var dir = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException("Не определена директория испольнительного файла");
                configurationFile = Path.Combine(dir, configurationFile);
            }

            factoty.AddProvider(new Log4NetLoggerProvider(configurationFile));
            return factoty;
        }
    }
}

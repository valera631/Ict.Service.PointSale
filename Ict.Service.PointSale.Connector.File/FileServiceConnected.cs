using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.Organization.Connector.File.Interface;
using Ict.Service.PointSale.Connector.File.Action;
using Ict.Service.PointSale.Connector.File.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Ict.Service.PointSale.Connector.File
{
    /// <summary>
    /// Класс расширений для сервисов
    /// </summary>
    public static class FileServiceConnected
    {
        /// <summary>
        /// Подключение сервисов для работы с файлами
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileService(this IServiceCollection services)
        {
            services.AddScoped<IFileConnected, FileConnected>();
            services.AddScoped<IFolderConnected, FolderConnected>();

            services.AddScoped<IConnectionToService, ConnectionToService>();
            return services;
        }
    }
}

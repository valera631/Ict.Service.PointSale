using Ict.ApiProvider;
using Ict.Service.File.Models;
using Ict.Service.File.Models.Folder;
using Ict.Service.PointSale.Connector.File.Interface;
using Microsoft.Extensions.Configuration;

namespace Ict.Service.PointSale.Connector.File.Action;

/// <summary>
/// Класс реализующий интерфейс для работы с файлами
/// </summary>
/// <param name="apiService"></param>
/// <param name="config"></param>
public class FolderConnected(IConnectionToService apiService, IConfiguration config) : IFolderConnected
{
    private readonly string _path = $"{config["Service.File:Url"]}/api/v1/{ControllerName}";
    private const string ControllerName = "Folder";

    /// <summary>
    /// Удалиение файла
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<List<FolderDto>>> GetUserFolders(SelectFolderDto item)
        => await apiService.GetData<List<FolderDto>, SelectFolderDto>(item, AppData.ApiType.GET, (_path, "GetUserFolders"));

    /// <summary>
    /// Метод для создания новой виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<FolderDto>> CreateFolder(CreateFolderDto item)
        => await apiService.GetData<FolderDto, CreateFolderDto>(item, AppData.ApiType.POST, (_path, "CreateFolder"));

    /// <summary>
    /// Метод для удаления виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<bool>> DeleteFolder(DeleteFolderDto item)
       => await apiService.GetData<bool, DeleteFolderDto>(item, AppData.ApiType.DELETE, (_path, "DeleteFolder"));

    /// <summary>
    /// Метод для переименования виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<FolderDto>> RenameFolder(RenameFolderDto item)
       => await apiService.GetData<FolderDto, RenameFolderDto>(item, AppData.ApiType.PUT, (_path, "RenameFolder"));

}
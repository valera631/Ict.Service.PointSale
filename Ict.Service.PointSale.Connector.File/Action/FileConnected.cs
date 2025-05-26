using Ict.ApiProvider;
using Ict.Service.File.Models;
using Ict.Service.File.Models.File;
using Ict.Service.Organization.Connector.File.Interface;
using Ict.Service.PointSale.Connector.File.Interface;
using Microsoft.Extensions.Configuration;

namespace Ict.Service.PointSale.Connector.File.Action;


/// <summary>
/// Класс реализующий интерфейс для работы с файлами
/// </summary>
/// <param name="apiService"></param>
/// <param name="config"></param>
public class FileConnected(IConnectionToService apiService, IConfiguration config) : IFileConnected
{
    private readonly string _path = $"{config["Service.File:Url"]}/api/v1/{ControllerName}";
    private const string ControllerName = "File";

    /// <summary>
    /// Удалиение файла
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<List<FileDto>>> Create(FileContainerDto item)
        => await apiService.GetData<List<FileDto>, FileContainerDto>(item, AppData.ApiType.POST, (_path, "Create"));


    public async Task<Response<List<FileDto>>> Create2(FileContainer item)
    => await apiService.GetData<List<FileDto>, FileContainer>(item, AppData.ApiType.POST, (_path, "Create2"));

    /// <summary>
    /// Переименовать файл
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<FileDto>> Rename(RenameFileDto item)
       => await apiService.GetData<FileDto, RenameFileDto>(item, AppData.ApiType.PUT, (_path, "Rename"));

    /// <summary>
    /// Получить все файлы по группе
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Response<List<FileDto>>> GetHistory(Guid item)
      => await apiService.GetData<List<FileDto>, Guid>(item, AppData.ApiType.GET, (_path, "GetHistory"));

    /// <summary>
    /// Получение файла по идентификатору
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<FileDto>> Select(RequestFileDto item)
        => await apiService.GetData<FileDto, RequestFileDto>(item, AppData.ApiType.GET, (_path, "Select"));


    public async Task<Response<FileDto>> SelectByParent(RequestImage item)
       => await apiService.GetData<FileDto, RequestImage>(item, AppData.ApiType.GET, (_path, "SelectByParent"));

    public async Task<Response<FileDto>> SelectForPatch(RequestFileForPatchDto item)
       => await apiService.GetData<FileDto, RequestFileForPatchDto>(item, AppData.ApiType.GET, (_path, "SelectForPatch"));

    /// <summary>
    /// Получение файла по идентификатору
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<FileDto>> SelectByName(SelectFileByNameDto item)
        => await apiService.GetData<FileDto, SelectFileByNameDto>(item, AppData.ApiType.GET, (_path, "SelectByName"));


    /// <summary>
    /// Удалиение файла
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<ResponseFileDelete>> Delete(RequestFileDelete item)
        => await apiService.GetData<ResponseFileDelete, RequestFileDelete>(item, AppData.ApiType.DELETE, (_path, "Delete"));

    /// <summary>
    /// Удалить файлы
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task<Response<bool>> DeleteByParameters(RequestImageRemoveDto item)
        => await apiService.GetData<bool, RequestImageRemoveDto>(item, AppData.ApiType.DELETE, (_path, "DeleteByParameters"));

    /// <summary>
    /// Удалить файлы
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    public async Task<Response<bool>> DeleteAllByParent(Guid item)
        => await apiService.GetData<bool, Guid>(item, AppData.ApiType.DELETE, (_path, "DeleteAllByParent"));

}
using Ict.Service.File.Models;
using Ict.Service.File.Models.File;

namespace Ict.Service.Organization.Connector.File.Interface;

/// <summary>
/// Интерфейс для работы с файлами
/// </summary>
public interface IFileConnected
{
    /// <summary>
    /// Получить файл по модели запроса файла
    /// </summary>
    /// <param name="item">Id файла</param>
    /// <returns></returns>
    Task<Response<FileDto>> Select(RequestFileDto item);
    Task<Response<FileDto>> SelectForPatch(RequestFileForPatchDto item);


    Task<Response<FileDto>> SelectByParent(RequestImage item);

    /// <summary>
    /// Получить файл по параметрам:
    /// по родителю и наименованию
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<FileDto>> SelectByName(SelectFileByNameDto item);

    /// <summary>
    ///Добавить новый файл в базу
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<List<FileDto>>> Create(FileContainerDto item);

    Task<Response<List<FileDto>>> Create2(FileContainer item);

    /// <summary>
    /// Получить все файлы по группе
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Response<List<FileDto>>> GetHistory(Guid item);

    /// <summary>
    /// Переименовать файл
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<FileDto>> Rename(RenameFileDto item);

    /// <summary>
    /// Удалиение файла
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<ResponseFileDelete>> Delete(RequestFileDelete item);

    /// <summary>
    /// Удалить файлы
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns></returns>
    Task<Response<bool>> DeleteAllByParent(Guid item);

    /// <summary>
    /// Удалить файлы
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<bool>> DeleteByParameters(RequestImageRemoveDto item);

}
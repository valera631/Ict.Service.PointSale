using Ict.Service.File.Models;
using Ict.Service.File.Models.Folder;

namespace Ict.Service.PointSale.Connector.File.Interface;

public interface IFolderConnected
{
    /// <summary>
    /// Получить файл по Id файла
    /// </summary>
    /// <param name="item">Id файла</param>
    /// <returns></returns>
    Task<Response<List<FolderDto>>> GetUserFolders(SelectFolderDto item);

    /// <summary>
    /// Метод для создания новой виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<FolderDto>> CreateFolder(CreateFolderDto item);

    /// <summary>
    /// Метод для переименования виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<FolderDto>> RenameFolder(RenameFolderDto item);

    /// <summary>
    /// Метод для удаления виртуальной папки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task<Response<bool>> DeleteFolder(DeleteFolderDto item);


}

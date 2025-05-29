using System.Collections.Generic;
using Ict.ApiResults;
using Ict.Service.PointSale.API.Abstractions.Models.Photo;
using Ict.Service.PointSale.Core.Abstractions.Interfaces;
using Ict.Service.PointSale.Models.Photo;
using Microsoft.AspNetCore.Mvc;

namespace Ict.Service.PointSale.RestAPI.Controllers.v1
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/pointsale/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }


        [HttpPost("UploadLogo")]
        public async Task<ApiResult<bool>> AddLogo(PhotoContainerRequest logoUploadRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var logoUploadDto = new PhotoContainerDto
                {
                    PointSaleId = logoUploadRequest.PointSaleId,
                    Photos = logoUploadRequest.Photos.Select(p => new PhotoItemDto
                    {
                        ImageId = p.ImageId,
                        Images = p.Images,
                        FileName = p.FileName,
                        ContentType = p.ContentType,
                        ParentId = p.ParentId,
                    }).ToList()
                };

                var result = await _photoService.AddLogoAsync(logoUploadDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }

        [HttpPost("AddPhoto")]
        public async Task<ApiResult<bool>> AddPhoto(PhotoContainerRequest photoUploadRequest)
        {
            var operation = ApiResult.CreateResult<bool>();
            try
            {
                var photoUploadDto = new PhotoContainerDto
                {
                    PointSaleId = photoUploadRequest.PointSaleId,
                    Photos = photoUploadRequest.Photos.Select(p => new PhotoItemDto
                    {
                        ImageId = p.ImageId,
                        Images = p.Images,
                        FileName = p.FileName,
                        ContentType = p.ContentType,
                        ParentId = p.ParentId,
                    }).ToList()
                };

                var result = await _photoService.AddPhotoAsync(photoUploadDto);
                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }



        [HttpGet("GetLogo")]
        public async Task<ApiResult<PointSalePhotoResult>> GetLogo(IdentifierRequest identifierRequest)
        {
            var operation = ApiResult.CreateResult<PointSalePhotoResult>();
            try
            {
                var result = await _photoService.GetLogoAsync(identifierRequest.PointSaleId);

                if (result == null || result.Data == null)
                {
                    operation.ErrorMessage = "Не удалось получить данные о фотографиях.";
                    return operation;
                }
                var logo = result.Data;
                if (logo == null)
                {
                    operation.ErrorMessage = "Логотип не найден.";
                    return operation;
                }

                operation.Result = new PointSalePhotoResult
                {
                    PhotoId = logo.PhotoId,
                    PhotoData = Convert.ToBase64String(logo.PhotoData),
                    PhotoName = logo.PhotoName,
                    IsMain = logo.IsMain,
                    PhotoContentType = logo.PhotoContentType
                };

            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;

        }


        [HttpGet("Previews")]
        public async Task<ApiResult<List<PointSalePhotoResult>>> GetPreviews(IdentifierRequest identifierRequest)
        {
            var operation = ApiResult.CreateResult<List<PointSalePhotoResult>>();

            try
            {

                var result = await _photoService.GetPreviewsAsync(identifierRequest.PointSaleId);

                if (result == null || result.Data == null)
                {
                    operation.ErrorMessage = "Не удалось получить данные о фотографиях.";
                    return operation;
                }

                var photos = result.Data.Select(photo => new PointSalePhotoResult
                {
                    PhotoId = photo.PhotoId,
                    PhotoData = Convert.ToBase64String(photo.PhotoData),
                    PhotoName = photo.PhotoName,
                    IsMain = photo.IsMain,
                    PhotoContentType = photo.PhotoContentType
                }).ToList();
                operation.Result = photos;

            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }


        [HttpDelete("DeletePhoto")]
        public async Task<ApiResult<List<Guid>>> DeletePhoto(PhotosDeleteRequest photosDeleteRequest)
        {
            var operation = ApiResult.CreateResult<List<Guid>>();
            try
            {

                var deletePhotoDto = new PhotosDeleteDto
                {
                    PhotoIds = photosDeleteRequest.ImageIds,
                    PointSaleId = photosDeleteRequest.PointSaleId
                };

                var result = await _photoService.DeletePhotoAsync(deletePhotoDto);

                operation.Result = result.Data;
                operation.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                operation.ErrorMessage = ex.Message;
            }
            return operation;
        }
    }
}
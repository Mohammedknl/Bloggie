namespace Bloggie.Web.Repositories
{
    public interface IImageRespository
    {
        //Has one property or defination to upload image with parameter as IFormFile
        Task<string> UploadAsync(IFormFile file);
    }
}

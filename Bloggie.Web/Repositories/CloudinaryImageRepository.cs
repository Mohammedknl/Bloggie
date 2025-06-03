
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repositories
{
    //This is the implementation of IImage Repository and using Cloudinary SDK to upload the image file
    public class CloudinaryImageRepository : IImageRespository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;
        

        //Creating Constructor for CloudImageRepository
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            //Now configuring/initializing the cloudinary account settings with the 3 properties like Name,ApiKey and ApiSecret
            account = new Account(
            configuration.GetSection("Cloudinary")["CloudName"],
            configuration.GetSection("Cloudinary")["ApiKey"],
            configuration.GetSection("Cloudinary")["ApiSecret"]);
        }
        //Before we need to register inside Cloudinary and get the settings fromcloudinary platform(3rd party to Host images)

        public async Task<string> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                DisplayName = file.FileName
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }



    }
}

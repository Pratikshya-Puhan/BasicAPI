namespace BasicAPI.Services
{
    public interface IFileUploadService
    {
        public Task<string> SingleUpload(IFormFile file, HttpRequest request);
    }
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SingleUpload(IFormFile file, HttpRequest request)
        {
            try
            {
                string _uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                var ext = Path.GetExtension(file.FileName);

                var _fileId = GenerateNewFileName();

                var _newFilePath = Path.Combine(_uploadPath, string.Concat(_fileId, ext));

                using (var fileStream = new FileStream(_newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);

                    fileStream.Close();
                }

                return string.Concat(GenerateRequestUrl(request), "/", "uploads", "/", _fileId, ext);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateRequestUrl(HttpRequest request)
        {
            return string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.PathBase.ToUriComponent());
        }

        private string GenerateNewFileName()
        {
            return new Random().Next(int.MaxValue).ToString("x");
        }
    }
}

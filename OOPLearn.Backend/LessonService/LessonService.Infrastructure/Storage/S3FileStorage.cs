using Amazon.S3;
using Amazon.S3.Model;
using LessonService.Application.Interfaces;

namespace LessonService.Infrastructure.Storage
{
    public class S3FileStorage : IFileStorage
    {
        private readonly IAmazonS3 _internalClient;
        private readonly IAmazonS3 _publicClient;
        private readonly S3Options _options;

        public S3FileStorage(S3Clients clients, S3Options options)
        {
            _internalClient = clients.Internal;
            _publicClient = clients.Public;
            _options = options;
        }

        public async Task UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _internalClient.PutObjectAsync(request, cancellationToken);
        }

        public async Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var response = await _internalClient.GetObjectAsync(_options.BucketName, fileName, cancellationToken);

            return response.ResponseStream;
        }

        public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
        {
            await _internalClient.DeleteObjectAsync(_options.BucketName, fileName, cancellationToken);
        }

        public async Task<string> GetPresignedUrlAsync(string fileName, int expirationMinutes = 15)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _options.BucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Verb = HttpVerb.GET
            };

            var url = await _publicClient.GetPreSignedURLAsync(request);

            return url.Replace("https://", "http://");
        }
    }
}

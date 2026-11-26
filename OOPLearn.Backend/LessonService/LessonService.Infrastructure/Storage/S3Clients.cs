using Amazon.S3;

namespace LessonService.Infrastructure.Storage
{
    public class S3Clients
    {
        public IAmazonS3 Internal { get; }

        public IAmazonS3 Public { get; }

        public S3Clients(IAmazonS3 internalClient, IAmazonS3 publicClient)
        {
            Internal = internalClient;
            Public = publicClient;
        }

        public void Dispose()
        {
            Internal.Dispose();
            Public.Dispose();
        }
    }
}

namespace LessonService.Application.Interfaces
{
    public interface IFileStorage
    {
        Task UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

        Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken = default);

        Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);

        Task<string> GetPresignedUrlAsync(string fileName, int expirationMinutes = 15);
    }
}

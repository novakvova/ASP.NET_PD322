namespace ApiStore.Interfaces
{
    public interface IImageHulk
    {
        Task<string> Save(IFormFile image);
    }
}

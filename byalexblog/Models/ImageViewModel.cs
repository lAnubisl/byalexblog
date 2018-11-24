namespace byalexblog.Models
{
    public class ImageModelItem
    {
        internal ImageModelItem(string path)
        {
            Path = path.Replace("\\", "/");
        }

        public string Path { get; }
    }
}
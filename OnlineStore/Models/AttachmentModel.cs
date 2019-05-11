namespace OnlineStore.Models
{
    public class AttachmentModel
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Contents { get; set; }
    }
}
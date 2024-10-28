namespace MemoriesBackend.API.DTO.ShareFile.Response
{
    public class ShareFileAddResponse
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public Guid? SharedForUserId { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

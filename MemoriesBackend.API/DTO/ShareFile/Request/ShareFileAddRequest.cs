namespace MemoriesBackend.API.DTO.ShareFile.Request
{
    public class ShareFileAddRequest
    {
        public Guid FileId { get; set; }
        public Guid? SharedForUserId { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

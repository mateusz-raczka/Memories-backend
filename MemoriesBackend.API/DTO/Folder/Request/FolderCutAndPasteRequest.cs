﻿namespace MemoriesBackend.API.DTO.Folder.Request
{
    public class FolderCutAndPasteRequest
    {
        public Guid SourceFolderId { get; set; }
        public Guid TargetFolderId { get; set; }
    }
}
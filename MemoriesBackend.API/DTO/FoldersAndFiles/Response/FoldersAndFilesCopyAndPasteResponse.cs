﻿using MemoriesBackend.API.DTO.File.Response;
using MemoriesBackend.API.DTO.Folder.Response;

namespace MemoriesBackend.API.DTO.FolderAndFile.Response
{
    public class FoldersAndFilesCopyAndPasteResponse
    {
        public IEnumerable<FolderCopyAndPasteResponse> Folders { get; set; }
        public IEnumerable<FileCopyPasteResponse> Files { get; set; }
    }
}

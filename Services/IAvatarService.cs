// filepath: /home/notionme/Document/Repos/SortProgram/Services/IAvatarService.cs
using Avalonia.Media.Imaging;
using System.Collections.Generic;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public interface IAvatarService
    {
        string GetRandomAvatarPath();
        string GetDefaultAvatarPath();
        bool IsValidAvatarPath(string path);
        Bitmap? LoadAvatar(string path);
        List<string> GetAllAvatarPaths();
    }
}
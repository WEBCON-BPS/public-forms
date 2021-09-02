using System.Linq;
using System.Text.Json;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Helper
{
    static class FrameAncestorsHelper
    {
        public static string[] GetArrayFromString(string frameAncestors)
        {
            if (string.IsNullOrEmpty(frameAncestors)) return null;
            return JsonSerializer.Deserialize<string[]>(frameAncestors);
        }
        public static string GetStringFromArray(string[] frameAncestors)
        {
            return frameAncestors?.Length > 0 ? JsonSerializer.Serialize(frameAncestors.Where(x => !string.IsNullOrEmpty(x))) : null;
        }
    }
}

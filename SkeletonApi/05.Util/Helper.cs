using System.Text;

namespace SkeletonApi.Util
{
    public class Helper
    {
        public static string GenerateKey()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 128; i++)
                sb.Append("a");

            return sb.ToString();
        }
    }
}

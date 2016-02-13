namespace Services.Common.Extensions
{
    using System;
    using Constants;

    public static class IntegerExtensions
    {
        public static string ToUrlPath(this int id)
        {
            return string.Format("{0}/{1}", id % Constants.SavedFilesSubfoldersCount, string.Format("{0}{1}", id.ToMd5Hash().Substring(0, Constants.FileHashLength), id));
        }
    }
}

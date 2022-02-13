using BookStore.Debugging;

namespace BookStore
{
    public class BookStoreConsts
    {
        public const string LocalizationSourceName = "BookStore";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "35c25905e28547a2969dead564d52f7b";
    }
}

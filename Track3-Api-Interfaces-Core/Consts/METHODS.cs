namespace Track3_Api_Interfaces_Core.Consts
{
    public class HTTPMETHODS
    {

        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string OPTIONS = "OPTIONS";
        public const string CONNECT = "CONNECT";
        public const string TRACE = "TRACE";
        public const string PATCH = "PATCH";
        public const string HEAD = "HEAD";

        public static readonly string[] METHODS = {
            GET,
            POST,
            PUT,
            DELETE,
            OPTIONS,
            CONNECT,
            TRACE,
            PATCH,
            HEAD
        };

    }
}

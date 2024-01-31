using System.Runtime.Serialization;

namespace Track3_Api_Interfaces_Core.Application.DATA
{
    public class BaseResult<T>
    {
        [DataMember]
        public ErrorApplication oError { get; set; }
        [DataMember(Name = "Data")]
        public T Data { get; set; }
        public string status { get; set; }
        public BaseResult()
        {
            this.oError = new ErrorApplication();
        }
    }
}

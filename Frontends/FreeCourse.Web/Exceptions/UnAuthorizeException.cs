using System;
using System.Runtime.Serialization;

namespace FreeCourse.Web.Exceptions// 125
{
    public class UnAuthorizeException : Exception
    {

        public UnAuthorizeException() : base()
        {
            
        }
        public UnAuthorizeException(string message) : base(message)
        {
        }

        public UnAuthorizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

      
    }
}
//bu sınıf hata fırlttıgında yakalacak ve bıze dondurecek bunu ResourceOwnerPasswordTokenHandler sınıfında kulanıyoruz
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos//20 dto eklıyoruz donusler ıcın
{
    public class ResponseDto<T>
    {
        public T Data { get;  set; }


        [JsonIgnore]//code dunuslerınde kodlar oldugu ıcın tekrar gondermeye gerek yok ama yazılım ıcınde kullanacagımız ıcın tanımdalık
        public int StatusCode { get;  set; }//137 setler private onları kaldırdık

        [JsonIgnore]
        public bool IsSuccessful { get;  set; }

        public List<string> Errors { get; set; }

        //statıc factory method

        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T>
            {
                Data = data,
                StatusCode = statusCode,
                IsSuccessful = true
                
            };
        }
        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T>
            {
                Data = default(T),
                StatusCode = statusCode,
                IsSuccessful = true
            };
        }
        public static ResponseDto<T> Fail(List<string> errors, int statusCode)
        {
            return new ResponseDto<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };

        }

        public static ResponseDto<T>Fail(string errors,int statusCode)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string>() 
                                 { 
                                 errors 
                                 },
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
//bu classta hata ve basarılı olma durumdan gerı dondurulecek sınıf tanımlandı statıc olarak eklendı 
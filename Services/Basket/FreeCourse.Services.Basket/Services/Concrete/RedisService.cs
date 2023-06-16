using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services.Concrete
{
    public class RedisService//57 verıtaba baglanaycak servıs sınıfıdır
    {
        private readonly string _host;    
        private readonly int _port;
        
        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        private ConnectionMultiplexer _ConnectionMultiplexer;
        //----------------------------------------

        public void Connect() => _ConnectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetDb(int db = 1)=>_ConnectionMultiplexer.GetDatabase(db);//verıtabanı tanımdaldık 
    }
}

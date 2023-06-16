using FreeCourse.Services.Order.Domain.Core.Abstract;
using FreeCourse.Services.Order.Domain.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate//87
{ //EF Core features   bu ef core dosayın edıldı
    // -- Owned Types bu adrres tablosonu ayrı tabloolarak kurar işrelıyoruz [owned]
    // -- Shadow Property //public int OrderId { get; set; }//tanımlamıyorz ef core bunu kendısı anlıyor bu tıplere shadow denır
    // -- Backing Field //private readonly List<OrderItem> _orderItems; busekılde tanımlanıyor acıklama aşagıdadrı
    public class Order:Entity,IAggregateRoot
    {
        public DateTime CreatedDate { get;private set; }


        public Addres Addres { get;private set; }//burdaef core bırsey demesek addres ıcerıgını order ıcınde sutun olarak eklere ama dersek yenı tabloda ekler ve order tablosunu ıkılnrır bu tıplere OWNED ENTİTY TYPE DENIR. dersek derken adrees sınıfında en uste [owned] etrutubesını eklıyoruzveya dbcontex ekleyebılırız
        public  string  BuyerId { get; private set; }//satın alan kısı  userId gönderecz

        //Backing Field
        private readonly List<OrderItem> _orderItems; // egerkı bunun gıbı get ve set yoksa bu fıeldır varsa properties yanı ozellıktı.. efcore okuma veyazmayı field uzerınden yapıyorsak buna backing fields denir.. orderıtems dısardan kımse ekleme yapamasın diye private yapacaksa sadece benım mettot uzerınden yapsın ama okuabılır ona metoto yapıyoruz 
        //order ulasıp ordan orderItems ekleme olmasın dıye sadece benım belırledıgım metot uzerınden ekleme yapabılsınler.  metot public void AddOrderItem
        //Encapsulation yaptık yanı bir sınıfın nesnenın belirli özellik ve metotlarının erişiminin kısıtlanması ve saklanması uyguladık
        public IReadOnlyCollection<OrderItem> OrderItems=>_orderItems;//ekleme sadece benım belırledıgımmetot ama okuma herse yapabılır

        public Order()
        {
            
        }

        public Order(string buyerId,Addres addres )
        {
            Addres = addres;
            BuyerId = buyerId;
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
        }

        public void AddOrderItem(string productId,string productName,decimal price,string pictureUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice=>_orderItems.Sum(x => x.Price);//toplam fiyat
    }
}
// bu desing patern de  order uzerınden orderItemslar eklenıyor bu yönteme AggregateRoot, order da orderItemslar ıcın dizi olsuturup oraya eklenıyor ve bu dısarıya sadece okuma IReadOnlyCollection fosnsıyonu uzernden yetkı verılıyor.ekleme sadece bu sınıf uzerındne yapılabılır addOrderItems  fonsıyonu ıle...
// egerkı bır sınıf IAggregateRoot ıse kullandıgı alt sınfıları baska IAggregateRoot olsmu sınıf bu alt sınıfları kullanamaz..yanı order IAggregateRoot sınıftır OrderItems sınıfını alt sınıf olarak kullandı baska IAggregateRoot olsmu sınıf OrderItems sınıfını alt sınıf oolarak kulanamaz

// kurdugumuz bu yontemle order uzerınden hem orderItem a hemde adres s ulasabılrıız dırek onlara ulasım yok
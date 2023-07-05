using FreeCourse.Web.Models.FakePaymnet;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Abstract//171
{
    public interface IFakePaymentService
    {
        Task<bool> ReceivePayment(FakePaymentInfoInput input);//sekron
        Task<bool> ReceiveAsenkronPayment(FakePaymenAsenkrontInfoInput input);//asenkron 185
    }
}

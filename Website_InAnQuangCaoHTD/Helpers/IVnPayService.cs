using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Helpers
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}

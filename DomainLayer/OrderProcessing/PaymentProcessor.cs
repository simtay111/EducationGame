using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using WePaySDK;

namespace DomainLayer.OrderProcessing
{
    public class PaymentProcessor
    {
        private readonly IPaymentAuditRepository _paymentAuditRepository;

        public PaymentProcessor(IPaymentAuditRepository paymentAuditRepository)
        {
            _paymentAuditRepository = paymentAuditRepository;
        }

        public CheckoutCreateResponse MakePayment(decimal cost, long creditCardToken, PaymentAudit audit)
        {
            _paymentAuditRepository.SaveNew(audit);
            var request = new CheckoutCreateRequest();
            request.accessToken = WePayConfig.accessToken;
            request.account_id = WePayConfig.accountId;
            request.amount = cost;
            request.type = "GOODS";
            request.payment_method_id = creditCardToken;
            request.payment_method_type = "credit_card";
            request.short_description = "Practice Owl Prizes";

            var response = new Checkout().Post(request);

            if (response.checkout_id == 0)
            {
                audit.Message = response.Error.error_message;
                audit.Status = PaymentStatus.Error;
            }
            else
            {
                audit.Status = PaymentStatus.Processed;
            }
            _paymentAuditRepository.Update(audit);

            return response;
        }

        public void DeleteCcCard(long creditCardId)
        {
            var request = new CreditCardDeleteRequest
                {
                    client_id = WePayConfig.clientId,
                    client_secret = WePayConfig.clientSecret,
                    credit_card_id = creditCardId
                };

            new CreditCard().Delete(request);
        }
    }
}
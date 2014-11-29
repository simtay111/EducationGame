using System;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using TangoApi;
using TangoApi.Entity;

namespace DomainLayer.ECards
{
    public class ECardOrderer
    {
        private readonly TangoAcctInfoProvider _infoProvider;
        private readonly OrderPlacer _placer;
        private readonly IAwardedPrizeRepository _awardedPrizeRepository;
        private readonly ITangoAuditRepository _tangoAuditRepository;

        public ECardOrderer(TangoAcctInfoProvider infoProvider, OrderPlacer placer, IAwardedPrizeRepository awardedPrizeRepository, ITangoAuditRepository tangoAuditRepository)
        {
            _infoProvider = infoProvider;
            _placer = placer;
            _awardedPrizeRepository = awardedPrizeRepository;
            _tangoAuditRepository = tangoAuditRepository;
        }

        public void PlaceOrder(ECardOrderRequest request, bool isForAccount = false)
        {
            var order = new OrderRequest
                {
                    account_identifier = _infoProvider.GetAcctInfo(),
                    customer = _infoProvider.GetCustomer(),
                    sku = request.Sku,
                    recipient = new Recipient
                        {
                            name = request.RecipientName,
                            email = request.RecipientEmail
                        },
                    reward_from = "THESITE",
                    reward_message = "All of us at " + "THESITE" + " are commited to helping you be the healthiest you possible. " +
                                     "We're so glad you're having fun getting there. Enjoy your reward!",
                    reward_subject = "Reward From " + "THESITE",
                    send_reward = true,
                    campaign = string.Empty
                };

            var tangoAudit = new TangoAudit
                {
                    CallType = TangoCallType.Order,
                    Recipient = request.RecipientEmail,
                    SKU = order.sku,
                    AccountInformation = request.AccountInformation,
                    AccountIdentifier = order.account_identifier,
                    Customer = order.customer,
                    RecipientName = order.recipient.name

                };
            _tangoAuditRepository.SaveNew(tangoAudit);


            OrderResponse response;
            try
            {
                if (request.IsRangePrize)
                    response = _placer.Order(order.WithAmount(request.Amount));
                else
                {
                    response = _placer.Order(order);
                }
                tangoAudit.Status = PaymentStatus.Processed;
                _tangoAuditRepository.Update(tangoAudit);
            }
            catch (TangoException ex)
            {
                tangoAudit.ErrorMessage = ex.ErrorMessage + " " + ex.Message;
                tangoAudit.Status = PaymentStatus.Error;
                _tangoAuditRepository.Update(tangoAudit);
                throw ex;
            }


            if (response.success && !isForAccount)
            {
                var awardedPrize = _awardedPrizeRepository.GetById(request.AwardedPrizeId);
                awardedPrize.DateOrdered = DateTime.Now;
                awardedPrize.Ordered = true;
                awardedPrize.Redeemed = true;
                _awardedPrizeRepository.Save(awardedPrize);
            }
        }
    }

    public class ECardOrderRequest
    {
        public long Amount { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string Sku { get; set; }
        public AccountInformation AccountInformation { get; set; }
        public int AwardedPrizeId { get; set; }
        public bool IsRangePrize { get; set; }
    }
}
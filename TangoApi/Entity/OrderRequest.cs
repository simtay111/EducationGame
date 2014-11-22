namespace TangoApi.Entity
{
    public class OrderRequest : BaseOrder
    {
        public string customer { get; set; }

        public bool send_reward { get; set; }

        public string campaign { get; set; }

        public OrderWithAmountRequest WithAmount(long amount)
        {
            return new OrderWithAmountRequest
                {
                    account_identifier = account_identifier,
                    amount = amount,
                    campaign = campaign,
                    customer = customer,
                    recipient = recipient,
                    reward_from = reward_from,
                    reward_subject = reward_subject,
                    send_reward = send_reward,
                    reward_message = reward_message,
                    sku = sku,
                };
        }
    }
    public class OrderWithAmountRequest : OrderRequest
    {
        public long amount { get; set; }
    }
}
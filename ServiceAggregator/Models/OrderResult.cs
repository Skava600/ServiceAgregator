namespace ServiceAggregator.Models
{
    public class OrderResult
    {
        public bool Success { get; set; } = true;
        public List<string> Errors { get; set; } = new List<string> { };
    }

    public static class OrderResultConstants
    {
        public const string ERROR_ORDER_NOT_EXIST = "Данный заказ не существует.";
        public const string ERROR_WRONG_CANCELING_OPERATION = "Нельзя отменить выполненный или отменный заказ.";
        public const string ERROR_WRONG_MARKING_DONE_OPERATION = "Нельзя пометить сделанным заказ не в исполнении.";
        public const string ERROR_UPDATING_NOT_OPEN_ORDER = "Нельзя обновить не открытый заказ.";
        public const string ERROR_NO_CUSTOMER_MAKING_ORDER = "За вашим заказом не закреплен исполнитель.";
    }
}

namespace OnlineStore.Areas.Admin.ViewModels
{
    public class AdminMenuViewModel
    {
        public bool IsProductManager { get; set; }
        public bool IsStoreOwner { get; set; }
        public bool IsOrderManager { get; set; }
        public int NumberOfOrdersInProgress { get; set; }
    }
}

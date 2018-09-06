namespace _360LawGroup.CostOfSalesBilling.Models {
    public class Token  {
        public string expires_in { get; set; }

        public string access_token { get; set; }

        public string token_type { get; set; }

        public string profile { get; set; }
        
        public string error { get; set; }

        public string role { get; set; }
    }
}
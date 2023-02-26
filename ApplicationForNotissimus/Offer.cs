using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationForNotissimus
{
    public class Offer
    {
        [Key]
        public string Id { get; set; }
        public string artist { get; set; }
        public string author { get; set; }
        public string binding { get; set; }
        public string categoryId { get; set; }
        public string country { get; set; }
        public string country_of_origin { get; set; }
        public string currencyId { get; set; }
        public string dataTour { get; set; }
        public string date { get; set; }
        public string days { get; set; }
        public string delivery { get; set; }
        public string description { get; set; }
        public string director { get; set; }
        public string downloadable { get; set; }
        public string format { get; set; }
        public string hall { get; set; }
        public string hall_part { get; set; }
        public string hotel_stars { get; set; }
        public string included { get; set; }
        public string is_kids { get; set; }
        public string is_premiere { get; set; }
        public string ISBN { get; set; }
        public string language { get; set; }
        public string local_delivery_cost { get; set; }
        public string manufacturer_warranty { get; set; }
        public string meal { get; set; }
        public string media { get; set; }
        public string model { get; set; }
        public string name { get; set; }
        public string originalName { get; set; }
        public string page_extent { get; set; }
        public string part { get; set; }
        public string performance_type { get; set; }
        public string performed_by { get; set; }
        public string picture { get; set; }
        public string place { get; set; }
        public string price { get; set; }
        public string publisher { get; set; }
        public string recording_length { get; set; }
        public string region { get; set; }
        public string room { get; set; }
        public string series { get; set; }
        public string starring { get; set; }
        public string storage { get; set; }
        public string title { get; set; }
        public string transport { get; set; }
        public string typePrefix { get; set; }
        public string url { get; set; }
        public string vendor { get; set; }
        public string vendorCode { get; set; }
        public string volume { get; set; }
        public string worldRegion { get; set; }
        public string year { get; set; }
    }
}

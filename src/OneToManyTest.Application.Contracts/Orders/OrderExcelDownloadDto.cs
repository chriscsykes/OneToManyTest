using Volo.Abp.Application.Dtos;
using System;

namespace OneToManyTest.Orders
{
    public class OrderExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string FilterText { get; set; }

        public string Item { get; set; }
        public int? QuantityMin { get; set; }
        public int? QuantityMax { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        public OrderExcelDownloadDto()
        {

        }
    }
}
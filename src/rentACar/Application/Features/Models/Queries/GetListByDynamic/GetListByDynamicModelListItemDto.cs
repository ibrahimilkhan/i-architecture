﻿namespace Application.Features.Models.Queries.GetListByDynamic
{
    public class GetListByDynamicModelListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string FuelName { get; set; }
        public string TransmissionName { get; set; }
        public int DailyPrice { get; set; }
        public string ImagePath { get; set; }
    }
}
using AutoMapper;
using CoreHangfire.Repository.DataModel;
using CoreHangfire.Service.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreHangfire.Service.Infrastructure.Mapping
{
    public class ServiceProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProfile"/> class.
        /// </summary>
        public ServiceProfile()
        {
            this.CreateMap<CustomerchoicebankDataModel, CustomerchoicebankResultModel>();
            this.CreateMap<CustomerchoicebankResultModel, CustomerchoicebankDataModel>();
        }
    }
}
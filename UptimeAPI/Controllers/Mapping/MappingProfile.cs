﻿using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UptimeAPI.Controllers.DTOs;

namespace UptimeAPI.Controllers.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, IdentityUser>()
                .ForMember(u => u.Email, opt => opt.MapFrom(ur => ur.Username))
                .ForMember(u => u.Id, opt => opt.Ignore());
            CreateMap<ResultData, ResultDataLatencyDTO>();
            CreateMap<ResultDataLatencyDTO,ResultData>();
        }
    }
}

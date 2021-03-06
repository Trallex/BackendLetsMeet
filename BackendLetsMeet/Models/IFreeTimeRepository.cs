﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public interface IFreeTimeRepository
    {
        FreeTime GetById(string id);
        List<FreeTime> GetUserFreeTime(string userId);
        List<FreeTime> GetGroupUserFreeTime(string groupId, string userId);
        FreeTime Add(FreeTime freeTime);
        FreeTime Delete(string id);
    }
}

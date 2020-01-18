using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class FreeTimeEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string GroupId { get; set; }



        public FreeTimeEntity(FreeTime freeTime)
        {
            Id = freeTime.Id;
            UserId = freeTime.UserId;
            StartTime = freeTime.StartTime.ToString("yyyy-MM-dd'T'HH:mm:ss.SSS");
            EndTime = freeTime.EndTime.ToString("yyyy-MM-dd'T'HH:mm:ss.SSS");
            GroupId = freeTime.GroupId;
        }
    }
}

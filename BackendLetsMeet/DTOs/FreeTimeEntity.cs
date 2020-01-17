using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class FreeTimeEntity
    {

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Id { get; set; }

        public FreeTimeEntity(FreeTime freeTime)
        {
            StartTime = freeTime.StartTime;
            EndTime = freeTime.EndTime;
            Id = freeTime.Id;
        }
    }
}

using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class EventEntity
    {
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Localization { get; set; }
        public List<IsGoingEntity> IsGoing { get; set; }
        public EventEntity()
        {

        }
        public EventEntity(Event _event, List<IsGoingEntity> isGoings)
        {
            Name = _event.Name;
            GroupName = _event.Group.Name;
            GroupId = _event.Group.GroupId;
            Description = _event.Description;
            StartTime = _event.StartTime;
            EndTime = _event.EndTime;
            Localization = _event.Localization;
            IsGoing = isGoings;
        }
    }
}

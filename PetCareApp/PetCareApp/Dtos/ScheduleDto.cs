﻿using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; } = 0;
        [Range(0, 6)]
        public int? DayOfWeek { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? AppUserId { get; set; }

        public ScheduleDto() { }
        public ScheduleDto(ScheduleDto dto) {
            Id = dto.Id;
            DayOfWeek = dto.DayOfWeek;
            Date = dto.Date;
            StartTime = dto.StartTime;
            EndTime = dto.EndTime;
            AppUserId = dto.AppUserId;
        }

    }
}

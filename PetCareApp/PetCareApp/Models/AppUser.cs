﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetCareApp.Models
{
    public class AppUser: IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone {  get; set; } = string.Empty;
        public byte[]? Photo {  get; set; } 

        public string? GoogleId { get; set; }
        public List<Pet> Pets { get; set; } = new List<Pet>();
        public Contacts Contacts { get; set; } = new Contacts();
        public Organization? Organization { get; set; }
        public List<OrganizationEmployee>? OrganizationEmployee { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
        public List<Break> Breaks { get; set; } = new List<Break>();
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
        public List<Record> Records { get; set; } = new List<Record>();
        public List<Review> Reviews { get; set; } = new List<Review>(); //user written reviews
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public List<RecordCancel> RecordCancels { get; set; } = new List<RecordCancel>();
        public List<HistoryEvent> HistoryEvents { get; set; }  = new List<HistoryEvent>();
    }
}

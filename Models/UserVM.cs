using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Models
{
    public class UserVM
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Intro { get; set; }
        public string Profile { get; set; }
        public string Role { get; set; }
        public string NameIdentifier { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
    public class ShowUser
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }

        public string Role { get; set; }

    }
    public class RegisterVM
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
    public class LoginVM
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
    public class PostLoginGoogleandFacebookVM
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string NameIdentifier { get; set; }
        public string ImagePath { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }

    }
}

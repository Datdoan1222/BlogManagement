using BlogManagement.Contracts;
using BlogManagement.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Repository
{
    public class VadicationFluentApi : AbstractValidator<LoginVM>
    {
        public VadicationFluentApi()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("Please input a Emai");
            RuleFor(p => p.PasswordHash).NotEmpty().WithMessage("Please input a Password");
            //RuleFor(p => p.Title).NotEmpty().WithMessage("Please input a Title").Length(1, 75);
            //RuleFor(p => p.Content).NotEmpty().WithMessage("Please input a Content for Post");
            //RuleFor(p => p.Slug).NotEmpty().WithMessage("Please input a Slug for Post").Length(1, 100);
        }
    }
    public class VadicationFluentApiCreateEdit : AbstractValidator<CreateEditPostVM>
    {
        public VadicationFluentApiCreateEdit()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Please input a Title").Length(1, 75);
            RuleFor(p => p.Content).NotEmpty().WithMessage("Please input a Content for Post");
            RuleFor(p => p.Slug).NotEmpty().WithMessage("Please input a Slug for Post").Length(1, 100);
            RuleFor(p => p.Published).NotEmpty().WithMessage("Please Choose a Published for Post");
            RuleFor(p => p.MetaTile).NotEmpty().WithMessage("Please Choose a MetaTile for Post").Length(1, 100);
            RuleFor(p => p.Summary).NotEmpty().WithMessage("Please Choose a Summary for Post").Length(1, 255);

        }
    }
    public class VadicationFluentApiLogin : AbstractValidator<RegisterVM>
    {
        public VadicationFluentApiLogin()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("Please input a Emai");
            RuleFor(p => p.PasswordHash).NotEmpty().WithMessage("Please input a Password");
            RuleFor(p => p.Mobile).NotEmpty().WithMessage("Please input a Mobile");
            RuleFor(p => p.FullName).NotEmpty().WithMessage("Please input a FullName");
        }
    }
    public class VadicationFluentCategory: AbstractValidator<CategoryVM>
    {
        public VadicationFluentCategory()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Please input a Title").Length(1, 75);
            RuleFor(p => p.Slug).NotEmpty().WithMessage("Please input a Password").Length(1, 100);

        }
    }
    public class VadicationFluentTag : AbstractValidator<TagVM>
    {
        public VadicationFluentTag()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Please input a Title").Length(1, 75);
            RuleFor(p => p.Slug).NotEmpty().WithMessage("Please input a Password").Length(1, 100);

        }
    }

}

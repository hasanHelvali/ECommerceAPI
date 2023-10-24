using ECommerceAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lutfen Urun Adini Bos Gecmeyiniz")
                .MaximumLength(150)
                .MinimumLength(5)
                    .WithMessage("Lutfen Urun Adını 5-150 Karakter Aralıgında Giriniz.");
            RuleFor(s => s.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lutfen Stok Bilgilerini Bos Bırakmayınız.")
                .Must(s => s >= 0)
                    .WithMessage("Stok Bilgisi Negatif Olamaz.");
            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Lutfen Urun Fiyatini Bos Gecmeyiniz")
                .Must(p => p >= 0)
                    .WithMessage("Fiyat Bilgisi Negatif Veya 0 Olamaz.");
        }
    }
}

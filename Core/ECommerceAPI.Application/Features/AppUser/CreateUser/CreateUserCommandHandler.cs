using ECommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using U = ECommerceAPI.Domain.Entities.Identity;

namespace ECommerceAPI.Application.Features.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<U.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<U.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult identityResult = await _userManager.CreateAsync(new U.AppUser
            {
                Id=Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                NameSurname = request.NameSurname,
                Telephone = request.Telephone,
            }, request.Password);

            CreateUserCommandResponse response= new CreateUserCommandResponse() { 
                Succeeded = identityResult.Succeeded,
            };
            if (identityResult.Succeeded)
                response.Message = "Kullanıcı Kaydı Başarıyla Yapıldı.";
            else
                foreach (var error in identityResult.Errors)
                    response.Message += $"{error.Code}-{error.Description}\n";

            return response;
            //throw new UserCreateFailedException();
        }
    }
}

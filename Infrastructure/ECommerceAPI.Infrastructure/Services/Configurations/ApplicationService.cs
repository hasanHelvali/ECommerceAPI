using ECommerceAPI.Application.Abstractions.Services.Configurations;
using ECommerceAPI.Application.CustomAttributes;
using ECommerceAPI.Application.DTOs.Configurations;
using ECommerceAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Action = ECommerceAPI.Application.DTOs.Configurations.Action;

namespace ECommerceAPI.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoint(Type type)
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();// O an calisan Assembly yi elde ediyorum.
            Assembly assembly = Assembly.GetAssembly(type);// Hangi type verildiyse o type ın bulundugu assembly elde edilmis olur.
            var controllers = assembly.GetTypes()//ne kadar tur varsa alayını elde ediyorum.
                .Where(t => t.IsAssignableTo(typeof(ControllerBase)));//ControllerBase referansından elde edilen ne kadar yapı varsa getir demis oldum.
            //Yani aslında butun controller ları elde etmis oldum.
            /*Bunun kullanıldıgı yapı api katmanı olacak. O katmandan program.cs tipini gondererek bu katmandan, yani farklı bri katmandan, o katman 
             ile ilgili buutn assembly leri elde etmis oluyorum.*/

            List<Menu> menus = new();
            if (controllers != null)
                foreach (var controller in controllers)
                {//Butun controller lar uzerinde geziyorum.
                    var actions = controller.GetMethods()//Controller lardaki metotlar uzerinde geziyorum.
                        .Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));//Buradaki attribute ile isaretlenmis olan butun metotları elde etmis oldum.
                    if (actions != null)
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if (attributes != null)
                            {
                                Menu menu = null;
                                var authorizeDefinitionAttribute = attributes.FirstOrDefault(
                                    a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
                                {
                                    menu = new() { Name = authorizeDefinitionAttribute.Menu };
                                    menus.Add(menu);
                                }
                                else
                                    menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu);

                                Application.DTOs.Configurations.Action _action = new()
                                {
                                    ActionType = Enum.GetName(typeof(ActionType), authorizeDefinitionAttribute.ActionType),//enum ın string degerini elde etmis olduk.
                                    Definition = authorizeDefinitionAttribute.Definition,
                                };
                                var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                if (httpAttribute != null)
                                    _action.HttpType = httpAttribute.HttpMethods.First();
                                else
                                    _action.HttpType = HttpMethods.Get;

                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ","")}";
                                //her action icin ayırd edici bir kod uretmeye calıstık. Bu kod uretimini ise _action nesnesinin yine kendi prop larından elde ettik.
                                menu.Actions.Add(_action);
                            }
                        }
                }
            return menus;

        }
    }
}
//Tum bu kodlar sonucunda authorize gerektiren butun action ları elde etmis oluyoruz.

using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using BookStore.Controllers;

namespace BookStore.Web.Controllers
{
    
    public class HomeController : BookStoreControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/post");
        }
    }
}

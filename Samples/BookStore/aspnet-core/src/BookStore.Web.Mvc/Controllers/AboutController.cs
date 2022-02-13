using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using BookStore.Controllers;

namespace BookStore.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : BookStoreControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}

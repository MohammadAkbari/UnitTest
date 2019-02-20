using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IRazorViewEngine razorViewEngine, IHttpContextAccessor httpContextAccessor, IViewRenderService viewRenderService)
        {
            _razorViewEngine = razorViewEngine;
            _httpContextAccessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var res = await _viewRenderService.RenderToStringAsync("View1", new Info(1, "one"));

            return View();
        }

        private Maybe<Info> Find()
        {
            //return Maybe<Info>.OfValue(new Info(1, "one"));
            return Maybe<Info>.NoValue();
        }
    }

    public struct Maybe<T>
    {
        private Maybe(bool hasValue, T value)
        {
            HasValue = hasValue;
            Value = value;
        }

        public bool HasValue { get; }

        public T Value { get; }

        public static Maybe<T> OfValue(T value)
        {
            if (value == null)
                throw new Exception("Value cannot be null");

            return new Maybe<T>(true, value);
        }

        public static Maybe<T> NoValue()
        {
            return new Maybe<T>(false, default);
        }
    }

}

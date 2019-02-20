using System.Threading.Tasks;

namespace WebApplication3.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}

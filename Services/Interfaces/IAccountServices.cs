using BarberSalon.Models.BindingModel;

namespace BarberSalon.Services.Interfaces
{
    public interface IAccountServices
    {
        public Task<int> LoginUser(LoginBindingModel l);
    }
}

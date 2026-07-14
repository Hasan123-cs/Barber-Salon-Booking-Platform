namespace BarberSalon.Services.Interfaces
{
    public interface IEmployeeServices
    {
        public  Task<List<BarberSalon.Models.Employee>> GetFirstThreeProfesionalEmployee();
        public  Task<List<BarberSalon.Models.Category>> GetCategorys();

    }
}

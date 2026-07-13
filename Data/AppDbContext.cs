using BarberSalon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<WorkingHour> WorkingHours { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // this mean 1,1
            builder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // this one to many with cascade on appoitment .. 
            builder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId);

            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId);

            builder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeId);

            builder.Entity<EmployeeService>()
                .HasOne(es => es.Service)
                .WithMany(s => s.EmployeeServices)
                .HasForeignKey(es => es.ServiceId);

            

            
            builder.Entity<WorkingHour>()
                .HasOne(w => w.Employee)
                .WithMany(e => e.WorkingHours)
                .HasForeignKey(w => w.EmployeeId);

           
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

         
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

        
            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);
            // here precision mean 18 numbers before , and 2 after is accepted only
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // TimeOnly support in postgreesql
            // to avoid store  full date 10-5-2026 ... 
            // store only the hour  e.g. 09:30
            builder.Entity<Appointment>()
                .Property(a => a.StartTime)
                .HasColumnType("time");

            builder.Entity<Appointment>()
                .Property(a => a.EndTime)
                .HasColumnType("time");

            builder.Entity<WorkingHour>()
                .Property(w => w.StartTime)
                .HasColumnType("time");

            builder.Entity<WorkingHour>()
                .Property(w => w.EndTime)
                .HasColumnType("time");
            // indexs 
            // this create index on employeeService
            // isunique i do it for no duplication (employee link to 3 service it be like a table)
            // with isunique we avoid this problem of duplication 
            builder.Entity<EmployeeService>()
                .HasIndex(es => new { es.EmployeeId, es.ServiceId })
                .IsUnique();

            // since in appointmentDate so money search for date and employee id 
         builder.Entity<Appointment>()
        .HasIndex(a => new
        {
            a.EmployeeId,
            a.AppointmentDate,
            a.StartTime
        });

            // so many time search for employee by name 
            builder.Entity<Employee>()
            .HasIndex(e => e.Name);

            // we search for the service name many time in where so add the index 
            builder.Entity<Service>()
            .HasIndex(s => s.Name);
        }
    }
}
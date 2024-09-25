using AppointmentWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentWebApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    /// </summary>
    public DbSet<Review> Reviews { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the one-to-one relationship between Appointment and Transaction
        modelBuilder.Entity<Appointment>()
                .HasOne<Transaction>() 
                .WithOne(t => t.Appointment)
                .HasForeignKey<Transaction>(t => t.AppointmentId) 
                .OnDelete(DeleteBehavior.Cascade);

        // Configure the one-to-one relationship for Patient
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId)  // Each ApplicationUser will have many appointments
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-one relationship for Doctor
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany()  // Each ApplicationUser will have many appointments
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasKey(r => r.ReviewId);

        // Enforce that a patient can only leave one review per doctor
        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.PatientId, r.DoctorId })
            .IsUnique();  // Ensures unique review per patient-doctor pair

        // Configure foreign keys
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Patient)
            .WithMany()  // Each patient can leave multiple reviews (but one per doctor)
            .HasForeignKey(r => r.PatientId);


        modelBuilder.Entity<Review>()
            .HasOne(r => r.Doctor)
            .WithMany()  // Each doctor can have many reviews
            .HasForeignKey(r => r.DoctorId);


    }
}

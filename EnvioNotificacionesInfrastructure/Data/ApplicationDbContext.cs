using System;
using System.Collections.Generic;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnvioNotificacionesInfrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
   

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Cita { get; set; }

    public virtual DbSet<TipoChequeo> TipoChequeo { get; set; }
    public virtual DbSet<LoginUsuario> LoginUsuarios { get; set; } // <--- ¡AÑADE ESTE DbSet!
    // --- ¡AÑADE ESTE DbSet PARA LA ENTIDAD CLIENTE! ---
    public virtual DbSet<Cliente> Cliente { get; set; }



    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=MCNT-78;Database=cita;User Id=sa1;Password=Med1234;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            //entity.Property(e => e.CodCit).ValueGeneratedOnAdd();
            // ¡¡¡ASEGÚRATE DE QUE ESTA LÍNEA ESTÉ PRESENTE PARA DEFINIR LA CLAVE PRIMARIA!!!
            entity.HasKey(e => e.CodCit); // <-- ¡ESTA ES LA LÍNEA CRUCIAL!

            entity.Property(e => e.CodCit).ValueGeneratedOnAdd(); // Esto es correcto para un IDENTITY(1,1)

            entity.Property(e => e.ApeMat)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.ApePat)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.AudCon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudCre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudMod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CenCos)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CodCpa).HasColumnName("CodCPa");
            entity.Property(e => e.CodTch).HasColumnName("CodTCh");
            entity.Property(e => e.CodTcl).HasColumnName("CodTCl");
            entity.Property(e => e.CodTdo).HasColumnName("CodTDo");
            entity.Property(e => e.CodTex).HasColumnName("CodTEx");
            entity.Property(e => e.CorElec).IsUnicode(false);
            entity.Property(e => e.DesDir).IsUnicode(false);
            entity.Property(e => e.ExaAdi).IsUnicode(false);
            entity.Property(e => e.FactuA)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FecCit).HasColumnType("datetime");
            entity.Property(e => e.FecEnv24).HasColumnType("datetime");
            entity.Property(e => e.FecEnv48).HasColumnType("datetime");
            entity.Property(e => e.FecEWa)
                .HasColumnType("datetime")
                .HasColumnName("FecEWa");
            //entity.Property(e => e.FecIWa)
            //    .HasColumnType("datetime")
            //    .HasColumnName("FecIWa");
            entity.Property(e => e.FecNac).HasColumnType("datetime");
            entity.Property(e => e.FecNas)
                .HasColumnType("datetime")
                .HasColumnName("FecNAs");
            entity.Property(e => e.FecReg).HasColumnType("datetime");
            entity.Property(e => e.FecRWa)
                .HasColumnType("datetime")
                .HasColumnName("FecRWa");
            entity.Property(e => e.FecTic).HasColumnType("datetime");
            entity.Property(e => e.FlagCpac).HasColumnName("FlagCPac");
            entity.Property(e => e.Gerenc).IsUnicode(false);
            entity.Property(e => e.IndAas).HasColumnName("IndAAs");
            entity.Property(e => e.IndEcm).HasColumnName("IndECM");
            entity.Property(e => e.IndEWa).HasColumnName("IndEWa");
            //entity.Property(e => e.IndIWa).HasColumnName("IndIWa");
            entity.Property(e => e.IndRWa).HasColumnName("IndRWa");
            entity.Property(e => e.MsjErrW).IsUnicode(false);
            entity.Property(e => e.MsjERW)
                .IsUnicode(false)
                .HasColumnName("MsjERW");
            //entity.Property(e => e.MsjIRW)
            //    .IsUnicode(false)
            //    .HasColumnName("MsjIRW");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.NroCel)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.NroCpa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NroCPa");
            entity.Property(e => e.NroReq)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NroTlf)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.NumDid)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("NumDId");
            entity.Property(e => e.Observ)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PueAct)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Responsable)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SexPac)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SubCon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ZonLab)
                .HasMaxLength(50)
                .IsUnicode(false);
            // --- ¡YA TIENES ESTA RELACIÓN, AHORA AÑADIMOS LA DE CLIENTE! ---
            entity.HasOne(d => d.TipoChequeo)
                .WithMany()
                .HasForeignKey(d => d.CodTch)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK__Cita_TipoChequeo");


            // --- ¡AÑADE ESTA CONFIGURACIÓN PARA LA RELACIÓN CITA CON CLIENTE! ---
            entity.HasOne(d => d.Cliente) // d.Cliente es la propiedad de navegación en Cita
                  .WithMany() // Cliente puede tener muchas Citas (no necesitamos una navegación inversa en Cliente por ahora)
                  .HasForeignKey(d => d.CodCli) // La clave foránea en Cita es CodCli
                  .OnDelete(DeleteBehavior.NoAction) // O .Restrict, .SetNull, etc., según tu lógica de negocio
                  .HasConstraintName("FK_Cita_Cliente"); // Este será el nombre de la FK que se creará en la DB
        });

        modelBuilder.Entity<TipoChequeo>(entity =>
        {
            entity.HasKey(e => e.CodTCh);

            //entity.ToTable("TipoChequeo");

            //entity.HasIndex(e => e.DesTCh, "UQ__TipoCheq__92C53B6CA20698EE").IsUnique();

            entity.Property(e => e.CodTCh).ValueGeneratedOnAdd();
            entity.Property(e => e.DesTCh)

                .IsRequired()
                .HasMaxLength(100);
        });

        // --- ¡AÑADE ESTA CONFIGURACIÓN PARA LA ENTIDAD CLIENTE! ---
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CodCli); // Define la clave primaria
            entity.Property(e => e.CodCli).ValueGeneratedOnAdd(); // Si el CodCli de Cliente es IDENTITY
            entity.Property(e => e.NomEmp)
                .IsRequired()
                .HasMaxLength(255); // Ajusta la longitud según tu necesidad
        });


        //--- ¡AÑADE ESTA CONFIGURACIÓN PARA USUARIO!-- -
       modelBuilder.Entity<LoginUsuario>(entity =>
       {
           entity.HasKey(u => u.CodUsr); // Define la clave primaria
           entity.Property(u => u.CodUsr).ValueGeneratedOnAdd(); // Si el ID es autoincremental
           entity.Property(u => u.NOMUSR)
               .IsRequired()
               .HasMaxLength(50); // Define la longitud máxima y que es requerido
           entity.Property(u => u.NOMCOM)
               .IsRequired()
               .HasMaxLength(256); // Longitud del hash SHA256 (SHA256 genera 64 caracteres hex)
           entity.Property(u => u.Rol)
               .IsRequired()
               .HasMaxLength(50)
               .HasDefaultValue("LoginUsuario");
           entity.Property(u => u.Activo)
               .HasDefaultValue(true);
           entity.HasIndex(u => u.NOMUSR).IsUnique(); // El nombre de usuario debe ser único
       });



        //// --- ¡AÑADE ESTOS DATOS DE SEMILLA PARA USUARIO! ---
        //var initialPassword = "adminpass"; // La contraseña inicial
        //var passwordHasher = new EnvioNotificacionesInfrastructure.Security.PasswordHasher(); // Asegúrate de la ruta correcta
        //var hashedPassword = passwordHasher.HashPassword(initialPassword);

        //modelBuilder.Entity<LoginUsuario>().HasData(
        //    new LoginUsuario
        //    {
        //        LogUId = 1,
        //        Username = "admin",
        //        PasswordHash = hashedPassword,
        //        Rol = "Administrador",
        //        Activo = true
        //    }
        //);

        ////3.Carga inicial de datos para EstadoNotificacion(si la usas para el Enum)
        //modelBuilder.Entity<EstadoNotificacion>().HasData(
        //    new EstadoNotificacion { EstadoNotificacionID = (int)EstadoNotificacionEnum.PENDIENTE, NombreEstado = "PENDIENTE" },
        //    new EstadoNotificacion { EstadoNotificacionID = (int)EstadoNotificacionEnum.ENVIADO, NombreEstado = "ENVIADO" },
        //    new EstadoNotificacion { EstadoNotificacionID = (int)EstadoNotificacionEnum.FALLIDO, NombreEstado = "FALLIDO" }
        //);

        // 4. Carga inicial de datos para TipoCita (si la usas para el Enum)
        //// Si TipoChequeo es la nueva tabla, y quieres precargarla, hazlo aquí:
        //modelBuilder.Entity<TipoChequeo>().HasData(
        //    new TipoChequeo { CodTCh = 1, DesTCh = "PREOCUPACIONAL" },
        //    new TipoChequeo { CodTCh = 2, DesTCh = "OFTALMOLOGÍA" },
        //    new TipoChequeo { CodTCh = 3, DesTCh = "ODONTOLOGÍA" },
        //    new TipoChequeo { CodTCh = 4, DesTCh = "CARDIOLOGÍA" },
        //    new TipoChequeo { CodTCh = 5, DesTCh = "NEUROLOGÍA" },
        //    new TipoChequeo { CodTCh = 6, DesTCh = "TRAUMATOLOGÍA" }
        //        // ...
        //);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

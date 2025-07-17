using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;

namespace EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

public partial class Cita
{
    public int CodCit { get; set; }

    public int? CodTdo { get; set; }

    public int? CodCpa { get; set; }

    public int CodCli { get; set; }

    public DateTime FecCit { get; set; }

    public string? Nombre { get; set; }

    public string? ApePat { get; set; }

    public string? ApeMat { get; set; }

    public string? SexPac { get; set; }

    public DateTime? FecNac { get; set; }

    public string? NumDid { get; set; }

    public int? CodEmp { get; set; }

    public int? CodSed { get; set; }

    public int? CodTcl { get; set; }

    public int? NumOrd { get; set; }

    public int? NumTic { get; set; }

    public DateTime? FecTic { get; set; }

    public int? CodDes { get; set; }

    public string? PueAct { get; set; }

    public string? CenCos { get; set; }

    public string? ZonLab { get; set; }

    public int? AreTra { get; set; }

    public int CodTch { get; set; }

    public string? Observ { get; set; }

    public string? SubCon { get; set; }

    public string? FactuA { get; set; }

    public int? EdaPac { get; set; }

    public string? Responsable { get; set; }

    public string? Gerenc { get; set; }

    public bool? IndCon { get; set; }

    public bool? IndReg { get; set; }

    public string? AudCre { get; set; }

    public string? AudMod { get; set; }

    public string? AudCon { get; set; }

    public int? TipPag { get; set; }

    public string? NroCpa { get; set; }

    public string? NroReq { get; set; }

    public int? TipoCarga { get; set; }

    public bool? PruCov { get; set; }

    public int? CodFic { get; set; }

    public int? CodHor { get; set; }

    public string? CorElec { get; set; }

    public bool? InHouse { get; set; }

    public string? DesDir { get; set; }

    public bool? EmoCov { get; set; }

    public bool? FlagCpac { get; set; }

    public bool? PruMol { get; set; }

    public bool? EmoMol { get; set; }

    public bool? PruAnt { get; set; }

    public bool? EmoAnt { get; set; }

    public bool? PruEclia { get; set; }

    public bool? EmoEclia { get; set; }

    public bool? PruElisa { get; set; }

    public bool? EmoElisa { get; set; }

    public int? CodTex { get; set; }

    public int? IndEnv { get; set; }

    public string? ExaAdi { get; set; }

    public bool? Campania { get; set; }

    public bool? IndEnv48 { get; set; }

    public DateTime? FecEnv48 { get; set; }

    public bool? IndEnv24 { get; set; }

    public DateTime? FecEnv24 { get; set; }

    public bool? IndEcm { get; set; }

    public int? RefCodCli { get; set; }

    public DateTime? FecReg { get; set; }

    public int? NoAsis { get; set; }

    public DateTime? FecNas { get; set; }

    //[NotMapped] // ¡NUEVO! Indica que esta propiedad no mapea a una columna de la DB solo se usa si tus campos llevan alias para que no haya errores 
    public string? DesTCh { get; set; }
    //[NotMapped] // ¡NUEVO! Indica que esta propiedad no mapea a una columna de la DB solo se usa si tus campos llevan alias para que no haya errores 
    public string? NomCom { get; set; }
    public int? IndAas { get; set; }

    public string? NroCel { get; set; }

    public string NroTlf { get; set; }

    public int? CodOri { get; set; }
    // Nuevos campos de notificación
    public bool? IndEWa { get; set; } // Indicador de envío de Bienvenida
    public DateTime? FecEWa { get; set; } // Fecha/Hora del último intento de Bienvenida
    public string? MsjErrW { get; set; } // Mensaje de error de Bienvenida

    public bool? IndRWa { get; set; } // Indicador de envío de Recordatorio
    public DateTime? FecRWa { get; set; } // Fecha/Hora del último intento de Recordatorio
    public string? MsjERW { get; set; } // Mensaje de error de Recordatorio

   
    public int NroInt { get; set; }

    // --- Propiedad de Navegación a TipoChequeo ---
    // Scaffold-DbContext la generará basándose en CodTCh y la FK.
    // El nombre exacto puede variar (ej. CodTChNavigation, TipoChequeo).
    // Asumiremos que se llama 'CodTChNavigation' por ahora, pero verifica lo generado.
    public virtual TipoChequeo TipoChequeo { get; set; }
    //// En tu entidad Cita.cs
    public virtual Cliente Cliente { get; set; } // Esta es la propiedad de navegación

    // Propiedades calculadas para los estados de notificación (para el DTO)
    public EstadoNotificacionEnum EstadoNotificacionBienvenida
    {
        get => IndEWa switch
        {
            true => EstadoNotificacionEnum.ENVIADO,
            false => EstadoNotificacionEnum.FALLIDO,
            _ => EstadoNotificacionEnum.PENDIENTE // NULL en DB
        };
    }

    public EstadoNotificacionEnum EstadoNotificacionRecordatorio
    {
        get => IndRWa switch
        {
            true => EstadoNotificacionEnum.ENVIADO,
            false => EstadoNotificacionEnum.FALLIDO,
            _ => EstadoNotificacionEnum.PENDIENTE
        };
    }

   


    // --- Lógica de Negocio (Adapta tus métodos existentes) ---
    // Estos métodos deben actualizar los campos de la DB directamente
    public void MarcarBienvenidaComoEnviada()
    {
        IndEWa = true;
        FecEWa = DateTime.Now;
        MsjErrW = null;
 
    }

    public void MarcarBienvenidaComoFallida(string? errorMessage)
    {
        IndEWa = false;
        FecEWa = DateTime.Now;
        MsjErrW = errorMessage;
        NroInt++;
    }
    public void MarcarRecordatorioComoEnviado()
    {
        IndRWa = true;
        FecRWa = DateTime.Now;
        MsjERW = null;
       
    }

    public void MarcarRecordatorioComoFallido(string? errorMessage)
    {
        IndRWa = false;
        FecRWa = DateTime.Now;
        MsjERW = errorMessage;
        NroInt++;
    }

    // Y para Finalizado:
  
}

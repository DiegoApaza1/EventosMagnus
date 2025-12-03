using Magnus.Domain.Enums;
using Magnus.Domain.Exceptions;

namespace Magnus.Domain.Entities;

public class EventoInvitado
{
    public Guid Id { get; private set; }
    public Guid EventoId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public EstadoInvitacion Estado { get; private set; }
    public bool EsAutopostulacion { get; private set; }
    public DateTime FechaInvitacion { get; private set; }
    public DateTime? FechaRespuesta { get; private set; }
    public string? Mensaje { get; private set; }
    
    public Evento? Evento { get; private set; }
    public Usuario? Usuario { get; private set; }

    private EventoInvitado() { }

    public EventoInvitado(Guid eventoId, Guid usuarioId, bool esAutopostulacion, string? mensaje = null)
    {
        Id = Guid.NewGuid();
        EventoId = eventoId;
        UsuarioId = usuarioId;
        EsAutopostulacion = esAutopostulacion;
        Estado = esAutopostulacion ? EstadoInvitacion.PENDIENTE_APROBACION : EstadoInvitacion.PENDIENTE_RESPUESTA;
        FechaInvitacion = DateTime.UtcNow;
        Mensaje = mensaje;
    }

    public void AceptarPorInvitado()
    {
        if (Estado != EstadoInvitacion.PENDIENTE_RESPUESTA)
            throw new DomainException("Solo se pueden aceptar invitaciones pendientes de respuesta.");
        
        Estado = EstadoInvitacion.CONFIRMADO;
        FechaRespuesta = DateTime.UtcNow;
    }

    public void RechazarPorInvitado()
    {
        if (Estado != EstadoInvitacion.PENDIENTE_RESPUESTA)
            throw new DomainException("Solo se pueden rechazar invitaciones pendientes de respuesta.");
        
        Estado = EstadoInvitacion.RECHAZADO_POR_INVITADO;
        FechaRespuesta = DateTime.UtcNow;
    }

    public void AprobarPorOrganizador()
    {
        if (Estado != EstadoInvitacion.PENDIENTE_APROBACION)
            throw new DomainException("Solo se pueden aprobar autopostulaciones pendientes.");
        
        Estado = EstadoInvitacion.CONFIRMADO;
        FechaRespuesta = DateTime.UtcNow;
    }

    public void RechazarPorOrganizador()
    {
        if (Estado != EstadoInvitacion.PENDIENTE_APROBACION)
            throw new DomainException("Solo se pueden rechazar autopostulaciones pendientes.");
        
        Estado = EstadoInvitacion.RECHAZADO_POR_ORGANIZADOR;
        FechaRespuesta = DateTime.UtcNow;
    }
}

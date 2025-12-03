namespace Magnus.Domain.Enums;

public enum EstadoInvitacion
{
    PENDIENTE_RESPUESTA = 0,           
    PENDIENTE_APROBACION = 1,          
    CONFIRMADO = 2,                    
    RECHAZADO_POR_INVITADO = 3,       
    RECHAZADO_POR_ORGANIZADOR = 4     
}

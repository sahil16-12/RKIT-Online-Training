using backend.DTOs;

namespace backend.Models
{
    /// <summary>
    /// Represents in-memory state for create appointment workflow execution.
    /// </summary>
    public class AppointmentCreateWorkflowState
    {
        public int PatientUserId { get; set; }

        public CreateAppointmentRequest Request { get; set; } = null!;

        public TBL04 Appointment { get; set; } = null!;
    }
}

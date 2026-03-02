using backend.DTOs;

namespace backend.Models
{
    /// <summary>
    /// Represents in-memory state for cancel appointment workflow execution.
    /// </summary>
    public class AppointmentCancelWorkflowState
    {
        public int DoctorUserId { get; set; }

        public int AppointmentId { get; set; }

        public CancelAppointmentRequest Request { get; set; } = null!;

        public TBL04? Appointment { get; set; }
    }
}

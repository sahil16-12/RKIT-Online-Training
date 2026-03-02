using backend.DTOs;

namespace backend.Models
{
    /// <summary>
    /// Represents in-memory state for decide appointment workflow execution.
    /// </summary>
    public class AppointmentDecisionWorkflowState
    {
        public int DoctorUserId { get; set; }

        public int AppointmentId { get; set; }

        public AppointmentDecisionRequest Request { get; set; } = null!;

        public TBL04? Appointment { get; set; }
    }
}

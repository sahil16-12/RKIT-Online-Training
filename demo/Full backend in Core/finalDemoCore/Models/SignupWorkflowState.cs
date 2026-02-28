using backend.DTOs;

namespace backend.Models
{
    /// <summary>
    /// Represents in-memory state for signup workflow execution.
    /// </summary>
    public class SignupWorkflowState
    {
        public SignupRequest Request { get; set; } = null!;

        public TBL01 User { get; set; } = null!;

        public TBL02? Patient { get; set; }

        public TBL03? Doctor { get; set; }
    }
}

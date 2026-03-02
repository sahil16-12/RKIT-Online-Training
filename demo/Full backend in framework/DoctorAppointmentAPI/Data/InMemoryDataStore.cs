using DoctorAppointmentAPI.Exceptions;
using DoctorAppointmentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DoctorAppointmentAPI.Data
{
    /// <summary>
    /// Simple in-memory data store for users and appointments.
    /// Acts as a singleton to maintain application-wide state.
    /// </summary>
    public class InMemoryDataStore
    {
        private static InMemoryDataStore _instance;
        private readonly static List<TBL03> _appointments = new List<TBL03>();
        private readonly static List<TBL01> _users = new List<TBL01>();
        private static int _userId = 1;
        private static int _appointmentId = 1;

        /// <summary>
        /// Gets the single instance of the in-memory data store.
        /// </summary>
        public static InMemoryDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InMemoryDataStore();
                }

                return _instance;
            }
        }

        // USER OPERATIONS

        /// <summary>
        /// Retrieves a user based on username (case-insensitive).
        /// </summary>
        public TBL01 GetByUsername(string username)
        {
            return _users.FirstOrDefault(u =>
                u.L01F02.Equals(username)); // Username
        }

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        public List<TBL01> GetAllUsers()
        {
            return _users.ToList();
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public TBL01 GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.L01F01 == id); // Id
        }

        /// <summary>
        /// Adds a new user to the data store after hashing the password.
        /// </summary>
        public TBL01 AddUser(TBL01 user)
        {
            user.L01F01 = _userId++; // Id
            user.L01F03 = HashPassword(user.L01F03); // Password
            user.L01F08 = DateTime.UtcNow; // CreatedDate

            _users.Add(user);
            return user;
        }

        /// <summary>
        /// Validates user credentials by comparing hashed passwords.
        /// </summary>
        public TBL01 ValidateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            TBL01 user = _users.FirstOrDefault(u =>
                u.L01F02.Equals(username) && // Username
                u.L01F03 == hashedPassword); // Password

            return user;
        }

        // PASSWORD HASHING

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Retrieves all appointments.
        /// </summary>
        public List<TBL03> GetAllAppointments()
        {
            return _appointments.ToList();
        }

        /// <summary>
        /// Retrieves appointments for a specific doctor.
        /// </summary>
        public List<TBL03> GetByDoctor(int doctorId)
        {
            return _appointments.Where(a => a.L03F02 == doctorId).ToList(); // DoctorId
        }

        /// <summary>
        /// Retrieves appointments for a specific patient.
        /// </summary>
        public List<TBL03> GetByPatient(int patientId)
        {
            return _appointments.Where(a => a.L03F03 == patientId).ToList(); // PatientId
        }

        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        public TBL03 GetById(int id)
        {
            return _appointments.FirstOrDefault(a => a.L03F01 == id); // Id
        }

        /// <summary>
        /// Books a new appointment if the time slot is available.
        /// </summary>
        public TBL03 BookAppointment(TBL03 appointment)
        {
            bool slotAlreadyBooked = _appointments.Any(a =>
                a.L03F02 == appointment.L03F02 && // DoctorId
                a.L03F04 == appointment.L03F04 && // AppointmentDate
                a.L03F05 != "Cancelled"); // Status

            if (slotAlreadyBooked)
                throw new BusinessException("This time slot is already booked");

            appointment.L03F01 = _appointmentId++; // Id
            appointment.L03F05 = "Pending"; // Status
            appointment.L03F06 = DateTime.UtcNow; // CreatedDate

            _appointments.Add(appointment);
            return appointment;
        }

        /// <summary>
        /// Updates the status of an existing appointment.
        /// </summary>
        public void UpdateStatus(int appointmentId, string status)
        {
            TBL03 appointment = GetById(appointmentId);

            if (appointment == null)
                throw new BusinessException("Appointment not found");

            appointment.L03F05 = status; // Status
        }

        /// <summary>
        /// Cancels an appointment by setting its status to Cancelled.
        /// </summary>
        public void Cancel(int appointmentId)
        {
            TBL03 appointment = GetById(appointmentId);

            if (appointment == null)
                throw new BusinessException("Appointment not found");

            appointment.L03F05 = "Cancelled"; // Status
        }
    }
}

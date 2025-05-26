using System.Data.SqlClient;

namespace HRManagementSystem
{
    public class EmployeeService : IService<Employee>
    {
        private readonly string _connectionString;

        public EmployeeService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT e.Id,e.EmployeeId, e.EmployeeType,e.Position,  e.Name, e.Email, e.Phone, e.Address, e.DepartmentId, d.Name AS DepartmentName
                                 FROM Employee e
                                 LEFT JOIN Department d ON e.DepartmentId = d.DepartmentId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader["Id"].ToString(),
                            EmployeeId = reader["EmployeeId"].ToString(),
                            EmployeeType = reader["EmployeeType"].ToString(),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Position = reader["Position"].ToString(),
                            Address = reader["Address"].ToString(),
                            DepartmentId = reader["DepartmentId"].ToString(),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            return employees;
        }

        public Employee GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT e.Id,e.EmployeeId, e.EmployeeType,e.Position,  e.Name, e.Email, e.Phone, e.Address, e.DepartmentId, d.Name AS DepartmentName
                                 FROM Employee e
                                 LEFT JOIN Department d ON e.DepartmentId = d.DepartmentId
                                 WHERE e.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                Id = reader["Id"].ToString(),
                                EmployeeId = reader["EmployeeId"].ToString(),
                                EmployeeType = reader["EmployeeType"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Position = reader["Position"].ToString(),
                                Address = reader["Address"].ToString(),
                                DepartmentId = reader["DepartmentId"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        public Employee GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId)) throw new ArgumentNullException(nameof(employeeId));

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT e.Id, e.EmployeeId, e.EmployeeType, e.Position, e.Name, e.Email, e.Phone, e.Address, e.DepartmentId, d.Name AS DepartmentName
                         FROM Employee e
                         LEFT JOIN Department d ON e.DepartmentId = d.DepartmentId
                         WHERE e.EmployeeId = @EmployeeId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                Id = reader["Id"].ToString(),
                                EmployeeId = reader["EmployeeId"].ToString(),
                                EmployeeType = reader["EmployeeType"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Position = reader["Position"].ToString(),
                                Address = reader["Address"].ToString(),
                                DepartmentId = reader["DepartmentId"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool Add(Employee entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Employee 
                         (Id, EmployeeId, Name, Email, Phone, DateOfBirth, Address, HireDate, Position, BaseSalary, DepartmentId, Status, EmployeeType, AnnualBonus)
                         VALUES 
                         (@Id, @EmployeeId, @Name, @Email, @Phone, @DateOfBirth, @Address, @HireDate, @Position, @BaseSalary, @DepartmentId, @Status, @EmployeeType, @AnnualBonus)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);
                    cmd.Parameters.AddWithValue("@Name", entity.Name);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);
                    cmd.Parameters.AddWithValue("@Phone", entity.Phone);
                    cmd.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", entity.Address);
                    cmd.Parameters.AddWithValue("@HireDate", entity.HireDate);
                    cmd.Parameters.AddWithValue("@Position", entity.Position);
                    cmd.Parameters.AddWithValue("@BaseSalary", entity.BaseSalary);
                    cmd.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (int)entity.Status);
                    cmd.Parameters.AddWithValue("@EmployeeType", entity.EmployeeType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnnualBonus", 0);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Employee entity)
        {
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                throw new ArgumentNullException(nameof(entity));

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
            UPDATE Employee SET 
                EmployeeId = @EmployeeId,
                Name = @Name,
                Email = @Email,
                Phone = @Phone,
                DateOfBirth = @DateOfBirth,
                Address = @Address,
                HireDate = @HireDate,
                Position = @Position,
                BaseSalary = @BaseSalary,
                DepartmentId = @DepartmentId,
                Status = @Status,
                EmployeeType = @EmployeeType,
                AnnualBonus = @AnnualBonus
            WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);
                    cmd.Parameters.AddWithValue("@Name", entity.Name);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);
                    cmd.Parameters.AddWithValue("@Phone", entity.Phone);
                    cmd.Parameters.AddWithValue("@DateOfBirth", entity.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Address", entity.Address);
                    cmd.Parameters.AddWithValue("@HireDate", entity.HireDate);
                    cmd.Parameters.AddWithValue("@Position", entity.Position);
                    cmd.Parameters.AddWithValue("@BaseSalary", entity.BaseSalary);
                    cmd.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (int)entity.Status);
                    cmd.Parameters.AddWithValue("@EmployeeType", entity.EmployeeType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnnualBonus", 0);

                    cmd.Parameters.AddWithValue("@Id", entity.Id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Employee WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

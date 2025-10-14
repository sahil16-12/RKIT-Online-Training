// STEP 1: DataTable and its structure
using System.Data;

DataTable employeeTable = new DataTable("Employees");
employeeTable.Columns.Add("ID", typeof(int));
employeeTable.Columns.Add("Name", typeof(string));
employeeTable.Columns.Add("Department", typeof(string));
employeeTable.Columns.Add("Salary", typeof(double));

// STEP 2: Sample data
employeeTable.Rows.Add(1, "Sahil", "HR", 50000);
employeeTable.Rows.Add(2, "Hakim", "IT", 70000);
employeeTable.Rows.Add(3, "Nahin", "Finance", 60000);
employeeTable.Rows.Add(4, "Sameed", "IT", 80000);
employeeTable.Rows.Add(5, "Danish", "HR", 55000);

Console.WriteLine("=== Original DataTable ===");
DisplayTable(employeeTable);

// STEP 3: DataView from the DataTable
DataView view = new DataView(employeeTable);

// STEP 4: Only IT department employees
view.RowFilter = "Department = 'IT'";

Console.WriteLine("\n=== Filtered View (Department = 'IT') ===");
DisplayView(view);

// STEP 5: sort by Salary descending
view.Sort = "Salary DESC";

Console.WriteLine("\n=== Sorted View (Salary DESC) ===");
DisplayView(view);

// STEP 6: Distinct departments using ToTable()
DataTable distinctDepartments = view.ToTable(true, "Department");
Console.WriteLine("\n=== Distinct Departments ===");
DisplayDepartments(distinctDepartments);

// STEP 7: DataView updates when DataTable changes
Console.WriteLine("\n=== After Adding New Row to DataTable ===");
employeeTable.Rows.Add(6, "Saif", "IT", 75000);
DisplayView(view);
        

static void DisplayDepartments(DataTable table)
{
    foreach (DataRow row in table.Rows)
        Console.WriteLine($"{row["Department"]}");
}

// Helper method to display DataTable contents
static void DisplayTable(DataTable table)
{
    foreach (DataRow row in table.Rows)
        Console.WriteLine($"{row["ID"],-2} {row["Name"],-8} {row["Department"],-10} {row["Salary"],8}");
}

// Helper method to display DataView contents
static void DisplayView(DataView view)
{
    foreach (DataRowView rowView in view)
        Console.WriteLine($"{rowView["ID"],-2} {rowView["Name"],-8} {rowView["Department"],-10} {rowView["Salary"],8}");
}
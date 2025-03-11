/*
Grading Program by Jay
- This program can add new student records and link test scores to the student number
- Display all current student records and show specific ones if you know the student number
- Delete a student record or test score
- Calculate grades of student
*/

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

/// Classifying the program
namespace Grading_Program
{
    internal class Program
    {
        public class TestRecord
        {
            /// <summary>
            /// Setting all test record definitions for the program
            /// </summary>
            public decimal StudentMark { get; set; }
            public decimal TestOutOf { get; set; }
            public decimal Percentage { get; set; }
            public string Grade { get; set; }
        }

        public class Student
        {
            /// <summary>
            /// Setting all student record definitions for the program
            /// </summary>
            public string StudentNo { get; set; }

            /// <summary>
            /// A list for TestRecord objects that represents the student's test results inside the program
            /// </summary>
            public List<TestRecord> TestRecords { get; set; } = new List<TestRecord>();
        }

        /// <summary>
        /// Setting up the JSON file and list link and setting them as global variables so all parts of the program can reference them 
        /// </summary>
        static List<Student> students = new List<Student>();
        static string fileName = "students.json";

        /// Program Start
        static void Main(string[] args)
        {
            Console.Title = "Torrens Grading Program";

            // Change the color of the heading to orange (DarkYellow)
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("--- Torrens Grading Program ---\n");
            Console.ResetColor();

            // Calling function to load the old student data into the program
            LoadStudentData();

            /// Boolean flag used to control the loop in the program
            bool exit = false;

            while (!exit)
            {
                // Print line split in cyan color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("----------------------------");
                Console.ResetColor();

                Console.WriteLine("Menu:");
                Console.WriteLine("1. Display existing student records");
                Console.WriteLine("2. Add new test record");
                Console.WriteLine("3. Delete a student record");
                Console.WriteLine("4. Exit");

                // Print line split in cyan color
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("----------------------------");
                Console.ResetColor();

                Console.Write("Please select an option (1-4): ");
                string clientChoice = Console.ReadLine();

                /// Display all menu options and run functions associated with the number the user types
                /// Used a switch statement for user input handling, ensuring that if they type anything undesirable, it asks again
                switch (clientChoice)
                {
                    case "1":
                        DisplayStudentRecords();
                        break;
                    case "2":
                        AddNewStudentRecord();
                        break;
                    case "3":
                        DeleteStudentRecord();
                        break;
                    case "4":
                        exit = true;
                        // Print exit message with line splits
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("----------------------------");
                        Console.ResetColor();

                        Console.WriteLine("Exiting Program Now...");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("----------------------------");
                        Console.ResetColor();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select an option between 1-4.");
                        break;
                }
            }
        }

        static void SaveStudentData()
        {
            try
            {
                string json = JsonConvert.SerializeObject(students, Newtonsoft.Json.Formatting.Indented); // formates json file with the newtonsoft.json system
                File.WriteAllText(fileName, json); // Writing the data into the JSON file 
                Console.WriteLine("Student data saved successfully."); // Acknowledging success
            }
            catch (Exception ex)
            {
                // Error handling with saving student data
                Console.WriteLine($"Error saving student data: {ex.Message}");
            }
        }

        /// <summary>
        /// Function for loading student data from the JSON file
        /// </summary>
        static void LoadStudentData()
        {
            if (File.Exists(fileName)) // Checking if file exists
            {
                try
                {
                    string json = File.ReadAllText(fileName); // Reading all information from the JSON file
                    students = JsonConvert.DeserializeObject<List<Student>>(json); // Converting the JSON and sending the information into the "students" list
                    Console.WriteLine("Student data loaded successfully."); // Confirming that the reading of the data was successful
                }
                catch (Exception ex)
                {
                    // Error handling if the file doesn't exist 
                    Console.WriteLine($"Error loading student data: {ex.Message}");
                    students = new List<Student>();
                }
            }
            else
            {
                // Create an empty list and save it to the newly created JSON file
                students = new List<Student>();
                SaveStudentData();
                Console.WriteLine("No existing student data found. Created new data file.");
            }
        }

        /// <summary>
        /// Options for displaying the data to the user
        /// </summary>
        static void DisplayStudentRecords()
        {
            if (students.Count == 0) // If no students are in the JSON file, it displays a message
            {
                Console.WriteLine("No student records to display.");
                return;
            }

            // Print line split in dark cyan color
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------");
            Console.ResetColor();

            Console.WriteLine("Display Options:");
            Console.WriteLine("1. Display all records");
            Console.WriteLine("2. Display a specific student's record");

            // Print line split in dark cyan color
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------");
            Console.ResetColor();

            Console.Write("Please select an option (1-2): ");
            string displayChoice = Console.ReadLine(); // Calling the switch function below

            switch (displayChoice) // Switch statement to handle user choice
            {
                case "1":
                    // Runs display all records function
                    DisplayAllRecords();
                    break;
                case "2":
                    // Runs display specific student record function
                    DisplaySpecificRecord();
                    break;
                default:
                    // Error handling
                    Console.WriteLine("Invalid option. Returning to main menu.");
                    break;
            }
        }

        static void DisplayAllRecords()
        {

            Console.WriteLine("Existing Student Records:");

            foreach (var student in students) // Loop through all students
            {
                // Print line split in dark red color
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("----------------------------");
                Console.ResetColor();

                Console.WriteLine($"Student No: {student.StudentNo}");
               
                Console.WriteLine("Test Records:");
                int testNumber = 1;
                foreach (var test in student.TestRecords) // Loop through each student's test records
                {
                    Console.WriteLine($"Test #{testNumber}");
                    Console.WriteLine($"Mark: {test.StudentMark}/{test.TestOutOf}");
                    Console.WriteLine($"Percentage: {test.Percentage:F2}%");
                    Console.WriteLine($"Grade: {test.Grade}");
                    testNumber++;
                }

                // Print line split in dark red color at the end of each student
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("----------------------------\n");
                Console.ResetColor();
            }
        }

        static void DisplaySpecificRecord()
        {
            Console.Write("Enter the Student Number to search: ");
            string studentNo = Console.ReadLine();
            // Searches for the student in the list using a lambda expression
            var student = students.Find(s => s.StudentNo.Equals(studentNo, StringComparison.OrdinalIgnoreCase));

            if (student != null) // If record found in the students list, then print the records
            {
                DisplayStudentRecord(student);
            }
            else // Print a line saying the record isn't found for that student number
            {
                Console.WriteLine($"No record found for Student Number: {studentNo}");
            }
        }

        /// <summary>
        /// Displays the record of a specific student
        /// </summary>
        static void DisplayStudentRecord(Student student)
        {
            Console.WriteLine("Student Record:");
            Console.WriteLine($"Student No: {student.StudentNo}");
            // Print line split in dark red color at the start
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("----------------------------");
            Console.ResetColor();

            Console.WriteLine("Test Records:");
            int testNumber = 1;
            foreach (var test in student.TestRecords)
            {
                Console.WriteLine($"Test #{testNumber}");
                Console.WriteLine($"Mark: {test.StudentMark}/{test.TestOutOf}");
                Console.WriteLine($"Percentage: {test.Percentage:F2}%");
                Console.WriteLine($"Grade: {test.Grade}");
                testNumber++;
            }

            // Print line split in dark red color at the end
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("----------------------------");
            Console.ResetColor();
        }

        static void AddNewStudentRecord()
        {
            // Print line split in cyan color
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----------------------------");
            Console.ResetColor();

            Console.Write("Enter Student Number: ");
            string studentNo = Console.ReadLine(); // Reading the input from the user

            // Check if student already exists
            var existingStudent = students.Find(s => s.StudentNo.Equals(studentNo, StringComparison.OrdinalIgnoreCase));

            if (existingStudent != null)
            {
                // Add new test record to existing student 
                Console.WriteLine($"Adding a new test record for Student Number {studentNo}.");
                var newTestRecord = CreateTestRecord(); // Calls method to get test details
                existingStudent.TestRecords.Add(newTestRecord); // Adds new test record to the existing student's list of tests
                Console.WriteLine("New test record added successfully."); // Confirming that it was able to add the record
                SaveStudentData(); // Saving the updated data
            }
            else
            {
                // Create a new student record and add the test record to the new student
                Console.WriteLine("Student not found. Creating a new student record.");
                var newStudent = new Student { StudentNo = studentNo }; // Creates the new student
                var newTestRecord = CreateTestRecord(); // Calls method to get test details
                newStudent.TestRecords.Add(newTestRecord); // Add the new record to the student's list
                students.Add(newStudent); // Adds the new student to the students list
                Console.WriteLine("Student record added successfully."); // Confirming the program worked
                SaveStudentData(); // Saving all updated data
            }
        }

        static TestRecord CreateTestRecord()
        {
            decimal studentMark;
            decimal testOutOf;

            while (true)
            {
                Console.Write("Enter Student Mark: ");
                // Validate that the input is a non-negative decimal
                if (!decimal.TryParse(Console.ReadLine(), out studentMark) || studentMark < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid non-negative decimal value for Student Mark.");
                    continue;
                }

                Console.Write("Enter Test Out Of: ");
                // Validate that the input is a positive decimal
                if (!decimal.TryParse(Console.ReadLine(), out testOutOf) || testOutOf <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive decimal value for Test Out Of.");
                    continue;
                }

                // Check if studentMark is greater than testOutOf
                if (studentMark > testOutOf)
                {
                    Console.WriteLine("Student mark cannot be greater than the total marks for the test.");
                    continue;
                }

                break; // Valid inputs received, exit the loop
            }

            // Calculate the percentage of the mark
            decimal percentage = (studentMark / testOutOf) * 100;
            string grade = CalculateGrade(percentage); // Calling the CalculateGrade method to determine the grade

            TestRecord testRecord = new TestRecord
            {
                // Create and return a new TestRecord
                StudentMark = studentMark,
                TestOutOf = testOutOf,
                Percentage = percentage,
                Grade = grade
            };

            return testRecord;
        }

        static void DeleteStudentRecord()
        {
            // Print line split in dark cyan color
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("----------------------------");
            Console.ResetColor();

            Console.Write("Enter the Student Number of the record you wish to delete: ");
            string studentNo = Console.ReadLine();

            var student = students.Find(s => s.StudentNo.Equals(studentNo, StringComparison.OrdinalIgnoreCase));

            if (student != null)
            {
                // Use the existing method to display the student's record
                DisplayStudentRecord(student);

                Console.WriteLine("Delete Options:");
                Console.WriteLine("1. Delete entire student record");
                Console.WriteLine("2. Delete a specific test record");

                // Print line split in dark cyan color
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("----------------------------");
                Console.ResetColor();

                Console.Write("Please select an option (1-2): ");
                string deleteChoice = Console.ReadLine();

                switch (deleteChoice)
                {
                    case "1":
                        Console.Write("Are you sure you want to delete the entire student record? (Y/N): ");
                        string confirmDelete = Console.ReadLine(); // Double checks the user chose the right option
                        if (confirmDelete.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            students.Remove(student); // Removes the record from the list
                            Console.WriteLine("Student record deleted successfully.");
                            SaveStudentData();
                        }
                        else
                        {
                            Console.WriteLine("Deletion canceled.");
                        }
                        break;
                    case "2":
                        Console.Write("Enter the test number you wish to delete: ");
                        int testIndex;
                        while (!int.TryParse(Console.ReadLine(), out testIndex) || testIndex < 1 || testIndex > student.TestRecords.Count)
                        {
                            Console.Write("Invalid input. Please enter a valid test number: ");
                        }
                        testIndex--; // Adjust for zero-based index
                        Console.Write("Are you sure you want to delete this test record? (Y/N): ");
                        string confirmTestDelete = Console.ReadLine();
                        if (confirmTestDelete.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            student.TestRecords.RemoveAt(testIndex); // Deletes the test record from the testIndex
                            Console.WriteLine("Test record deleted successfully.");
                            SaveStudentData(); // Updating the data
                        }
                        else
                        {
                            Console.WriteLine("Deletion canceled.");
                        }
                        break;
                    default:
                        // Error handling for any invalid inputs
                        Console.WriteLine("Invalid option. Returning to main menu.");
                        break;
                }

            }
            else
            {
                // Error handling if the program can't find the student number
                Console.WriteLine($"No record found for Student Number: {studentNo}");
            }
        }

        static string CalculateGrade(decimal percentage)
        {
            // Function to classify which mark they got in uni terms
            if (percentage >= 85)
                return "HD";
            else if (percentage >= 75)
                return "D";
            else if (percentage >= 65)
                return "C";
            else if (percentage >= 50)
                return "P";
            else
                return "F";
        }
    }
}

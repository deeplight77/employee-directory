using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeDirectory.API.Entities
{
    public class DirectoryEntry
    {
        public int EntryID { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeLocation { get; set; }

        public static List<DirectoryEntry> CreateEntries()
        {
            List<DirectoryEntry> DirectoryEntryList = new List<DirectoryEntry> 
            {
                new DirectoryEntry {EntryID = 10248, EmployeeName = "Alphonse Elric", OfficeLocation = "Houston" },
                new DirectoryEntry {EntryID = 10249, EmployeeName = "Edward Elric", OfficeLocation = "Houston" },
                new DirectoryEntry {EntryID = 10250, EmployeeName = "Jane Doe", OfficeLocation = "Houston" },
                new DirectoryEntry {EntryID = 10251, EmployeeName = "John Doe", OfficeLocation = "Austin" },
                new DirectoryEntry {EntryID = 10252, EmployeeName = "Alberto R", OfficeLocation = "Austin" }
            };

            return DirectoryEntryList;
        }
    }
}